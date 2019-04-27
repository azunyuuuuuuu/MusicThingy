using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicThingy.Models;
using YoutubeExplode;

namespace MusicThingy.Services
{
    public class YouTubeService : BackgroundService
    {
        private readonly ILogger<YouTubeService> _logger;
        private readonly IServiceProvider _services;

        public YouTubeService(ILoggerFactory loggerfactory, IServiceProvider services)
        {
            _logger = loggerfactory.CreateLogger<YouTubeService>();
            _services = services;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(YouTubeService)} started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Updating YouTube Sources");

                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var _repository = scope.ServiceProvider.GetRequiredService<DataRepository>();
                        var _client = scope.ServiceProvider.GetRequiredService<YoutubeClient>();

                        foreach (var source in await _repository.GetAllSources())
                        {
                            _logger.LogInformation($"Getting playlist {source.Title}");
                            var playlist = await _client.GetPlaylistAsync(source.PlaylistId);
                            var videos = playlist.Videos.Select(x => new Video
                            {
                                Id = $"YT#{x.Id}",
                                YouTubeId = x.Id,
                                Title = x.Title,
                                Author = x.Author,
                                Description = x.Description,
                                Duration = x.Duration,
                                UploadDate = x.UploadDate
                            });

                            foreach (var video in videos)
                                if (!await _repository.ContainsVideo(video))
                                    await _repository.AddVideo(video);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating sources");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation($"{nameof(YouTubeService)} stopped");
        }
    }
}
