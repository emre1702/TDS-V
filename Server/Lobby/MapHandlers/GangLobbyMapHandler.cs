using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class GangLobbyMapHandler : BaseLobbyMapHandler
    {
        public GangLobbyMapHandler(GangLobby lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            NAPI.Task.Run(() =>
            {
                player.Spawn(player.Gang.SpawnPosition ?? SpawnPoint, player.Gang.SpawnHeading ?? SpawnRotation);
                player.Freeze(false);
            });
        }
    }
}
