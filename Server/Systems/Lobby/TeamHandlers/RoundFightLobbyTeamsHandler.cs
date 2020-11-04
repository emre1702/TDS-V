using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class RoundFightLobbyTeamsHandler : FightLobbyTeamsHandler, IRoundFightLobbyTeamsHandler
    {
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;

        public RoundFightLobbyTeamsHandler(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper, ITeamsProvider teamsProvider)
            : base(lobby, events, langHelper, teamsProvider)
        {
            events.RoundClear += RoundClear;
            events.RoundEnd += RoundEnd;
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            if (Events.RoundClear is { })
                Events.RoundClear -= RoundClear;
            if (Events.RoundEnd is { })
                Events.RoundEnd -= RoundEnd;
        }

        public Task<ITeam?> GetTeamWithHighestHp()
        {
            return Do(teams =>
            {
                var highestHp = 0;
                var teamWithHighestHp = teams[1];

                foreach (ITeam team in teams)
                {
                    var teamHp = team.Players.GetAlivesHealth(Lobby.Entity.FightSettings.StartArmor, Lobby.Entity.FightSettings.StartHealth);
                    if (teamHp > highestHp)
                    {
                        teamWithHighestHp = team;
                        highestHp = teamHp;
                    }
                    else if (teamHp == highestHp)
                        teamWithHighestHp = null;
                }

                return (ITeam?)teamWithHighestHp;
            });
        }

        private ValueTask RoundEnd()
        {
            return new ValueTask(Do(teams =>
            {
                foreach (var team in teams)
                    team.Players.ClearAlive();
            }));
        }

        private ValueTask RoundClear()
            => new ValueTask(ClearTeamPlayersAmounts());
    }
}
