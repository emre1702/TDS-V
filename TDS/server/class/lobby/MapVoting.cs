using System.Collections.Generic;
using System.Linq;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby : Script {
		private Dictionary<string, int> mapVotes = new Dictionary<string, int>();
		private Dictionary<Client, string> playerVotes = new Dictionary<Client, string> ();

		private void SendMapsForVoting ( Client player ) {
			if ( this.mapNames != null ) {
				player.triggerEvent ( "onMapMenuOpen", this.mapNames, this.mapDescriptions );
			}
		}

		private void AddVoteToMap ( Client player, string mapname ) {
			if ( playerVotes.ContainsKey ( player ) ) {
				if ( playerVotes[player] == mapname )
					return;
				mapVotes[playerVotes[player]]--;
				this.SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname, playerVotes[player] );
			} else
				this.SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname );
			playerVotes[player] = mapname;
			mapVotes[mapname]++;
		}

		private void AddMapToVoting ( Client player, string mapname ) {
			if ( !mapVotes.ContainsKey ( mapname ) ) {
				// Anti-Cheat //
				if ( !this.mapNames.Contains ( mapname ) ) {
					Manager.Log.Error ( player.socialClubName + " voted for " + mapname + ", but it doesn't exist!", this.name );
					return;
				}
				///////////////
				mapVotes[mapname] = 0;
				this.SendAllPlayerEvent ( "onNewMapForVoting", -1, mapname );
			}
			this.AddVoteToMap ( player, mapname );
		}

		private Map GetNextMap ( ) {
			if ( mapVotes.Count > 0 ) {
				string wonmap = mapVotes.Aggregate ( ( l, r ) => l.Value > r.Value ? l : r ).Key;
				this.SendAllPlayerLangNotification ( "map_won_voting", -1, wonmap );
				return Manager.Map.GetMapClass ( wonmap, this );
			} else
				return this.GetRandomMap ();
		}

		private void SyncMapVotingOnJoin ( Client player ) {
			if ( mapVotes.Count > 0 ) {
				List<string> mapnames = new List<string> ();
				List<int> mapvotes = new List<int> ();

				foreach ( KeyValuePair<string, int> pair in this.mapVotes.OrderByDescending ( p => p.Value ) ) {
					mapnames.Add ( pair.Key );
					mapvotes.Add ( pair.Value );
				}
				player.triggerEvent ( "onMapVotingSyncOnJoin", mapnames, mapvotes );
			}
		}
	}
}
