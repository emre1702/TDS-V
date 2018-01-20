namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using extend;
	using GTANetworkAPI;
	using manager.logs;
	using manager.utility;
	using map;
    using Newtonsoft.Json;

    partial class Arena {
		private readonly Dictionary<string, uint> mapVotes = new Dictionary<string, uint> ();
		private readonly Dictionary<NetHandle, string> playerVotes = new Dictionary<NetHandle, string> ();

        public void SendMapsForVoting ( Client player ) {
			if ( mapsJson != null ) {
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientMapMenuOpen", mapsJson );
			}
		}

		public void AddVoteToMap ( Client player, string mapname ) {
			if ( playerVotes.ContainsKey ( player ) ) {
				string oldvote = playerVotes[player];
				if ( oldvote == mapname )
					return;
				if ( --mapVotes[oldvote] <= 0 ) {
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
				if ( mapVotes.Count < 9 ) {
					mapVotes[mapname] = 0;
					SendAllPlayerEvent ( "onNewMapForVoting", -1, mapname );
				} else
					player.SendLangNotification ( "not_more_maps_for_voting_allowed" );
			}
			AddVoteToMap ( player, mapname );
		}

		private Map GetNextMap () {
			if ( mapVotes.Count > 0 ) {
				string wonmap = mapVotes.Aggregate ( ( l, r ) => l.Value > r.Value ? l : r ).Key;
				SendAllPlayerLangNotification ( "map_won_voting", -1, wonmap );
				mapVotes.Clear ();
				playerVotes.Clear ();
                for ( int i = 0; i < maps.Count; ++i )
                    if ( maps[i].SyncData.Name == wonmap )
                        return maps[i];
			}
            return GetRandomMap ();
		}

		private void SyncMapVotingOnJoin ( Client player ) {
			if ( mapVotes.Count > 0 ) {
				List<string> mapnames = new List<string> ();
				List<uint> mapvotes = new List<uint> ();

				foreach ( KeyValuePair<string, uint> pair in mapVotes.OrderByDescending ( p => p.Value ) ) {
					mapnames.Add ( pair.Key );
					mapvotes.Add ( pair.Value );
				}
                NAPI.ClientEvent.TriggerClientEvent ( player, "onMapVotingSyncOnJoin", mapnames, mapvotes );
			}
		}
	}

}
