using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicThingy.Helpers;
using MusicThingy.Models;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace MusicThingy.Services
{
    public class YouTubeDownloadService : BackgroundService
    {
        private readonly ILogger<YouTubeDownloadService> _logger;
        private readonly IServiceProvider _services;
        private readonly Configuration _config;
        private readonly IHttpClientFactory _httpclientfactory;

        public YouTubeDownloadService(
            ILoggerFactory loggerfactory,
            IServiceProvider services,
            IConfiguration config,
            IHttpClientFactory httpclientfactory)
        {
            _logger = loggerfactory.CreateLogger<YouTubeDownloadService>();
            _services = services;
            _config = config.GetSection("config").Get<Configuration>();
            _httpclientfactory = httpclientfactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(YouTubeDownloadService)} started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Downloading YouTube media");

                try
                {
                    using (var scope = _services.CreateScope())
                    using (var httpclient = _httpclientfactory.CreateClient())
                    {
                        var _repository = scope.ServiceProvider.GetRequiredService<DataRepository>();
                        var _ytclient = scope.ServiceProvider.GetRequiredService<YoutubeClient>();

                        foreach (var media in (await _repository
                            .GetAllMedia())
                            .OfType<YouTubeSourceMedia>()
                            .Where(x => x.IsDownloaded == false && x.IsActive == true))
                            try { await DownloadMusicFile(httpclient, _repository, _ytclient, media); }
                            catch (Exception ex) { _logger.LogError(ex, $"An error occurred while downloading {media.YouTubeId}"); }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while downloading media");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation($"{nameof(YouTubeDownloadService)} stopped");
        }

        private async Task DownloadMusicFile(HttpClient httpclient, DataRepository _repository, YoutubeClient _ytclient, YouTubeSourceMedia media)
        {
            _logger.LogInformation($"Getting metadata for {media.YouTubeId}");

            try
            {
                var mediainfo = await _ytclient.GetVideoAsync(media.YouTubeId);
                var channel = await _ytclient.GetVideoAuthorChannelAsync(media.YouTubeId);
                MediaStreamInfoSet info = null;

                info = await _ytclient.GetVideoMediaStreamInfosAsync(media.YouTubeId);

                var audioinfo = info.Audio
                    .OrderBy(x => x.Size)
                    .Where(x => x.AudioEncoding == AudioEncoding.Aac)
                    .FirstOrDefault();

                media.FilePath = Path.Combine(media.GetType().Name, channel.Id, $"{media.YouTubeId}.m4a")
                    .GetSafePath();
                media.ArtworkPath = Path.Combine(media.GetType().Name, channel.Id, $"{media.YouTubeId}.jpg")
                    .GetSafePath();

                var downloadfilepath = Path.Combine(_config.SourcesPath, media.FilePath).GetSafePath();
                var downloadartworkpath = Path.Combine(_config.SourcesPath, media.ArtworkPath).GetSafePath();

                _logger.LogInformation($"Downloading file {downloadfilepath}");
                Directory.CreateDirectory(Path.GetDirectoryName(downloadfilepath));
                await _ytclient.DownloadMediaStreamAsync(audioinfo, downloadfilepath);

                await File.WriteAllBytesAsync(downloadartworkpath, await GetThumbnail(httpclient, mediainfo));
                media.IsDownloaded = true;
                await _repository.UpdateMedia(media);
            }
            catch (YoutubeExplode.Exceptions.VideoUnplayableException ex)
            {
                _logger.LogError(ex, $"Unable to download {media.YouTubeId}");
                media.IsActive = false;
                await _repository.UpdateMedia(media);
                return;
            }

            _logger.LogInformation($"Download for {media.YouTubeId} completed");
        }

        private static async Task<byte[]> GetThumbnail(HttpClient httpclient, YoutubeExplode.Models.Video mediainfo)
        {
            try { return await httpclient.GetByteArrayAsync(mediainfo.Thumbnails.MaxResUrl); } catch (Exception) { }
            try { return await httpclient.GetByteArrayAsync(mediainfo.Thumbnails.StandardResUrl); } catch (Exception) { }
            try { return await httpclient.GetByteArrayAsync(mediainfo.Thumbnails.HighResUrl); } catch (Exception) { }
            try { return await httpclient.GetByteArrayAsync(mediainfo.Thumbnails.MediumResUrl); } catch (Exception) { }
            return await httpclient.GetByteArrayAsync(mediainfo.Thumbnails.LowResUrl);
        }
    }
}
