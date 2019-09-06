using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MusicThingy.Models;
using MusicThingy.Services;
using YoutubeExplode;

namespace MusicThingy.Pages
{
    public class ManageSyncTargetsBase : ComponentBase
    {
        [Inject] protected DataRepository _repository { get; set; }
        [Inject] protected SyncService _syncservice { get; set; }

        public List<SyncTarget> SyncTargets { get; private set; } = new List<SyncTarget>();

        public SyncTarget NewSyncTarget { get; private set; } = new SyncTarget();

        protected override async Task OnInitializedAsync()
        {
            SyncTargets = await _repository.GetSyncTargets();
        }

        public async Task AddNewSyncTarget()
        {
            if (string.IsNullOrWhiteSpace(NewSyncTarget.Name)) return;
            if (string.IsNullOrWhiteSpace(NewSyncTarget.Path)) return;

            try
            {
                await _repository.AddSyncTarget(NewSyncTarget);

                NewSyncTarget = new SyncTarget();
            }
            catch (Exception) { }
        }

        public async Task RemoveSyncTarget(SyncTarget synctarget)
        {
            try { await _repository.RemoveSyncTarget(synctarget); } catch (Exception) { }
        }

        public async Task SyncAll()
        {
            try { await _syncservice.SyncAll(); } catch (Exception) { }
        }

        public async Task Sync(SyncTarget synctarget)
        {
            try { await _syncservice.Sync(synctarget); } catch (Exception) { }
        }
    }
}
