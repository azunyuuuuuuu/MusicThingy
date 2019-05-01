using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicThingy.Models;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace MusicThingy.Services
{
    public class YouTubeDownloadService : BackgroundService
    {
        private readonly ILogger<YouTubeDownloadService> _logger;
        private readonly IServiceProvider _services;

        public YouTubeDownloadService(ILoggerFactory loggerfactory, IServiceProvider services)
        {
            _logger = loggerfactory.CreateLogger<YouTubeDownloadService>();
            _services = services;
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
                            .Take(5)
                            .Cast<YouTubeSourceMedia>())
                        {
                            _logger.LogInformation($"Getting metadata for {media.YouTubeId}");
                            var info = await _client.GetVideoMediaStreamInfosAsync(media.YouTubeId);
                            var audioinfo = info.Audio
                                .OrderBy(x => x.Size)
                                .Where(x => x.AudioEncoding == AudioEncoding.Aac)
                                .FirstOrDefault();

                            if (audioinfo == null)
                            {
                                _logger.LogInformation($"Video {media.Title} has no suitable music :(");
                                continue;
                            }
                            media.FilePath = Path.Combine(media.Author.GetSafeFilename(), $"[{media.YouTubeId}] {media.Title.GetSafeFilename()}.m4a");

                            _logger.LogInformation($"Downloading file {media.FilePath}");
                            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine("data", media.FilePath)));
                            await _client.DownloadMediaStreamAsync(audioinfo, Path.Combine("data", media.FilePath));
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
