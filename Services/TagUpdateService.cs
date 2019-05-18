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

namespace MusicThingy.Services
{
    public class TagUpdateService : BackgroundService
    {
        private readonly ILogger<TagUpdateService> _logger;
        private readonly IServiceProvider _services;
        private readonly Configuration _config;

        public TagUpdateService(ILoggerFactory loggerfactory, IServiceProvider services, IConfiguration config)
        {
            _logger = loggerfactory.CreateLogger<TagUpdateService>();
            _services = services;
            _config = config.GetSection("config").Get<Configuration>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(YouTubeFetchingService)} started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Updating media tags on disk");

                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var _repository = scope.ServiceProvider.GetRequiredService<DataRepository>();

                        foreach (var media in await _repository.GetAllDownloadedMedia())
                        {
                            var mediapath = Path.Combine(_config.SourcesPath, media.FilePath).GetSafePath();
                            var artworkpath = Path.Combine(_config.SourcesPath, media.ArtworkPath).GetSafePath();

                            using (var stream = File.Open(mediapath, FileMode.Open))
                            {
                                var tagfile = TagLib.File.Create(new TagLib.StreamFileAbstraction(stream.Name, stream, stream));

                                if (
                                    media.Name != tagfile.Tag.Title ||
                                    media.Artist != tagfile.Tag.FirstPerformer ||
                                    media.Description != tagfile.Tag.Comment
                                )
                                {
                                    _logger.LogInformation($"Updating metadata for file {mediapath}");

                                    tagfile.Tag.Title = media.Name;
                                    tagfile.Tag.Performers = new[] { media.Artist };
                                    // tagfile.Tag.AlbumArtists = new[] { media.YouTubeUploader };
                                    tagfile.Tag.Comment = media.Description;
                                    tagfile.Save();
                                }

                                if (tagfile.Tag.Pictures.Count() == 0 && File.Exists(artworkpath))
                                {
                                    _logger.LogInformation($"Updating thumbnail for file {mediapath}");
                                    using (var imagestream = File.OpenRead(artworkpath))
                                        tagfile.Tag.Pictures = new TagLib.IPicture[] {
                                            new TagLib.Picture(new TagLib.StreamFileAbstraction(imagestream.Name, imagestream, imagestream))
                                        };
                                    tagfile.Save();
                                }
                            }
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
