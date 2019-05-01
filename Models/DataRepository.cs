using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MusicThingy.Models
{
    public class DataRepository
    {
        private readonly AppDbContext _context;

        public DataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChanges()
            => await _context.SaveChangesAsync();

        public async Task<List<Source>> GetAllSources()
            => await _context.Sources
                .AsNoTracking()
                .ToListAsync();

        public async Task<Source> GetSource(string id)
            => await _context.Sources
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

        internal async Task<List<Media>> GetAllMedia()
        {
            return await _context.Media.ToListAsync();
        }

        public IEnumerable<Media> GetAllMediaWriteable(Func<Media, bool> predicate)
        {
            return _context.Media
                .Where(predicate);
        }

        public async Task<Source> GetSourceWithMedia(string id)
            => await _context.Sources
                .AsNoTracking()
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
                .AsNoTracking()
                .ContainsAsync(video);
        }

        public async Task AddMedia(Media video)
        {
            await _context.Media
                .AddAsync(video);
            await SaveChanges();
            _context.Entry(video).State = EntityState.Detached;
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
    }
}
