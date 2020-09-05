using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        #region Public Methods

        public async override Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, null))
                return false;

            var team = player.Gang.GangLobbyTeam;
            NAPI.Task.Run(() =>
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
