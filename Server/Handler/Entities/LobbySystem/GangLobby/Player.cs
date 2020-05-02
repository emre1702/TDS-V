﻿using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        public async override Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, null))
                return false;

            var team = player.Gang.GangLobbyTeam;
            ModAPI.Thread.RunInMainThread(() =>
            {
                SetPlayerTeam(player, team);

                player.ModPlayer?.Freeze(false);
                player.ModPlayer?.SetInvincible(true);

                var spawnPoint = player.Gang.House?.Position ?? SpawnPoint;
                var spawnRotation = player.Gang.House?.SpawnRotation ?? Entity.DefaultSpawnRotation;
                player.Spawn(spawnPoint, spawnRotation);
                player.ModPlayer?.Freeze(false);
            });
            
            return true;
        }
    }
}
