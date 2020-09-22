using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class MapCreatorLobbyMapHandler : BaseLobbyMapHandler
    {
        private readonly MapCreatorLobbySync _sync;

        public MapCreatorLobbyMapHandler(LobbyDb entity, IBaseLobbyEventsHandler events, MapCreatorLobbySync sync)
            : base(entity, events)
        {
            _sync = sync;
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            NAPI.Task.Run(() =>
            {
                player.Spawn(SpawnPoint, SpawnRotation);
                player.Freeze(false);
            });
        }
    }
}
