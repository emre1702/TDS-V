using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Maps;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class GangActionLobbyMapHandler : RoundFightLobbyMapHandler
    {
        public GangActionLobbyMapHandler(IGangActionLobby lobby, IRoundFightLobbyEventsHandler events, ISettingsHandler settingsHandler, MapsLoadingHandler mapsLoadingHandler)
            : base(lobby, events, settingsHandler, mapsLoadingHandler)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.Run(() =>
            {
                data.Player.Spawn(data.Player.Position);
                data.Player.Freeze(false);
            });
            return default;
        }
    }
}
