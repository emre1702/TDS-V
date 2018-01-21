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
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientMapsListRequest", mapsJson );
			}
		}

		public void AddVoteToMap ( Client player, string mapname ) {
            if ( playerVotes.ContainsKey ( player ) ) {
				string oldvote = playerVotes[player];
                playerVotes.Remove ( player );

                if ( oldvote == mapname )
					return;
				if ( --mapVotes[oldvote] <= 0 ) {
					mapVotes.Remove ( oldvote, out uint _ );
				}
			    SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname, oldvote );
			} else
				SendAllPlayerEvent ( "onAddVoteToMap", -1, mapname );
            playerVotes[player] = mapname;
            mapVotes[mapname]++;
		}

        public void AddMapToVoting ( Client player, string mapname ) {
			if ( !mapVotes.ContainsKey ( mapname ) ) {
                if ( mapVotes.Count < 9 ) {
                    mapVotes[mapname] = 0;
                    AddVoteToMap ( player, mapname );
                } else
					player.SendLangNotification ( "not_more_maps_for_voting_allowed" );
			}
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
                NAPI.ClientEvent.TriggerClientEvent ( player, "onMapVotingSyncOnJoin", JsonConvert.SerializeObject ( mapVotes ) );
			}
		}
	}

}
