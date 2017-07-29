using System.Collections.Generic;
using System.Linq;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby : Script {
		private Dictionary<string, int> mapVotes = new Dictionary<string, int>();

		private void SendMapsForVoting ( Client player ) {
			if ( this.mapNames != null ) {
				player.triggerEvent ( "onMapMenuOpen", this.mapNames );
			}
		}

		private void AddMapToVoting ( Client player, string mapname ) {

		}

		private Map GetNextMap ( ) {
			if ( mapVotes.Count > 0 ) {
				string wonmap = mapVotes.Aggregate ( ( l, r ) => l.Value > r.Value ? l : r ).Key;
				return Manager.Map.GetMapClass ( wonmap, this );
			} else
				return this.GetRandomMap ();
		}
	}
}
