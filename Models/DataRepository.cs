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

        public async Task<Source> GetSource(string id)
            => await _context.Sources
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

        public async Task<Source> GetSourceWithVideos(string id)
            => await _context.Sources
                .AsNoTracking()
                .Include(x => x.SourceVideos)
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

        public async Task<bool> ContainsVideo(Video video)
        {
            return await _context.Videos
                .AsNoTracking()
                .ContainsAsync(video);
        }

        public async Task AddVideo(Video video)
        {
            await _context.Videos
                .AddAsync(video);
            await SaveChanges();
            _context.Entry(video).State = EntityState.Detached;
        }
    }
}
