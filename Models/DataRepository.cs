
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
        private readonly AppDbContext _context;
        private readonly ILogger<DataRepository> _logger;

        public DataRepository(ILoggerFactory loggerfactory, AppDbContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            _logger = loggerfactory.CreateLogger<DataRepository>();
        }

        private async Task SaveChanges(int tries = 3)
        {
            if (tries == 0)
                return;

            try
            {
                foreach (var item in _context.ChangeTracker.Entries().OfType<ModelBase>())
                    item.TimeChanged = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                tries--;
                _logger.LogError(ex, $"Could not save right now, retry {tries} more times");
                await SaveChanges(tries);
            }
        }

        public async Task<List<Source>> GetAllSources()
            => await _context.Sources
                .ToListAsync();

        public async Task<List<Source>> GetAllSourcesWithMedia()
            => await _context.Sources
                .Include(x => x.SourceMedias)
                .ThenInclude(x => x.Media)
                .ToListAsync();

        public async Task<Source> GetSource(string id)
            => await _context.Sources
                .SingleAsync(x => x.Id == id);

        internal async Task<List<Media>> GetAllMedia()
        {
            return await _context.Media
                .ToListAsync();
        }

        internal async Task<List<Media>> GetAllDownloadedMedia()
        {
            return await _context.Media
                .Where(x => x.IsDownloaded == true)
                .ToListAsync();
        }

        public IEnumerable<Media> GetAllMediaWriteable(Func<Media, bool> predicate)
        {
            return _context.Media
                .Where(predicate);
        }

        public async Task<Source> GetSourceWithMedia(string id)
            => await _context.Sources
                .Include(x => x.SourceMedias)
                .ThenInclude(x => x.Media)
                .SingleAsync(x => x.Id == id);

        public async Task AddSource(Source source)
        {
            await _context.Sources
                .AddAsync(source);
            await SaveChanges();
            _context.Entry(source).State = EntityState.Detached;
        }

        public async Task RemoveSource(Source source)
        {
            _context.Sources
               .Remove(source);
            await SaveChanges();
        }

        public async Task<bool> ContainsMedia(Media video)
        {
            return await _context.Media
                .ContainsAsync(video);
        }

        public async Task AddMedia(Media video)
        {
            await _context.Media
                .AddAsync(video);
            await SaveChanges();
            _context.Entry(video).State = EntityState.Detached;
        }

        public async Task UpdateMedia(Media media)
        {
            media.TimeChanged = DateTimeOffset.Now;
            _context.Media
                .Update(media);
            await SaveChanges();
        }

        public async Task AddSourceMedia(SourceMedia sourcemedia)
        {
            await _context.Set<SourceMedia>()
                .AddAsync(sourcemedia);
            await SaveChanges();
            _context.Entry(sourcemedia).State = EntityState.Detached;
        }

        public async Task<bool> ContainsSourceMedia(SourceMedia sourcemedia)
        {
            return null != await _context.Set<SourceMedia>()
                .FirstOrDefaultAsync(x =>
                    x.SourceId == sourcemedia.SourceId &&
                    x.MediaId == sourcemedia.MediaId);
        }

        public async Task<List<SyncTarget>> GetSyncTargets()
        {
            return await _context.SyncTargets
                .ToListAsync();
        }

        public async Task<SyncTarget> GetSyncTarget(int id)
        {
            return await _context.SyncTargets
                .SingleAsync(x => x.Id == id);
        }

        public async Task AddSyncTarget(SyncTarget synctarget)
        {
            await _context.SyncTargets
                .AddAsync(synctarget);
            await SaveChanges();
            _context.Entry(synctarget).State = EntityState.Detached;
        }

        public async Task UpdateSyncTarget(SyncTarget synctarget)
        {
            _context.SyncTargets.Update(synctarget);
            await SaveChanges();
            _context.Entry(synctarget).State = EntityState.Detached;
        }

        public async Task RemoveSyncTarget(SyncTarget synctarget)
        {
            _context.SyncTargets.Remove(synctarget);
            await SaveChanges();
            _context.Entry(synctarget).State = EntityState.Detached;
        }
    }
}
