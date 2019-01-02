using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{

    partial class FightLobby
    {
        public void SpectateNext(TDSPlayer player, bool forward)
        {
            if (player.Lifes > 0)
                return;

            if (player.Team.IsSpectatorTeam)
                SpectateOtherAllTeams(player, forward);
            else
                SpectateOtherSameTeam(player, forward);
        }

        protected void RemoveAsSpectator(TDSPlayer character)
        {
            character.Client.StopSpectating();
            if (character.Spectates == null)
                return;
            TDSPlayer spectating = character.Spectates;
            character.Spectates = null;
            spectating.Spectators.Remove(character);
        }

        /// <summary>
        /// Always call that before the players leaves the lobby etc. (we need his team and his index on AliveOrNotDisappearedPlayers)
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected virtual void SpectateOtherSameTeam(TDSPlayer character, bool next = true)
        {
            TDSPlayer currentlySpectating = character.Spectates ?? character;
            TDSPlayer nextPlayer;
            if (next)
                nextPlayer = GetNextSpectatePlayerInSameTeam(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInSameTeam(currentlySpectating);
            nextPlayer = nextPlayer ?? character;

            currentlySpectating.Spectators.Remove(character);
            character.Spectates = nextPlayer;
            if (nextPlayer == null)
            {
                character.Client.StopSpectating();
            }
            else
            {
                nextPlayer.Spectators.Add(character);
                character.Client.Spectate(nextPlayer.Client);
            } 
        }

        protected virtual void SpectateOtherAllTeams(TDSPlayer character, bool next = true)
        {
            TDSPlayer currentlySpectating = character.Spectates ?? character;
            TDSPlayer nextPlayer;
            if (next)
                nextPlayer = GetNextSpectatePlayerInAllTeams(currentlySpectating);
            else
                nextPlayer = GetPreviousSpectatePlayerInAllTeams(currentlySpectating);
            nextPlayer = nextPlayer ?? character;

            currentlySpectating.Spectators.Remove(character);
            character.Spectates = nextPlayer;
            if (nextPlayer == null)
            {
                character.Client.StopSpectating();
            }
            else
            {
                nextPlayer.Spectators.Add(character);
                character.Client.Spectate(nextPlayer.Client);
            }
        }
        
        private TDSPlayer GetNextSpectatePlayerInSameTeam(TDSPlayer start)
        {
            uint teamIndex = start.Team.Index;
            List<TDSPlayer> teamlist = SpectateablePlayers[teamIndex];
            if (teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) + 1;
            if (startindex >= teamlist.Count - 1)
                startindex = 0;
            return teamlist[startindex];
        }

        private TDSPlayer GetPreviousSpectatePlayerInSameTeam(TDSPlayer start)
        {
            uint teamIndex = start.Team.Index;
            List<TDSPlayer> teamlist = SpectateablePlayers[teamIndex];
            if (teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) - 1;
            if (startindex < 0)
                startindex = teamlist.Count - 1;
            return teamlist[startindex];
        }

        private TDSPlayer GetNextSpectatePlayerInAllTeams(TDSPlayer start)
        {
            uint teamIndex = start.Team.Index;
            List<TDSPlayer> teamlist = SpectateablePlayers[teamIndex];
            int charIndex = teamlist.IndexOf(start) + 1;
            if (teamlist.Count == 0 || charIndex >= teamlist.Count - 1)
            {
                Teams team = GetNextNonSpectatorTeamWithPlayers(start.Team);
                if (team == null)
                {
                    return null;
                }
                teamlist = SpectateablePlayers[team.Index];
                charIndex = 0;
            }
            return teamlist[charIndex];
        }

        private TDSPlayer GetPreviousSpectatePlayerInAllTeams(TDSPlayer start)
        {
            uint teamIndex = start.Team.Index;
            List<TDSPlayer> teamlist = SpectateablePlayers[teamIndex];
            int charIndex = teamlist.IndexOf(start) - 1;
            if (teamlist.Count == 0 || charIndex < 0)
            {
                Teams team = GetPreviousNonSpectatorTeamWithPlayers(start.Team);
                if (team == null)
                {
                    return null;
                }
                teamlist = SpectateablePlayers[team.Index];
                charIndex = teamlist.Count - 1;
            }
            return teamlist[charIndex];
        }
    }
}
