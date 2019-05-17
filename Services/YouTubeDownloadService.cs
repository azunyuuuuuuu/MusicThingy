using System;
using System.IO;
using System.Linq;
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

        public YouTubeDownloadService(ILoggerFactory loggerfactory, IServiceProvider services, IConfiguration config)
        {
            _logger = loggerfactory.CreateLogger<YouTubeDownloadService>();
            _services = services;
            _config = config.GetSection("config").Get<Configuration>();
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
                    {
                        var _repository = scope.ServiceProvider.GetRequiredService<DataRepository>();
                        var _client = scope.ServiceProvider.GetRequiredService<YoutubeClient>();

                        foreach (var media in _repository
                            .GetAllMediaWriteable(x => x.IsDownloaded == false && x.IsActive == true)
                            .Take(1)
                            .Cast<YouTubeSourceMedia>())
                        {
                            _logger.LogInformation($"Getting metadata for {media.YouTubeId}");

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
                            media.FilePath = Path.Combine("YouTube", channel.Id, $"{media.YouTubeId}.m4a")
                                .GetSafePath();

                            var downloadpath = Path.Combine(_config.SourcesPath, media.FilePath).GetSafePath();

                            _logger.LogInformation($"Downloading file {downloadpath}");
                            Directory.CreateDirectory(Path.GetDirectoryName(downloadpath));
                            await _client.DownloadMediaStreamAsync(audioinfo, downloadpath);
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

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation($"{nameof(YouTubeDownloadService)} stopped");
        }
    }
}
