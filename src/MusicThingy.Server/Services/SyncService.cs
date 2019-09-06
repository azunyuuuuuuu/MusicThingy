using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicThingy.Server.Helpers;
using MusicThingy.Server.Models;

namespace MusicThingy.Server.Services
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
                await Sync(target);
            }
            _logger.LogInformation("Sync complete");
        }

        public async Task Sync(SyncTarget synctarget)
        {
            _logger.LogInformation($"Syncing target {synctarget.Name}...");

            var media = (await _repository.GetAllMedia()).Where(x => x.IsActive && x.IsDownloaded);

            foreach (var item in media)
            {
                var sourcepath = Path.Combine(_config.DataPath, "sources", item.FilePath).GetSafePath();
                var targetpath = Path.Combine(synctarget.Path, item.GetType().Name, item.Artist.GetSafeFilename(), $"{item.Name}.m4a".GetSafeFilename()).GetSafePath();

                Directory.CreateDirectory(Path.GetDirectoryName(targetpath));

                if (File.Exists(targetpath) && item.TimeChanged < File.GetLastWriteTimeUtc(targetpath))
                    continue;

                _logger.LogInformation($"Copying {sourcepath} to {targetpath}...");
                using (var sourcestream = File.OpenRead(sourcepath))
                using (var targetstream = File.OpenWrite(targetpath))
                    await sourcestream.CopyToAsync(targetstream);
            }

            // create playlists
            _logger.LogInformation($"Creating playlists for {synctarget.Name}...");
            var sources = await _repository.GetAllSourcesWithMedia();

            foreach (var source in sources)
            {
                _logger.LogInformation($"Creating playlist {source.Title}");
                var output = source.SourceMedias
                    .Select(x => x.Media)
                    .Where(x => x.IsActive && x.IsDownloaded)
                    .Select(x => Path.Combine(x.GetType().Name, x.Artist.GetSafeFilename(), $"{x.Name}.m4a".GetSafeFilename()).GetSafePath())
                    .OrderBy(x => x);
                await File.WriteAllLinesAsync(Path.Combine(synctarget.Path, $"Source - {source.Title}.m3u".GetSafeFilename()), output);
            }
        }
    }
}
