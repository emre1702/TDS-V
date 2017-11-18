namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using player;

	partial class Lobby {

		private Dictionary<Client, List<Client>> spectatingMe = new Dictionary<Client, List<Client>> ();

		private void SpectateTeammate ( Client player, bool forwards = true, int givenIndex = -1, int givenTeam = -1 ) {
			Character character = player.GetChar ();
			Client spectating = character.Spectating ?? player;
			uint teamID = character.Team;
			if ( this.alivePlayers[(int)teamID].Count == 0 )
				this.SpectateAllTeams ( player, forwards, givenIndex, givenTeam );
			else {
				int index = this.alivePlayers[(int)teamID].IndexOf ( spectating );
				if ( index == -1 )
					index = 0;
				if ( forwards ) {
					index++;
					if ( index >= this.alivePlayers[(int)teamID].Count )
						index = 0;
				} else {
					index--;
					if ( index < 0 )
						index = this.alivePlayers[(int)teamID].Count - 1;
				}
				this.Spectate ( player, this.alivePlayers[(int)teamID][index] );
			}
		}

		private void SpectateAllTeams ( Client player, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Character character = player.GetChar ();
			Client spectating = character.Spectating ?? player;
			uint teamID = giventeam != -1 ? (uint) giventeam : spectating.GetChar ().Team;
			if ( teamID == 0 )
				teamID = 1;
			uint amountteams = (uint) this.alivePlayers.Count;
			uint amounttried = 0;
			while ( this.alivePlayers[(int)teamID].Count == 0 ) {
				teamID += (uint) ( forwards ? 1 : -1 );
				amounttried++;
				givenindex = -1;
				if ( teamID == 0 )
					teamID = amountteams - 1;
				else if ( teamID == amountteams )
					teamID = 0;
				if ( amounttried == amountteams ) {
					this.Spectate ( player, player );
					return;
				}
			}
			int index = givenindex != -1 ? givenindex : this.alivePlayers[(int)teamID].IndexOf ( spectating );
			if ( index == -1 )
				index = 0;
			if ( forwards ) {
				index++;
				if ( index >= this.alivePlayers[(int)teamID].Count )
					index = 0;
			} else {
				index--;
				if ( index < 0 )
					index = this.alivePlayers[(int)teamID].Count - 1;
			}
			this.Spectate ( player, this.alivePlayers[(int)teamID][index] );
		}

		private void Spectate ( Client player, Client target ) {
			if ( player.Exists ) {
				if ( target.Exists ) {
					Character character = player.GetChar ();
					if ( player != target ) {
						character.Spectating = target;
						player.Spectate ( target );
						if ( !this.spectatingMe.ContainsKey ( target ) ) {
							this.spectatingMe[target] = new List<Client> ();
						}
						this.spectatingMe[target].Add ( player );
					} else {
						if ( character.Spectating != null ) {
							Client oldspectating = character.Spectating;
							if ( this.spectatingMe.ContainsKey ( oldspectating ) ) {
								this.spectatingMe[oldspectating].Remove ( player );
							}
						}
						character.Spectating = null;
						player.StopSpectating ();
					}
				}
			}
		}

		private void PlayerCantBeSpectatedAnymore ( Client player, int givenindex, int giventeam ) {
			if ( this.spectatingMe.ContainsKey ( player ) ) {
				for ( int i = this.spectatingMe[player].Count - 1; i >= 0; i-- ) {
					if ( this.spectatingMe[player][i].GetChar ().Team == 0 )
						this.SpectateAllTeams ( this.spectatingMe[player][i], true, givenindex, giventeam );
					else
						this.SpectateTeammate ( this.spectatingMe[player][i], true, givenindex, giventeam );
				}
				this.spectatingMe.Remove ( player );
			}
		}
	}

}
