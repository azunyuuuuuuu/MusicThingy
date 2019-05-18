﻿using System;
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
                        var _client = scope.ServiceProvider.GetRequiredService<YoutubeClient>();

                        foreach (var media in _repository
                            .GetAllMediaWriteable(x => x.IsDownloaded == false && x.IsActive == true)
                            .Take(1)
                            .Cast<YouTubeSourceMedia>())
                        {
                            _logger.LogInformation($"Getting metadata for {media.YouTubeId}");

                            var mediainfo = await _client.GetVideoAsync(media.YouTubeId);
                            var channel = await _client.GetVideoAuthorChannelAsync(media.YouTubeId);
                            var info = await _client.GetVideoMediaStreamInfosAsync(media.YouTubeId);
                            var audioinfo = info.Audio
                                .OrderBy(x => x.Size)
                                .Where(x => x.AudioEncoding == AudioEncoding.Aac)
                                .FirstOrDefault();

                            if (audioinfo == null)
                            {
                                _logger.LogInformation($"Video {media.YouTubeId} has no suitable music data 😢");
                                continue;
                            }
                            media.FilePath = Path.Combine(media.GetType().Name, channel.Id, $"{media.YouTubeId}.m4a")
                                .GetSafePath();
                            media.ArtworkPath = Path.Combine(media.GetType().Name, channel.Id, $"{media.YouTubeId}.jpg")
                                .GetSafePath();

                            var downloadpath = Path.Combine(_config.SourcesPath, media.FilePath).GetSafePath();
                            var artworkpath = Path.Combine(_config.SourcesPath, media.ArtworkPath).GetSafePath();

                            _logger.LogInformation($"Downloading file {downloadpath}");
                            Directory.CreateDirectory(Path.GetDirectoryName(downloadpath));
                            await _client.DownloadMediaStreamAsync(audioinfo, downloadpath);
                            await File.WriteAllBytesAsync(artworkpath, await GetThumbnail(httpclient, mediainfo));
                            media.IsDownloaded = true;
                            await _repository.SaveChanges();

                            _logger.LogInformation($"Download for {media.YouTubeId} completed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while downloading media");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
            }

            _logger.LogInformation($"{nameof(YouTubeDownloadService)} stopped");
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
