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
    public class IndexBase : ComponentBase
    {
        [Inject] protected DataRepository _repository { get; set; }

        public List<Video> Videos { get; set; } = new List<Video>();

        protected override async Task OnInitAsync()
        {
            Videos.AddRange((await _repository.GetAllVideos())
                .OrderBy(x => x.Title));
        }
    }
}
