namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using extend;
	using GTANetworkAPI;
	using manager.logs;
	using manager.utility;
	using map;

	partial class Arena {
		private readonly Dictionary<string, uint> mapVotes = new Dictionary<string, uint> ();
		private readonly Dictionary<Client, string> playerVotes = new Dictionary<Client, string> ();

        public void SendMapsForVoting ( Client player ) {
			if ( mapNames != null ) {
				player.TriggerEvent ( "onMapMenuOpen", mapNames, mapDescriptions[player.GetChar ().Language] );
			}
		}

		public void AddVoteToMap ( Client player, string mapname ) {
			if ( playerVotes.ContainsKey ( player ) ) {
				string oldvote = playerVotes[player];
				if ( oldvote == mapname )
					return;
				mapVotes[oldvote]--;
				if ( mapVotes[oldvote] <= 0 ) {
					SendAllPlayerEvent ( "onMapRemoveFromVoting", -1, oldvote );
					SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname );
					mapVotes.Remove ( oldvote, out uint _ );
				} else {
					SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname, oldvote );
				}
			} else
				SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname );
			playerVotes[player] = mapname;
			mapVotes[mapname]++;
		}

        public void AddMapToVoting ( Client player, string mapname ) {
			if ( !mapVotes.ContainsKey ( mapname ) ) {
				if ( mapVotes.Count < 6 ) {
					// Anti-Cheat //
					if ( !mapNames.Contains ( mapname ) ) {
						Log.Error ( player.SocialClubName + " voted for " + mapname + ", but it doesn't exist!", Name );
						return;
					}
					///////////////
					mapVotes[mapname] = 0;
					SendAllPlayerEvent ( "onNewMapForVoting", -1, mapname );
				} else
					player.SendLangNotification ( "not_more_maps_for_voting_allowed" );
			}
			AddVoteToMap ( player, mapname );
		}

		private async Task<Map> GetNextMap () {
			if ( mapVotes.Count > 0 ) {
				string wonmap = mapVotes.Aggregate ( ( l, r ) => l.Value > r.Value ? l : r ).Key;
				SendAllPlayerLangNotification ( "map_won_voting", -1, wonmap );
				mapVotes.Clear ();
				playerVotes.Clear ();
				return await manager.map.Map.GetMapClass ( wonmap, this ).ConfigureAwait ( false );
			}
			return await GetRandomMap ().ConfigureAwait ( false );
		}

		private void SyncMapVotingOnJoin ( Client player ) {
			if ( mapVotes.Count > 0 ) {
				List<string> mapnames = new List<string> ();
				List<uint> mapvotes = new List<uint> ();

				foreach ( KeyValuePair<string, uint> pair in mapVotes.OrderByDescending ( p => p.Value ) ) {
					mapnames.Add ( pair.Key );
					mapvotes.Add ( pair.Value );
				}
				player.TriggerEvent ( "onMapVotingSyncOnJoin", mapnames, mapvotes );
			}
		}
	}

}
