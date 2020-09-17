using System.Collections.Generic;
using GTANetworkAPI;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class BaseLobbyMapHandler
    {
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint>();

        public uint Dimension { get; }
        protected Vector3 SpawnPoint { get; }

        public BaseLobbyMapHandler(LobbyDb entity)
        {
            Dimension = GetFreeDimension();

            SpawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );
        }

        private static uint GetFreeDimension()
        {
            uint tryid = 1;
            while (_dimensionsUsed.Contains(tryid))
                ++tryid;
            return tryid;
        }
    }
}
