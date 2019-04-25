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

        public List<Source> Sources { get; set; } = new List<Source>();

        protected override async Task OnInitAsync()
        {
            Sources.AddRange((await _repository.GetAllSources())
                .OrderBy(x => x.Title));
        }
    }
}
