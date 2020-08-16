using AltV.Net.Async;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

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
            await AltAsync.Do(() =>
            {
                SetPlayerTeam(player, team);

                player.Freeze(false);
                player.SetInvincible(true);

                var spawnPoint = player.Gang.House?.Position ?? SpawnPoint;
                var spawnRotation = player.Gang.House?.SpawnRotation ?? Entity.DefaultSpawnRotation;
                player.Spawn(spawnPoint, spawnRotation);
                player.Freeze(false);
            });

            return true;
        }

        #endregion Public Methods
    }
}
