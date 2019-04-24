using System;
using System.Collections.Generic;
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

        public async Task<Source> GetSource(Guid id)
            => await _context.Sources
                .AsNoTracking()
                .SingleAsync(x => x.SourceId == id);

        public async Task<Source> GetSourceWithVideos(Guid id)
            => await _context.Sources
                .AsNoTracking()
                .Include(x => x.SourceVideos)
                .SingleAsync(x => x.SourceId == id);

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
    }
}
