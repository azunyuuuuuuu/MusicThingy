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
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject] protected DataRepository _repository { get; set; }

        public List<Media> Videos { get; set; } = new List<Media>();

        protected override async Task OnInitializedAsync()
        {
            Videos.AddRange((await _repository.GetAllMedia())
                .OrderBy(x => x.Name));
        }

        public void Dispose()
        {
            Videos.Clear();
        }
    }
}
