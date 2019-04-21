using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MusicThingy.Models;
using YoutubeExplode;

namespace MusicThingy.Pages
{
    public class ManageSourcesBase : ComponentBase
    {
        [Inject] protected AppDbContext _context { get; set; }
        [Inject] protected YoutubeClient _ytclient { get; set; }

        public List<Source> Sources { get; private set; } = new List<Source>();
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

            var playlistid = YoutubeClient.ParsePlaylistId(NewSourceUrl);

            var playlist = await _ytclient.GetPlaylistAsync(playlistid);

            var item = new Source
            {
                PlaylistId = playlistid,
                Title = playlist.Title,
                Author = playlist.Author,
                Description = playlist.Description,
            };

            NewSourceUrl = string.Empty;

            await _context.Sources.AddAsync(item);
            await _context.SaveChangesAsync();
            await RebuildList();
            StateHasChanged();
        }

        public async Task RemoveSource(Guid id)
        {
            _context.Sources.Remove(Sources.Single(x => x.SourceId == id));
            await _context.SaveChangesAsync();
            await RebuildList();
        }
    }
}
