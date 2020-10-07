using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class RoundFightLobbyTeamsHandler : FightLobbyTeamsHandler, IRoundFightLobbyTeamsHandler
    {
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;

        public RoundFightLobbyTeamsHandler(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper)
            : base(lobby, events, langHelper)
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
                    var teamHp = GetTeamHp(team);
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

        private int GetTeamHp(ITeam team)
        {
            if (team.AlivePlayers is null)
                return -1;
            int teamHp = 0;
            foreach (var player in team.AlivePlayers)
                teamHp += player.Health + player.Armor + ((player.Lifes - 1) * (Lobby.Entity.FightSettings.StartArmor + Lobby.Entity.FightSettings.StartHealth));
            return teamHp;
        }

        private ValueTask RoundEnd()
        {
            return new ValueTask(Do(teams =>
            {
                foreach (var team in teams)
                    team.AlivePlayers?.Clear();
            }));
        }

        private ValueTask RoundClear()
            => new ValueTask(ClearTeamPlayersAmounts());
    }
}
