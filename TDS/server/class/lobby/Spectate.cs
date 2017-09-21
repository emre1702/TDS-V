using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby {

		private Dictionary<Client, List<Client>> spectatingMe = new Dictionary<Client, List<Client>> ();

		private void SpectateTeammate ( Client player, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Class.Character character = player.GetChar ();
			Client spectating = character.spectating ?? player;
			int teamID = character.team;
			if ( this.alivePlayers[teamID].Count == 0 )
				this.SpectateAllTeams ( player, forwards, givenindex, giventeam );
			else {
				int index = this.alivePlayers[teamID].IndexOf ( spectating );
				if ( index == -1 )
					index = 0;
				if ( forwards ) {
					index++;
					if ( index >= this.alivePlayers[teamID].Count )
						index = 0;
				} else {
					index--;
					if ( index < 0 )
						index = this.alivePlayers[teamID].Count - 1;
				}
				this.Spectate ( player, this.alivePlayers[teamID][index] );
			}
		}

		private void SpectateAllTeams ( Client player, bool forwards = true, int givenindex = -1, int giventeam = -1 ) {
			Character character = player.GetChar ();
			Client spectating = character.spectating ?? player;
			int teamID = giventeam != -1 ? giventeam : spectating.GetChar ().team;
			if ( teamID == 0 )
				teamID = 1;
			int amountteams = this.alivePlayers.Count;
			int amounttried = 0;
			while ( this.alivePlayers[teamID].Count == 0 ) {
				teamID += forwards ? 1 : -1;
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
			int index = givenindex != -1 ? givenindex : this.alivePlayers[teamID].IndexOf ( spectating );
			if ( index == -1 )
				index = 0;
			if ( forwards ) {
				index++;
				if ( index >= this.alivePlayers[teamID].Count )
					index = 0;
			} else {
				index--;
				if ( index < 0 )
					index = this.alivePlayers[teamID].Count - 1;
			}
			this.Spectate ( player, this.alivePlayers[teamID][index] );
		}

		private void Spectate ( Client player, Client target ) {
			if ( player.exists ) {
				if ( target.exists ) {
					Character character = player.GetChar ();
					if ( player != target ) {
						character.spectating = target;
						player.spectate ( target );
						if ( !this.spectatingMe.ContainsKey ( target ) ) {
							this.spectatingMe[target] = new List<Client> ();
						}
						this.spectatingMe[target].Add ( player );
					} else {
						if ( character.spectating != null ) {
							Client oldspectating = character.spectating;
							if ( this.spectatingMe.ContainsKey ( oldspectating ) ) {
								this.spectatingMe[oldspectating].Remove ( player );
							}
						}
						character.spectating = null;
						player.stopSpectating ();
					}
				}
			}
		}

		private void PlayerCantBeSpectatedAnymore ( Client player, int givenindex, int giventeam ) {
			if ( this.spectatingMe.ContainsKey ( player ) ) {
				foreach ( Client spectator in this.spectatingMe[player] ) {
					if ( spectator.GetChar ().team == 0 )
						this.SpectateAllTeams ( spectator, true, givenindex, giventeam );
					else
						this.SpectateTeammate ( spectator, true, givenindex, giventeam );
				}
				this.spectatingMe.Remove ( player );
			}
		}
	}
}