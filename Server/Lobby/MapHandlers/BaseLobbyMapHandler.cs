using System.Collections.Generic;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class BaseLobbyMapHandler : IBaseLobbyMapHandler
    {
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint>();

        public uint Dimension { get; }
        public Vector3 SpawnPoint { get; }
        public float SpawnRotation { get; }
        protected readonly LobbyDb Entity;

        public BaseLobbyMapHandler(LobbyDb entity, IBaseLobbyEventsHandler events)
        {
            Entity = entity;
            Dimension = GetFreeDimension();

            SpawnPoint = new Vector3(
                entity.DefaultSpawnX,
                entity.DefaultSpawnY,
                entity.DefaultSpawnZ
            );
            SpawnRotation = entity.DefaultSpawnRotation;

            events.PlayerJoined += Events_PlayerJoined;
        }

        protected virtual void Events_PlayerJoined(ITDSPlayer player, int _)
        {
        }

        public virtual void DeleteMapBlips()
        {
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
