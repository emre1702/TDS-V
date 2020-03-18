using System.Collections.Generic;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        public void SpectateNext(ITDSPlayer player, bool forward)
        {
            if (player.Lifes > 0)
                return;

            if (player.Team is null || player.Team.IsSpectator)
                SpectateOtherAllTeams(player, forward);
            else
                SpectateOtherSameTeam(player, forward);
        }

        protected static void RemoveAsSpectator(ITDSPlayer character)
        {
            if (character.Spectates is null)
                return;
            character.Spectates = null;
        }

        /// <summary>
        /// Always call that before the players leaves the lobby etc. (we need his team and his index on AliveOrNotDisappearedPlayers)
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected virtual void SpectateOtherSameTeam(ITDSPlayer character, bool next = true)
        {
            ITDSPlayer currentlySpectating = character.Spectates ?? character;
            ITDSPlayer? nextPlayer;
            if (next)
                nextPlayer = GetNextSpectatePlayerInSameTeam(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInSameTeam(currentlySpectating);
            nextPlayer ??= character;

            character.Spectates = nextPlayer;
            if (nextPlayer != null)
            {
                character.Spectates = nextPlayer;
            }
        }

        protected virtual void SpectateOtherAllTeams(ITDSPlayer player, bool next = true)
        {
            ITDSPlayer currentlySpectating = player.Spectates ?? player;
            ITDSPlayer? nextPlayer;
            if (next)
                nextPlayer = GetNextSpectatePlayerInAllTeams(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInAllTeams(currentlySpectating);
            nextPlayer ??= player;

            player.Spectates = nextPlayer;
            if (nextPlayer != null)
            {
                player.Spectates = nextPlayer;
            }
        }

        private ITDSPlayer? GetNextSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            List<ITDSPlayer>? teamlist = start.Team?.SpectateablePlayers;
            if (teamlist is null || teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) + 1;
            if (startindex >= teamlist.Count - 1)
                startindex = 0;
            return teamlist[startindex];
        }

        private ITDSPlayer? GetPreviousSpectatePlayerInSameTeam(ITDSPlayer start)
        {
            List<ITDSPlayer>? teamlist = start.Team?.SpectateablePlayers;
            if (teamlist is null || teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) - 1;
            if (startindex < 0)
                startindex = teamlist.Count - 1;
            return teamlist[startindex];
        }

        private ITDSPlayer? GetNextSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            int teamIndex = start.Team?.Entity.Index ?? 0;
            if (teamIndex == 0)
                ++teamIndex;
            List<ITDSPlayer>? teamlist = Teams[teamIndex].SpectateablePlayers;
            if (teamlist is null)
                return null;
            int charIndex = teamlist.IndexOf(start) + 1;
            if (teamlist.Count == 0 || charIndex >= teamlist.Count - 1)
            {
                ITeam? team = GetNextNonSpectatorTeamWithPlayers(start.Team);
                if (team is null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist is null)
                    return null;
                charIndex = 0;
            }
            return teamlist[charIndex];
        }

        private ITDSPlayer? GetPreviousSpectatePlayerInAllTeams(ITDSPlayer start)
        {
            int teamIndex = start.Team?.Entity.Index ?? 0;
            if (teamIndex == 0)
                teamIndex = Teams.Count - 1;
            List<ITDSPlayer>? teamlist = Teams[teamIndex].SpectateablePlayers;
            if (teamlist is null)
                return null;
            int charIndex = teamlist.IndexOf(start) - 1;
            if (teamlist.Count == 0 || charIndex < 0)
            {
                ITeam? team = GetPreviousNonSpectatorTeamWithPlayers(start.Team);
                if (team is null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist is null)
                    return null;
                charIndex = teamlist.Count - 1;
            }
            return teamlist[charIndex];
        }
    }
}
