using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MusicThingy.Models;

namespace MusicThingy.Pages
{
    public class ManageSourcesBase : ComponentBase
    {
        [Inject] protected AppDbContext _context { get; set; }

        public List<YouTubeSource> Sources { get; private set; } = new List<YouTubeSource>();
        public string NewSourceUrl { get; set; } = string.Empty;

        protected override async Task OnInitAsync()
        {
            await RebuildList();
        }

        private async Task RebuildList()
        {
            Sources.Clear();
            Sources.AddRange(await _context.Sources.AsNoTracking().ToListAsync());
        }

        public async Task AddSource()
        {
            if (string.IsNullOrWhiteSpace(NewSourceUrl))
                return;

            var item = new YouTubeSource
            {
                Url = NewSourceUrl
            };

            NewSourceUrl = string.Empty;

            await _context.Sources.AddAsync(item);
            await _context.SaveChangesAsync();
            await RebuildList();
        }

        public async Task RemoveSource(Guid id)
        {
            _context.Sources.Remove(Sources.Single(x => x.YouTubeSourceId == id));
            await _context.SaveChangesAsync();
            await RebuildList();
        }
    }
}
