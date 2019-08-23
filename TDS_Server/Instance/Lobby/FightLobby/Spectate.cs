using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public void SpectateNext(TDSPlayer player, bool forward)
        {
            if (player.Lifes > 0)
                return;

            if (player.Team == null || player.Team.IsSpectator)
                SpectateOtherAllTeams(player, forward);
            else
                SpectateOtherSameTeam(player, forward);
        }

        protected void RemoveAsSpectator(TDSPlayer character)
        {
            if (character.Spectates == null)
                return;
            character.Spectates = null;
        }

        /// <summary>
        /// Always call that before the players leaves the lobby etc. (we need his team and his index on AliveOrNotDisappearedPlayers)
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected virtual void SpectateOtherSameTeam(TDSPlayer character, bool next = true)
        {
            TDSPlayer currentlySpectating = character.Spectates ?? character;
            TDSPlayer? nextPlayer;
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

        protected virtual void SpectateOtherAllTeams(TDSPlayer character, bool next = true)
        {
            TDSPlayer currentlySpectating = character.Spectates ?? character;
            TDSPlayer? nextPlayer;
            if (next)
                nextPlayer = GetNextSpectatePlayerInAllTeams(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInAllTeams(currentlySpectating);
            nextPlayer ??= character;

            character.Spectates = nextPlayer;
            if (nextPlayer != null)
            {
                character.Spectates = nextPlayer;
            }
        }

        private TDSPlayer? GetNextSpectatePlayerInSameTeam(TDSPlayer start)
        {
            List<TDSPlayer>? teamlist = start.Team?.SpectateablePlayers;
            if (teamlist == null || teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) + 1;
            if (startindex >= teamlist.Count - 1)
                startindex = 0;
            return teamlist[startindex];
        }

        private TDSPlayer? GetPreviousSpectatePlayerInSameTeam(TDSPlayer start)
        {
            List<TDSPlayer>? teamlist = start.Team?.SpectateablePlayers;
            if (teamlist == null || teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) - 1;
            if (startindex < 0)
                startindex = teamlist.Count - 1;
            return teamlist[startindex];
        }

        private TDSPlayer? GetNextSpectatePlayerInAllTeams(TDSPlayer start)
        {
            int teamIndex = start.Team?.Entity.Index ?? 0;
            if (teamIndex == 0)
                ++teamIndex;
            List<TDSPlayer>? teamlist = Teams[teamIndex].SpectateablePlayers;
            if (teamlist == null)
                return null;
            int charIndex = teamlist.IndexOf(start) + 1;
            if (teamlist.Count == 0 || charIndex >= teamlist.Count - 1)
            {
                Team? team = GetNextNonSpectatorTeamWithPlayers(start.Team);
                if (team == null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist == null)
                    return null;
                charIndex = 0;
            }
            return teamlist[charIndex];
        }

        private TDSPlayer? GetPreviousSpectatePlayerInAllTeams(TDSPlayer start)
        {
            int teamIndex = start.Team?.Entity.Index ?? 0;
            if (teamIndex == 0)
                teamIndex = Teams.Length - 1;
            List<TDSPlayer>? teamlist = Teams[teamIndex].SpectateablePlayers;
            if (teamlist == null)
                return null;
            int charIndex = teamlist.IndexOf(start) - 1;
            if (teamlist.Count == 0 || charIndex < 0)
            {
                Team? team = GetPreviousNonSpectatorTeamWithPlayers(start.Team);
                if (team == null)
                    return null;
                teamlist = team.SpectateablePlayers;
                if (teamlist == null)
                    return null;
                charIndex = teamlist.Count - 1;
            }
            return teamlist[charIndex];
        }
    }
}