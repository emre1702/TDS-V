using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Spectator
{
    public class FightLobbySpectator
    {
        private readonly FightLobbyTeamsHandler _teams;

        public FightLobbySpectator(FightLobbyTeamsHandler teams)
        {
            _teams = teams;
        }

        public async Task SpectateNext(ITDSPlayer player, bool forward)
        {
            if (player.Lifes > 0)
                return;

            if (player.Team is null || player.Team.IsSpectator)
                await SpectateOtherAllTeams(player, forward);
            else
                SpectateOtherSameTeam(player, forward);
        }

        internal virtual async Task SpectateOtherAllTeams(ITDSPlayer player, bool spectateNext = true)
        {
            var currentlySpectating = player.Spectates ?? player;
            ITDSPlayer? nextPlayer;
            if (spectateNext)
                nextPlayer = await GetNextSpectatePlayerInAllTeams(currentlySpectating);
            else
                nextPlayer = await GetPreviousSpectatePlayerInAllTeams(currentlySpectating);
            nextPlayer ??= currentlySpectating;

            player.Spectates = nextPlayer;
        }

        internal virtual void SpectateOtherSameTeam(ITDSPlayer player, bool spectateNext = true, bool ignoreSource = false)
        {
            var currentlySpectating = player.Spectates ?? player;
            ITDSPlayer? nextPlayer;
            if (spectateNext)
                nextPlayer = GetNextSpectatePlayerInSameTeam(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInSameTeam(currentlySpectating);
            nextPlayer ??= player;

            if (ignoreSource && player == nextPlayer)
                return;
            player.Spectates = nextPlayer;
        }

        private async Task<ITDSPlayer?> GetNextSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            var teamIndex = start.Team?.Entity.Index ?? 0;
            if (teamIndex == 0)
                ++teamIndex;
            var teamlist = (await _teams.GetTeam(teamIndex)).SpectateablePlayers;
            if (teamlist is null)
                return null;
            var charIndex = teamlist.IndexOf(start) + 1;
            if (teamlist.Count == 0 || charIndex >= teamlist.Count - 1)
            {
                var team = await _teams.GetNextNonSpectatorTeamWithPlayers(start.Team);
                if (team is null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist is null)
                    return null;
                charIndex = 0;
            }
            return teamlist[charIndex];
        }

        private ITDSPlayer? GetNextSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            var teamlist = start.Team?.SpectateablePlayers;
            if (teamlist is null || teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) + 1;
            if (startindex >= teamlist.Count - 1)
                startindex = 0;
            return teamlist[startindex];
        }

        private async Task<ITDSPlayer?> GetPreviousSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            var teamIndex = (int?)start.Team?.Entity.Index ?? 0;
            var teamlist = await _teams.Do(teams =>
            {
                if (teamIndex == 0)
                    teamIndex = teams.Count - 1;
                return teams[teamIndex].SpectateablePlayers;
            });

            if (teamlist is null)
                return null;
            var charIndex = teamlist.IndexOf(start) - 1;
            if (teamlist.Count == 0 || charIndex < 0)
            {
                var team = await _teams.GetPreviousNonSpectatorTeamWithPlayers(start.Team);
                if (team is null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist is null)
                    return null;
                charIndex = teamlist.Count - 1;
            }
            return teamlist[charIndex];
        }

        private ITDSPlayer? GetPreviousSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            var teamlist = start.Team?.SpectateablePlayers;
            if (teamlist is null || teamlist.Count == 0)
                return null;
            var startindex = teamlist.IndexOf(start) - 1;
            if (startindex < 0)
                startindex = teamlist.Count - 1;
            return teamlist[startindex];
        }
    }
}
