using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class MapCreatorLobbyMapHandler : BaseLobbyMapHandler
    {
        public MapCreatorLobbyMapHandler(IMapCreatorLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.Spawn(SpawnPoint, SpawnRotation);
                data.Player.Freeze(false);
            });
            return default;
        }
    }
}
