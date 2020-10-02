using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class BaseLobbyMapHandler : IBaseLobbyMapHandler
    {
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint>();

        public uint Dimension { get; }
        public Vector3 SpawnPoint { get; }
        public float SpawnRotation { get; }
        protected IBaseLobby Lobby { get; }

        private readonly List<ITDSBlip> _mapBlips = new List<ITDSBlip>();

        public BaseLobbyMapHandler(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            Dimension = GetFreeDimension();

            SpawnPoint = new Vector3(
                lobby.Entity.DefaultSpawnX,
                lobby.Entity.DefaultSpawnY,
                lobby.Entity.DefaultSpawnZ
            );
            SpawnRotation = lobby.Entity.DefaultSpawnRotation;

            events.PlayerJoined += Events_PlayerJoined;
        }

        protected virtual ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            return default;
        }

        public void AddMapBlip(ITDSBlip blip)
        {
            lock (_mapBlips)
            {
                _mapBlips.Add(blip);
            }
        }

        public void DeleteMapBlips()
        {
            lock (_mapBlips)
            {
                foreach (var blip in _mapBlips)
                    blip.Delete();
                _mapBlips.Clear();
            }
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
