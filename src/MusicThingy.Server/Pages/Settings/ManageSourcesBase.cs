﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MusicThingy.Server.Models;
using YoutubeExplode;

namespace MusicThingy.Server.Pages
{
    public class ManageSourcesBase : ComponentBase
    {
        [Inject] protected DataRepository _repository { get; set; }
        [Inject] protected YoutubeClient _ytclient { get; set; }

        public List<Source> Sources { get; private set; } = new List<Source>();
        public string NewSourceUrl { get; set; } = string.Empty;

        public bool IsAdd { get; set; } = false;

        protected override async Task OnInitializedAsync()
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
            try
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
            }
            catch (Exception) { }
            finally
            {
                await RebuildList();

                IsAdd = false;

                StateHasChanged();
            }
        }

        public async Task RemoveSource(Source source)
        {
            try { await _repository.RemoveSource(source); } catch (Exception) { }
            await RebuildList();
            StateHasChanged();
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
