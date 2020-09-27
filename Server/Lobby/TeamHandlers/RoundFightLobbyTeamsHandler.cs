using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Handler.Helper;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class RoundFightLobbyTeamsHandler : FightLobbyTeamsHandler
    {
        public RoundFightLobbyTeamsHandler(LobbyDb entity, IRoundFightLobbyEventsHandler events, LangHelper langHelper, IBaseLobbyPlayers players)
            : base(entity, events, langHelper, players)
        {
            events.RoundClear += RoundClear;
            events.RoundEnd += RoundEnd;
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
                teamHp += player.Health + player.Armor + ((player.Lifes - 1) * (Entity.FightSettings.StartArmor + Entity.FightSettings.StartHealth));
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
