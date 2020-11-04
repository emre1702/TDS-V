using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class DamageTestLobbyMapHandler : BaseLobbyMapHandler
    {
        public DamageTestLobbyMapHandler(IBaseLobby lobby, IBaseLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.Spawn(SpawnPoint, SpawnRotation);
                data.Player.Freeze(false);
                data.Player.SetInvisible(false);
            });
            return default;
        }
    }
}
