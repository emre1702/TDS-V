using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby {
		private Dictionary<string, int> mapVotes = new Dictionary<string, int>();
		private Dictionary<Client, string> playerVotes = new Dictionary<Client, string> ();

		private void SendMapsForVoting ( Client player ) {
			if ( this.mapNames != null ) {
				player.triggerEvent ( "onMapMenuOpen", this.mapNames, this.mapDescriptions[player.GetChar().language] );
			}
		}

		private void AddVoteToMap ( Client player, string mapname ) {
			if ( this.playerVotes.ContainsKey ( player ) ) {
				string oldvote = this.playerVotes[player];
				if ( oldvote == mapname )
					return;
				this.mapVotes[oldvote]--;
				if ( this.mapVotes[oldvote] <= 0 ) {
					this.SendAllPlayerEvent ( "onMapRemoveFromVoting", -1, oldvote );
					this.SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname );
					this.mapVotes.Remove ( oldvote );
				} else {
					this.SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname, oldvote );
				}
			} else
				this.SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname );
			this.playerVotes[player] = mapname;
			this.mapVotes[mapname]++;
		}

		private void AddMapToVoting ( Client player, string mapname ) {
			if ( !this.mapVotes.ContainsKey ( mapname ) ) {
				if ( this.mapVotes.Count < 6 ) {
					// Anti-Cheat //
					if ( !this.mapNames.Contains ( mapname ) ) {
						Manager.Log.Error ( player.socialClubName + " voted for " + mapname + ", but it doesn't exist!", this.name );
						return;
					}
					///////////////
					this.mapVotes[mapname] = 0;
					this.SendAllPlayerEvent ( "onNewMapForVoting", -1, mapname );
				} else
					player.SendLangNotification ( "not_more_maps_for_voting_allowed" );
			}
			this.AddVoteToMap ( player, mapname );
		}

		private async Task<Map> GetNextMap ( ) {
			if ( this.mapVotes.Count > 0 ) {
				string wonmap = this.mapVotes.Aggregate ( ( l, r ) => l.Value > r.Value ? l : r ).Key;
				this.SendAllPlayerLangNotification ( "map_won_voting", -1, wonmap );
				this.mapVotes = new Dictionary<string, int> ();
				this.playerVotes = new Dictionary<Client, string> ();
				return await Manager.Map.GetMapClass ( wonmap, this ).ConfigureAwait ( false );
			} else
				return await this.GetRandomMap ().ConfigureAwait ( false );
		}

		private void SyncMapVotingOnJoin ( Client player ) {
			if ( this.mapVotes.Count > 0 ) {
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
