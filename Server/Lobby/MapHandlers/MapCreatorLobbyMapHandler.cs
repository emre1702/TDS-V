using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class MapCreatorLobbyMapHandler : BaseLobbyMapHandler
    {
        public MapCreatorLobbyMapHandler(MapCreatorLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
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
