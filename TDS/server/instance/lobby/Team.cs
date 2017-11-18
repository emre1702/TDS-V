namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;

	partial class Lobby {

		private bool mixTeamsAfterRound = true;
		public List<string> Teams = new List<string> {
			"Spectator"
		};
		private readonly List<PedHash> teamSkins = new List<PedHash> {
			(PedHash) ( 225514697 )
		};
		public Dictionary<uint, string> TeamColorStrings = new Dictionary<uint, string> {
			[0] = "s"
		};
		private readonly List<int> teamColorsList = new List<int> {
			255,
			255,
			255
		};
		private readonly List<int> teamBlipColors = new List<int> {
			0
		};

		public void AddTeam ( string name, PedHash hash, string colorstring = "s" ) {
			uint index = (uint) this.Teams.Count;
			this.Teams.Add ( name );
			this.teamSkins.Add ( hash );
			this.Players.Add ( new List<Client> () );
			this.alivePlayers.Add ( new List<Client> () );
			this.spawnCounter[(uint)this.teamSkins.Count - 1] = 0;
			this.TeamColorStrings[index] = colorstring;
			Color rgb = Colors.FontColor[colorstring];
			this.teamBlipColors.Add ( Colors.BlipColorByString[colorstring] );
			this.teamColorsList.Add ( rgb.Red );
			this.teamColorsList.Add ( rgb.Green );
			this.teamColorsList.Add ( rgb.Blue );
		}

		private uint GetTeamIDWithFewestMember ( List<List<Client>> newplayerlist ) {
			uint lastteamID = 1;
			int lastteamcount = newplayerlist[1].Count;
			for ( uint k = 2; k < newplayerlist.Count; k++ ) {
				int count = newplayerlist[(int)k].Count;
				if ( count < lastteamcount || count == lastteamcount && Utility.Rnd.Next ( 1, 2 ) == 1 ) {
					lastteamID = k;
					lastteamcount = count;
				}
			}
			return lastteamID;
		}

		private void MixTeams () {
			List<List<Client>> newplayerslist = new List<List<Client>> {
				new List<Client> ()
			};
			for ( int i = 0; i < this.Players[0].Count; i++ ) {
				newplayerslist[0].Add ( this.Players[0][i] );
			}
			for ( int i = 1; i < this.Players.Count; i++ )
				newplayerslist.Add ( new List<Client> () );
			for ( uint i = 1; i < this.Players.Count; i++ ) {
				this.spawnCounter[i] = 0;
				foreach ( Client player in this.Players[(int)i] ) {
					if ( player.Exists ) {
						int teamID = (int) this.GetTeamIDWithFewestMember ( newplayerslist );
						newplayerslist[teamID].Add ( player );
						player.SetSkin ( this.teamSkins[teamID] );
					}
				}
			}
			this.Players = new List<List<Client>> ( newplayerslist );
		}

		private int GetTeamAmountStillInRound ( int minalive = 1 ) {
			int amount = 0;
			for ( int i = 1; i < this.alivePlayers.Count; i++ )
				if ( this.alivePlayers[i].Count >= minalive )
					amount++;
			return amount;
		}

		public string GetTeamName ( uint id ) {
			return this.Teams[(int)id];
		}

	}

}
