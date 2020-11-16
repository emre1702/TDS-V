using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class MainMenuMapHandler : BaseLobbyMapHandler
    {
        public MainMenuMapHandler(IMainMenu lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.Spawn(SpawnPoint.Around(Lobby.Entity.AroundSpawnPoint), SpawnRotation);
                data.Player.Freeze(true);
            });
            return default;
        }
    }
}
