using System.Collections.Concurrent;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby {

		private bool mixTeamsAfterRound = true;
		public List<string> teams = new List<string> { "Spectator" };
		private List<PedHash> teamSkins = new List<PedHash> { (PedHash) ( 225514697 ) };
		public ConcurrentDictionary<int,string> teamColorStrings = new ConcurrentDictionary<int,string> { [0] = "s" };
		private List<int> teamColorsList = new List<int> { 255, 255, 255 };
		private List<int> teamBlipColors = new List<int> { 0 };

		public void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
			int index = this.teams.Count;
			this.teams.Add ( name );
			this.teamSkins.Add ( hash );
			this.players.Add ( new List<Client> () );
			this.alivePlayers.Add ( new List<Client> () );
			this.spawnCounter[this.teamSkins.Count - 1] = 0;
			this.teamColorStrings.TryAdd ( index, colorstring );
			Color rgb = Manager.Colors.fontColor[colorstring];
			this.teamBlipColors.Add ( Manager.Colors.blipColorByString[colorstring] );
			this.teamColorsList.Add ( rgb.red );
			this.teamColorsList.Add ( rgb.green );
			this.teamColorsList.Add ( rgb.blue );
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
					if ( this.players[i][j].exists ) {
						int teamID = this.GetTeamIDWithFewestMember ( newplayerslist );
						newplayerslist[teamID].Add ( players[i][j] );
						this.players[i][j].setSkin ( this.teamSkins[teamID] );
					}
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