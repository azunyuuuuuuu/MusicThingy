
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MusicThingy.Models
{
    public class DataRepository
    {
        private readonly ILogger<DataRepository> _logger;
        private readonly Func<AppDbContext> _factory;

        public DataRepository(ILoggerFactory loggerfactory, Func<AppDbContext> factory)
        {
            _factory = factory;
            _logger = loggerfactory.CreateLogger<DataRepository>();
        }

        private async Task SaveChanges()
        {
            using (var _context = _factory())
            {
                foreach (var item in _context.ChangeTracker.Entries().OfType<ModelBase>())
                    item.TimeChanged = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Source>> GetAllSources()
        {
            using (var _context = _factory())
            {
                return await _context.Sources
                  .ToListAsync();
            }
        }

        public async Task<List<Source>> GetAllSourcesWithMedia()
        {
            using (var _context = _factory())
            {
                return await _context.Sources
                  .Include(x => x.SourceMedias)
                  .ThenInclude(x => x.Media)
                  .ToListAsync();
            }
        }

        public async Task<Source> GetSource(string id)
        {
            using (var _context = _factory())
            {
                return await _context.Sources
                  .SingleAsync(x => x.Id == id);
            }
        }

        internal async Task<List<Media>> GetAllMedia()
        {
            using (var _context = _factory())
            {
                return await _context.Media
                    .ToListAsync();
            }
        }

        internal async Task<List<Media>> GetAllDownloadedMedia()
        {
            using (var _context = _factory())
            {
                return await _context.Media
                    .Where(x => x.IsDownloaded == true)
                    .ToListAsync();
            }
        }

        public async Task<Source> GetSourceWithMedia(string id)
        {
            using (var _context = _factory())
            {
                return await _context.Sources
                  .Include(x => x.SourceMedias)
                  .ThenInclude(x => x.Media)
                  .SingleAsync(x => x.Id == id);
            }
        }

        public async Task AddSource(Source source)
        {
            using (var _context = _factory())
            {
                await _context.Sources
                    .AddAsync(source);
                await SaveChanges();
                _context.Entry(source).State = EntityState.Detached;
            }
        }

        public async Task RemoveSource(Source source)
        {
            using (var _context = _factory())
            {
                _context.Sources
                   .Remove(source);
                await SaveChanges();
            }
        }

        public async Task<bool> ContainsMedia(Media video)
        {
            using (var _context = _factory())
            {
                return await _context.Media
                    .ContainsAsync(video);
            }
        }

        public async Task AddMedia(Media video)
        {
            using (var _context = _factory())
            {
                await _context.Media
                    .AddAsync(video);
                await SaveChanges();
                _context.Entry(video).State = EntityState.Detached;
            }
        }

        public async Task UpdateMedia(Media media)
        {
            using (var _context = _factory())
            {
                media.TimeChanged = DateTimeOffset.Now;
                _context.Media
                    .Update(media);
                await SaveChanges();
            }
        }

        public async Task AddSourceMedia(SourceMedia sourcemedia)
        {
            using (var _context = _factory())
            {
                await _context.Set<SourceMedia>()
                    .AddAsync(sourcemedia);
                await SaveChanges();
                _context.Entry(sourcemedia).State = EntityState.Detached;
            }
        }

        public async Task<bool> ContainsSourceMedia(SourceMedia sourcemedia)
        {
            using (var _context = _factory())
            {
                return null != await _context.Set<SourceMedia>()
                    .FirstOrDefaultAsync(x =>
                        x.SourceId == sourcemedia.SourceId &&
                        x.MediaId == sourcemedia.MediaId);
            }
        }

        public async Task<List<SyncTarget>> GetSyncTargets()
        {
            using (var _context = _factory())
            {
                return await _context.SyncTargets
                    .ToListAsync();
            }
        }

        public async Task<SyncTarget> GetSyncTarget(int id)
        {
            using (var _context = _factory())
            {
                return await _context.SyncTargets
                    .SingleAsync(x => x.Id == id);
            }
        }

        public async Task AddSyncTarget(SyncTarget synctarget)
        {
            using (var _context = _factory())
            {
                await _context.SyncTargets
                    .AddAsync(synctarget);
                await SaveChanges();
                _context.Entry(synctarget).State = EntityState.Detached;
            }
        }

        public async Task UpdateSyncTarget(SyncTarget synctarget)
        {
            using (var _context = _factory())
            {
                _context.SyncTargets.Update(synctarget);
                await SaveChanges();
                _context.Entry(synctarget).State = EntityState.Detached;
            }
        }

        public async Task RemoveSyncTarget(SyncTarget synctarget)
        {
            using (var _context = _factory())
            {
                _context.SyncTargets.Remove(synctarget);
                await SaveChanges();
                _context.Entry(synctarget).State = EntityState.Detached;
            }
        }
    }
}
