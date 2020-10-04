using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class MapCreatorLobbyMapHandler : BaseLobbyMapHandler
    {
        public MapCreatorLobbyMapHandler(IMapCreatorLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.Run(() =>
            {
                data.Player.Spawn(SpawnPoint, SpawnRotation);
                data.Player.Freeze(false);
            });
            return default;
        }
    }
}
