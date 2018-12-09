namespace TDS.Instance.Lobby {
    using GTANetworkAPI;
    using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
    using TDS.Manager.Utility;

    partial class Arena {

        private readonly Dictionary<string, uint> mapVotes = new Dictionary<string, uint>();
        private readonly Dictionary<Client, string> playerVotes = new Dictionary<Client, string>();

        private Map GetVotedMap()
        {
            if (mapVotes.Count > 0)
            {
                string wonmap = mapVotes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                SendAllPlayerLangNotification(lang =>
                {
                    return Utils.GetReplaced(lang.MAP_WON_VOTING, wonmap);
                });  
                mapVotes.Clear();
                playerVotes.Clear();
                /*for (int i = 0; i < maps.Count; ++i)
                    if (maps[i].SyncData.Name == wonmap)
                        return maps[i];*/
            }
            return null;
        }

        /*
		

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

		private void SyncMapVotingOnJoin ( Client player ) {
			if ( mapVotes.Count > 0 ) {
                NAPI.ClientEvent.TriggerClientEvent ( player, "onMapVotingSyncOnJoin", JsonConvert.SerializeObject ( mapVotes ) );
			}
		}*/
    }

}
