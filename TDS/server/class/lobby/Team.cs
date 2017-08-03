using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby : Script {

		private bool mixTeamsAfterRound = true;
		public List<string> teams = new List<string> { "Spectator" };
		private List<PedHash> teamSkins = new List<PedHash> { (PedHash) ( 225514697 ) };
		private List<Color> teamColors = new List<Color> {  };

		public void AddTeam ( string name, PedHash hash ) {
			this.teams.Add ( name );
			this.teamSkins.Add ( hash );
			this.players.Add ( new List<Client> () );
			this.spawnCounter[this.teamSkins.Count - 1] = 0;
		}

		private int GetTeamIDWithFewestMember ( List<List<Client>> newplayerlist ) {
			int lastteamID = 1;
			int lastteamcount = newplayerlist[1].Count;
			for ( int k = 2; k < newplayerlist.Count; k++ ) {
				int count = newplayerlist[k].Count;
				if ( count < lastteamcount || count == lastteamcount && Manager.Utility.rnd.Next ( 1, 2 ) == 1 ) {
					lastteamID = k;
					lastteamcount = count;
				}
			}
			return lastteamID;
		}

		private void MixTeams ( ) {
			List<List<Client>> newplayerslist = new List<List<Client>> { new List<Client> () };
			for ( int i = 0; i < this.players[0].Count; i++ ) {
				newplayerslist[0].Add ( this.players[0][i] );
			}
			for ( int i = 1; i < this.players.Count; i++ )
				newplayerslist.Add ( new List<Client> () );
			for ( int i = 1; i < this.players.Count; i++ ) {
				this.spawnCounter[i] = 0;
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					int teamID = this.GetTeamIDWithFewestMember ( newplayerslist );
					newplayerslist[teamID].Add ( players[i][j] );
					this.players[i][j].setSkin ( this.teamSkins[teamID] );
				}
			}
			this.players = new List<List<Client>> ( newplayerslist );
		}

		private int GetTeamAmountStillInRound ( int minalive = 1 ) {
			int amount = 0;
			for ( int i = 1; i < this.alivePlayers.Count; i++ )
				if ( this.alivePlayers[i].Count >= minalive )
					amount++;
			return amount;
		}

		public string GetTeamName ( int ID ) {
			return teams[ID];
		}

	}
}