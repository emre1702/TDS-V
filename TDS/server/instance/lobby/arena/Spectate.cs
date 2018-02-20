namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using player;

	partial class Arena {

		private Dictionary<Client, List<Client>> spectatingMe = new Dictionary<Client, List<Client>> ();

        public void RespawnPlayerInSpectateMode ( Client player ) {
            SpectateTeammate ( player );
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerSpectateMode" );
        }

        public void SpectateTeammate ( Client player, bool forwards = true, int givenIndex = -1, int givenTeam = -1 ) {
			Character character = player.GetChar ();
			Client spectating = character.Spectating ?? player;
			int teamID = character.Team;
			if ( alivePlayers[teamID].Count == 0 )
				SpectateAllTeams ( player, forwards, givenIndex, givenTeam );
			else {
				int index = alivePlayers[teamID].IndexOf ( spectating );
				if ( index == -1 )
					index = 0;
				if ( forwards ) {
					index++;
					if ( index >= alivePlayers[teamID].Count )
						index = 0;
				} else {
					index--;
					if ( index < 0 )
						index = alivePlayers[teamID].Count - 1;
				}
				Spectate ( player, alivePlayers[teamID][index] );
			}
		}

		public void SpectateAllTeams ( Client player, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Character character = player.GetChar ();
			Client spectating = character.Spectating ?? player;
			int teamID = giventeam != -1 ? giventeam : spectating.GetChar ().Team;
			if ( teamID == 0 )
				teamID = 1;
			int amountteams = alivePlayers.Count;
			uint amounttried = 0;
			while ( alivePlayers[teamID].Count == 0 ) {
				teamID += ( forwards ? 1 : -1 );
				amounttried++;
				givenindex = -1;
				if ( teamID == 0 )
					teamID = amountteams - 1;
				else if ( teamID == amountteams )
					teamID = 0;
				if ( amounttried == amountteams ) {
					Spectate ( player, player );
					return;
				}
			}
			int index = givenindex != -1 ? givenindex : alivePlayers[teamID].IndexOf ( spectating );
			if ( index == -1 )
				index = 0;
			if ( forwards ) {
				index++;
				if ( index >= alivePlayers[teamID].Count )
					index = 0;
			} else {
				index--;
				if ( index < 0 )
					index = alivePlayers[teamID].Count - 1;
			}
			Spectate ( player, alivePlayers[teamID][index] );
		}

		private void Spectate ( Client player, Client target ) {
			if ( player.Exists ) {
				if ( target.Exists ) {
					Character character = player.GetChar ();
					if ( player != target ) {
						character.Spectating = target;
						player.Spectate ( target );
						if ( !spectatingMe.ContainsKey ( target ) ) {
							spectatingMe[target] = new List<Client> ();
						}
						spectatingMe[target].Add ( player );
					} else {
						if ( character.Spectating != null ) {
							Client oldspectating = character.Spectating;
							if ( spectatingMe.ContainsKey ( oldspectating ) ) {
								spectatingMe[oldspectating].Remove ( player );
							}
						}
						character.Spectating = null;
						player.StopSpectating ();
					}
				}
			}
		}

		private void PlayerCantBeSpectatedAnymore ( Client player, int givenindex, int giventeam ) {
			if ( spectatingMe.ContainsKey ( player ) ) {
				for ( int i = spectatingMe[player].Count - 1; i >= 0; i-- ) {
					if ( spectatingMe[player][i].GetChar ().Team == 0 )
						SpectateAllTeams ( spectatingMe[player][i], true, givenindex, giventeam );
					else
						SpectateTeammate ( spectatingMe[player][i], true, givenindex, giventeam );
				}
				spectatingMe.Remove ( player );
			}
		}
	}

}
