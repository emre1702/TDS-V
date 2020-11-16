using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class BaseLobbyMapHandler : IBaseLobbyMapHandler
    {
        private static readonly HashSet<uint> _dimensionsUsed = new HashSet<uint>();

        public uint Dimension { get; }
        public Vector3 SpawnPoint { get; }
        public float SpawnRotation { get; }
        protected IBaseLobby Lobby { get; }
        protected IBaseLobbyEventsHandler Events { get; }

        private readonly List<ITDSBlip> _mapBlips = new List<ITDSBlip>();

        public BaseLobbyMapHandler(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            Dimension = GetFreeDimension();
            Events = events;

            SpawnPoint = new Vector3(
                lobby.Entity.DefaultSpawnX,
                lobby.Entity.DefaultSpawnY,
                lobby.Entity.DefaultSpawnZ
            );
            SpawnRotation = lobby.Entity.DefaultSpawnRotation;

            events.PlayerJoined += Events_PlayerJoined;
            events.RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            if (Events.PlayerJoined is { })
                Events.PlayerJoined -= Events_PlayerJoined;
            Events.RemoveAfter -= RemoveEvents;
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
            NAPI.Task.RunSafe(() =>
            {
                lock (_mapBlips)
                {
                    foreach (var blip in _mapBlips)
                        blip.Delete();
                    _mapBlips.Clear();
                }
            });
        }

        private static uint GetFreeDimension()
        {
            uint tryid = 1;
            lock (_dimensionsUsed)
            {
                while (_dimensionsUsed.Contains(tryid))
                    ++tryid;
            }
            return tryid;
        }
    }
}
