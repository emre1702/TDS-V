using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Models.Map;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Spectator
{
    public class RoundFightLobbySpectator : FightLobbySpectator, IRoundFightLobbySpectator
    {
        public Vector3 CurrentMapSpectatorPosition { get; private set; } = new Vector3();
        private readonly IRoundFightLobbyEventsHandler _events;

        public RoundFightLobbySpectator(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby)
        {
            _events = events;

            events.InitNewMap += Events_InitNewMap;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby lobby)
        {
            _events.InitNewMap -= Events_InitNewMap;
            _events.RemoveAfter -= RemoveEvents;
        }

        private void Events_InitNewMap(MapDto map)
        {
            CurrentMapSpectatorPosition = map.LimitInfo.Center?.ToVector3().AddToZ(10) ?? map.TeamSpawnsList.TeamSpawns[0].Spawns[0].ToVector3().AddToZ(10);
        }

        public async ValueTask SetPlayerCantBeSpectatedAnymore(ITDSPlayer player)
        {
            player.Team?.Players.RemoveSpectatable(player);

            if (player.SpectateHandler.HasSpectators())
            {
                // ToList because the list gets changed in both methods
                foreach (var spectator in player.SpectateHandler.GetSpectators())
                    await SpectateNext(spectator, true).ConfigureAwait(false);
            }
        }
    }
}
