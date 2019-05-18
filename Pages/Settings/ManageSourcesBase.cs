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
        [Inject] protected DataRepository _repository { get; set; }
        [Inject] protected YoutubeClient _ytclient { get; set; }

        public List<Source> Sources { get; private set; } = new List<Source>();
        public string NewSourceUrl { get; set; } = string.Empty;

        public bool IsAdd { get; set; } = false;

        protected override async Task OnInitAsync()
        {
            await RebuildList();
        }

        private async Task RebuildList()
        {
            Sources.Clear();
            Sources.AddRange(await _repository.GetAllSources());
        }

        public async Task AddSource()
        {
            if (string.IsNullOrWhiteSpace(NewSourceUrl))
                return;

            var playlistid = YoutubeClient.ParsePlaylistId(NewSourceUrl);

            var playlist = await _ytclient.GetPlaylistAsync(playlistid);

            var item = new Source
            {
                Id = $"YTPl#{playlistid}",
                PlaylistId = playlistid,
                Title = playlist.Title,
                Author = playlist.Author,
                Description = playlist.Description,
            };

            NewSourceUrl = string.Empty;

            await _repository.AddSource(item);
            await RebuildList();

            IsAdd = false;

            StateHasChanged();
        }

        public async Task RemoveSource(Source source)
        {
            await _repository.RemoveSource(source);
            await RebuildList();
        }

        public void OpenModal()
        {
            NewSourceUrl = string.Empty;
            IsAdd = true;
        }

        public void CloseModal()
        {
            NewSourceUrl = string.Empty;
            IsAdd = false;
        }
    }
}
