using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Maps;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class GangActionLobbyMapHandler : RoundFightLobbyMapHandler
    {
        public GangActionLobbyMapHandler(IGangActionLobby lobby, IRoundFightLobbyEventsHandler events, ISettingsHandler settingsHandler, MapsLoadingHandler mapsLoadingHandler)
            : base(lobby, events, settingsHandler, mapsLoadingHandler)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.Spawn(data.Player.Position);
                data.Player.Freeze(false);
            });
            return default;
        }
    }
}
