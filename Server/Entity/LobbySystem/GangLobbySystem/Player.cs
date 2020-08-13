using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Entity.LobbySystem.GangLobbySystem
{
    partial class GangLobby
    {
        #region Public Methods

        public async override Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, null))
                return false;

            var team = player.Gang.GangLobbyTeam;
            ModAPI.Thread.QueueIntoMainThread(() =>
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

        #endregion Public Methods
    }
}
