using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class MainMenuMapHandler : BaseLobbyMapHandler
    {
        public MainMenuMapHandler(IMainMenu lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.Run(() =>
            {
                data.Player.Spawn(SpawnPoint.Around(Lobby.Entity.AroundSpawnPoint), SpawnRotation);
                data.Player.Freeze(true);
            });
            return default;
        }
    }
}
