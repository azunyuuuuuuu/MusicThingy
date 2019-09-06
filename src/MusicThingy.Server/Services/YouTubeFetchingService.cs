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
using MusicThingy.Helpers;
using MusicThingy.Models;
using YoutubeExplode;

namespace MusicThingy.Services
{
    public class YouTubeFetchingService : BackgroundService
    {
        private readonly ILogger<YouTubeFetchingService> _logger;
        private readonly IServiceProvider _services;

        public YouTubeFetchingService(ILoggerFactory loggerfactory, IServiceProvider services)
        {
            _logger = loggerfactory.CreateLogger<YouTubeFetchingService>();
            _services = services;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(YouTubeFetchingService)} started");

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
                            var videos = playlist.Videos.Select(x => new YouTubeSourceMedia
                            {
                                Id = $"YT#{x.Id}",
                                YouTubeId = x.Id,
                                YouTubeTitle = x.Title,
                                YouTubeUploader = x.Author,
                                YouTubeDescription = x.Description,
                                YouTubeDuration = x.Duration,
                                YouTubeUploadDate = x.UploadDate,
                                Name = x.Title,
                                Artist = x.Author.RegexReplace(" - Topic$", string.Empty),
                                Time = x.Duration,
                                // Album = source.Title,
                                Description = x.Description,
                            });

                            foreach (var video in videos)
                                if (!await _repository.ContainsMedia(video))
                                    await _repository.AddMedia(video);

                            foreach (var item in videos.Select(video => new SourceMedia
                            {
                                SourceId = source.Id,
                                MediaId = video.Id
                            }))
                                if (!await _repository.ContainsSourceMedia(item))
                                    await _repository.AddSourceMedia(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating sources");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation($"{nameof(YouTubeFetchingService)} stopped");
        }
    }
}
