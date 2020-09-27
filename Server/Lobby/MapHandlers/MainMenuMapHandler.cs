using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class MainMenuMapHandler : BaseLobbyMapHandler
    {
        public MainMenuMapHandler(MainMenu lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            NAPI.Task.Run(() =>
            {
                player.Spawn(SpawnPoint.Around(Lobby.Entity.AroundSpawnPoint), SpawnRotation);
                player.Freeze(true);
            });
        }
    }
}
