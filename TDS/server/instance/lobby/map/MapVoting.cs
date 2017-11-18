namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using extend;
	using GTANetworkAPI;
	using manager.logs;
	using manager.utility;
	using map;

	partial class Lobby {
		private readonly Dictionary<string, uint> mapVotes = new Dictionary<string, uint> ();
		private readonly Dictionary<Client, string> playerVotes = new Dictionary<Client, string> ();

		private void SendMapsForVoting ( Client player ) {
			if ( this.mapNames != null ) {
				player.TriggerEvent ( "onMapMenuOpen", this.mapNames, this.mapDescriptions[player.GetChar ().Language] );
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
					this.mapVotes.Remove ( oldvote, out uint _ );
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
						Log.Error ( player.SocialClubName + " voted for " + mapname + ", but it doesn't exist!", this.Name );
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

		private async Task<Map> GetNextMap () {
			if ( this.mapVotes.Count > 0 ) {
				string wonmap = this.mapVotes.Aggregate ( ( l, r ) => l.Value > r.Value ? l : r ).Key;
				this.SendAllPlayerLangNotification ( "map_won_voting", -1, wonmap );
				this.mapVotes.Clear ();
				this.playerVotes.Clear ();
				return await manager.map.Map.GetMapClass ( wonmap, this ).ConfigureAwait ( false );
			}
			return await this.GetRandomMap ().ConfigureAwait ( false );
		}

		private void SyncMapVotingOnJoin ( Client player ) {
			if ( this.mapVotes.Count > 0 ) {
				List<string> mapnames = new List<string> ();
				List<uint> mapvotes = new List<uint> ();

				foreach ( KeyValuePair<string, uint> pair in this.mapVotes.OrderByDescending ( p => p.Value ) ) {
					mapnames.Add ( pair.Key );
					mapvotes.Add ( pair.Value );
				}
				player.TriggerEvent ( "onMapVotingSyncOnJoin", mapnames, mapvotes );
			}
		}
	}

}
