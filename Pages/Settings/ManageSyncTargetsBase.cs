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
    public class ManageSyncTargetsBase : ComponentBase
    {
        [Inject] protected DataRepository _repository { get; set; }

        public List<SyncTarget> SyncTargets { get; private set; } = new List<SyncTarget>();

        public SyncTarget NewSyncTarget { get; private set; } = new SyncTarget();

        protected override async Task OnInitAsync()
        {
            SyncTargets = await _repository.GetSyncTargets();
        }

        public async Task AddNewSyncTarget()
        {
            if (string.IsNullOrWhiteSpace(NewSyncTarget.Name)) return;
            if (string.IsNullOrWhiteSpace(NewSyncTarget.Path)) return;

            await _repository.AddSyncTarget(NewSyncTarget);

            NewSyncTarget = new SyncTarget();
        }

        public async Task RemoveSyncTarget(SyncTarget synctarget)
        {
            await _repository.RemoveSyncTarget(synctarget);
        }
    }
}
