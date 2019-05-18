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
    public class SyncService
    {
        private readonly ILogger<SyncService> _logger;
        private readonly Configuration _config;
        private readonly DataRepository _repository;

        public SyncService(ILoggerFactory loggerfactory, IConfiguration config, DataRepository repository)
        {
            _logger = loggerfactory.CreateLogger<SyncService>();
            _config = config.GetSection("config").Get<Configuration>();
            _repository = repository;
        }

        public async Task SyncAll()
        {
            _logger.LogInformation("Syncing...");
            foreach (var target in await _repository.GetSyncTargets())
            {
                // copy files
                _logger.LogInformation($"Syncing target {target.Name}...");

                var media = (await _repository.GetAllMedia()).Where(x => x.IsActive && x.IsDownloaded);

                foreach (var item in media)
                {
                    var sourcepath = Path.Combine(_config.DataPath, "sources", item.FilePath).GetSafePath();
                    var targetpath = Path.Combine(target.Path, item.GetType().Name, item.Artist.GetSafeFilename(), $"{item.Name}.m4a".GetSafeFilename()).GetSafePath();

                    Directory.CreateDirectory(Path.GetDirectoryName(targetpath));

                    _logger.LogInformation($"Copying {sourcepath} to {targetpath}...");
                    using (var sourcestream = File.OpenRead(sourcepath))
                    using (var targetstream = File.OpenWrite(targetpath))
                        await sourcestream.CopyToAsync(targetstream);
                }

                // create playlists
                _logger.LogInformation($"Creating playlists for {target.Name}...");
                var sources = await _repository.GetAllSourcesWithMedia();

                foreach (var source in sources)
                {
                    _logger.LogInformation($"Creating playlist {source.Title}");
                    var output = source.SourceMedias
                        .Select(x => x.Media)
                        .Where(x => x.IsActive && x.IsDownloaded)
                        .Select(x => Path.Combine(target.Path, x.GetType().Name, x.Artist, $"{x.Name}.m4a".GetSafeFilename()).GetSafePath());
                    await File.WriteAllLinesAsync(Path.Combine(target.Path, $"Source - {source.Title}.m3u"), output);
                }
            }
            _logger.LogInformation("Sync complete");
        }
    }
}
