using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Default;

namespace TDS_Server.DamageSystem.KillingSprees
{
    internal class KillingSpreeSoundsHandler
    {
        private static readonly Dictionary<short, string> _longTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [3] = CustomSound.KillingSpree,
            [4] = CustomSound.Dominating,
            [5] = CustomSound.MultiKill,
            [6] = CustomSound.Unstoppable,
            [7] = CustomSound.WickedSick,
            [8] = CustomSound.MonsterKill,
            [9] = CustomSound.Godlike,
            [10] = CustomSound.HolyShit
        };

        private static readonly Dictionary<short, string> _shortTimeKillingSpreeSounds = new Dictionary<short, string>
        {
            [2] = CustomSound.DoubleKill,
            [3] = CustomSound.TripleKill,
            [4] = CustomSound.UltraKill,
            [5] = CustomSound.Rampage
        };

        private static readonly short _minLongTimeKillingSpreeKill = _longTimeKillingSpreeSounds.Keys.Min();
        private static readonly short _minShortTimeKillingSpreeKill = _shortTimeKillingSpreeSounds.Keys.Min();

        private static readonly short _maxLongTimeKillingSpreeKill = _longTimeKillingSpreeSounds.Keys.Max();
        private static readonly short _maxShortTimeKillingSpreeKill = _shortTimeKillingSpreeSounds.Keys.Max();

        internal void PlayerDeath(ITDSPlayer killer)
        {
            if (killer.Deathmatch.KillingSpree <= 1)
                return;

            if (killer.Deathmatch.ShortTimeKillingSpree >= _minShortTimeKillingSpreeKill)
                PlayShortTimeKillingSpree(killer);
            if (killer.Deathmatch.KillingSpree >= _minLongTimeKillingSpreeKill)
                PlayLongTimeKillingSpree(killer);
        }

        private void PlayShortTimeKillingSpree(ITDSPlayer killer)
        {
            var playSoundIndex = Math.Min(killer.Deathmatch.ShortTimeKillingSpree, _maxShortTimeKillingSpreeKill);
            var soundName = _shortTimeKillingSpreeSounds[playSoundIndex];
            NAPI.Task.RunSafe(() =>
                killer.Lobby?.Sync.TriggerEvent(ToClientEvent.PlayCustomSound, soundName));
            //if (player.KillingSpree <= 5)
            //    playLongTimeKillSound = false;
        }

        private void PlayLongTimeKillingSpree(ITDSPlayer killer)
        {
            var playSoundIndex = Math.Min(killer.Deathmatch.KillingSpree, _maxLongTimeKillingSpreeKill);
            var soundName = _longTimeKillingSpreeSounds[playSoundIndex];
            NAPI.Task.RunSafe(() =>
                killer.Lobby?.Sync.TriggerEvent(ToClientEvent.PlayCustomSound, soundName));
        }
    }
}
