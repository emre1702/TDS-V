using System.Collections.Generic;
using TDS.Entity;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{

    partial class FightLobby
    {
        protected void RemoveAsSpectator(Character character)
        {
            character.Player.StopSpectating();
            if (character.Spectates == null)
                return;
            Character spectating = character.Spectates;
            character.Spectates = null;
            spectating.Spectators.Remove(character);
        }

        /// <summary>
        /// Always call that before the players leaves the lobby etc. (we need his team and his index on AliveOrNotDisappearedPlayers)
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected virtual void SpectateNextSameTeam(Character character)
        {
            Character currentlySpectating = character.Spectates ?? character;
            Character next = GetNextSpectatePlayerInSameTeam(currentlySpectating) ?? character;

            currentlySpectating.Spectators.Remove(character);
            character.Spectates = next;
            if (next == null)
            {
                character.Player.StopSpectating();
            }
            else
            {
                next.Spectators.Add(character);
                character.Player.Spectate(next.Player);
            } 
        }

        protected virtual void SpectateNextAllTeams(Character character)
        {
            Character currentlySpectating = character.Spectates ?? character;
            Character next = GetNextSpectatePlayerInAllTeams(currentlySpectating) ?? character;

            currentlySpectating.Spectators.Remove(character);
            character.Spectates = next;
            if (next == null)
            {
                character.Player.StopSpectating();
            }
            else
            {
                next.Spectators.Add(character);
                character.Player.Spectate(next.Player);
            }
        }

        
        private Character GetNextSpectatePlayerInSameTeam(Character start)
        {
            uint teamIndex = start.Team.Index;
            List<Character> teamlist = AliveOrNotDisappearedPlayers[teamIndex];
            if (teamlist.Count == 0)
                return null;
            int startindex = teamlist.IndexOf(start) + 1;
            if (startindex >= teamlist.Count - 1)
                startindex = 0;
            return teamlist[startindex];
        }

        private Character GetNextSpectatePlayerInAllTeams(Character start)
        {
            uint teamIndex = start.Team.Index;
            List<Character> teamlist = AliveOrNotDisappearedPlayers[teamIndex];
            int charIndex = teamlist.IndexOf(start);
            if (teamlist.Count == 0 || charIndex >= teamlist.Count - 1)
            {
                Teams team = GetNextNonSpectatorTeamWithPlayers(start.Team);
                if (team == null)
                {
                    return null;
                }
                teamlist = AliveOrNotDisappearedPlayers[team.Index];
                charIndex = -1;
            }
            return teamlist[charIndex + 1];
        }




        /*

        public void RespawnPlayerInSpectateMode ( Character character ) {
            SpectateTeammate ( character );
            NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientPlayerSpectateMode" );
        }

        public void SpectateTeammate ( Character character, bool forwards = true, int givenIndex = -1, int givenTeam = -1 ) {
			Character spectating = character.Spectating ?? character;
			int teamID = character.Team;
			if ( AlivePlayers[teamID].Count == 0 )
				SpectateAllTeams ( character, forwards, givenIndex, givenTeam );
			else {
				int index = AlivePlayers[teamID].IndexOf ( spectating );
				if ( index == -1 )
					index = 0;
				if ( forwards ) {
					index++;
					if ( index >= AlivePlayers[teamID].Count )
						index = 0;
				} else {
					index--;
					if ( index < 0 )
						index = AlivePlayers[teamID].Count - 1;
				}
				Spectate ( character, AlivePlayers[teamID][index] );
			}
		}

		public void SpectateAllTeams ( Character character, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Character spectating = character.Spectating ?? character;
			int teamID = giventeam != -1 ? giventeam : spectating.Team;
			if ( teamID == 0 )
				teamID = 1;
			int amountteams = AlivePlayers.Count;
			uint amounttried = 0;
			while ( AlivePlayers[teamID].Count == 0 ) {
				teamID += ( forwards ? 1 : -1 );
				amounttried++;
				givenindex = -1;
				if ( teamID == 0 )
					teamID = amountteams - 1;
				else if ( teamID == amountteams )
					teamID = 0;
				if ( amounttried == amountteams ) {
					Spectate ( character, character );
					return;
				}
			}
			int index = givenindex != -1 ? givenindex : AlivePlayers[teamID].IndexOf ( spectating );
			if ( index == -1 )
				index = 0;
			if ( forwards ) {
				index++;
				if ( index >= AlivePlayers[teamID].Count )
					index = 0;
			} else {
				index--;
				if ( index < 0 )
					index = AlivePlayers[teamID].Count - 1;
			}
			Spectate ( character, AlivePlayers[teamID][index] );
		}

		private void Spectate ( Character character, Character targetcharacter ) {
			if ( character.Player.Exists ) {
				if ( targetcharacter.Player.Exists ) {
                    Client player = character.Player;
                    Client target = targetcharacter.Player;
                    if ( player != target ) {
						character.Spectating = targetcharacter;
						player.Spectate ( target );
						if ( !spectatingMe.ContainsKey ( targetcharacter ) ) {
							spectatingMe[targetcharacter] = new List<Character> ();
						}
						spectatingMe[targetcharacter].Add ( character );
					} else {
						if ( character.Spectating != null ) {
							Character oldspectating = character.Spectating;
							if ( spectatingMe.ContainsKey ( oldspectating ) ) {
								spectatingMe[oldspectating].Remove ( character );
							}
						}
						character.Spectating = null;
						player.StopSpectating ();
					}
				}
			}
		}

		private void PlayerCantBeSpectatedAnymore ( Character character, int givenindex, int giventeam ) {
			if ( spectatingMe.ContainsKey ( character ) ) {
				for ( int i = spectatingMe[character].Count - 1; i >= 0; i-- ) {
					if ( spectatingMe[character][i].Team == 0 )
						SpectateAllTeams ( spectatingMe[character][i], true, givenindex, giventeam );
					else
						SpectateTeammate ( spectatingMe[character][i], true, givenindex, giventeam );
				}
				spectatingMe.Remove ( character );
			}
		}*/
    }

}
