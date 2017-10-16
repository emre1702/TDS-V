using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {
	partial class Lobby {

		private static Dictionary<Client, NetHandle> playerCamObject = new Dictionary<Client, NetHandle> ();
		private static Dictionary<Client, bool> freecamControlsDisabled = new Dictionary<Client, bool> ();

		private void StartPlayerFreecam ( Client player ) {
			if ( playerCamObject.ContainsKey ( player ) )
				this.StopPlayerFreecam ( player, false );
			NetHandle obj = API.createObject ( -1358020705, API.getEntityPosition ( player ), new Vector3 ( 0.0, 0.0, 0.0 ) ); //We create the object from server side, so later if we want to sync the object's position we can send the pos back here.
			playerCamObject[player] = obj;

			API.triggerClientEvent ( player, "startFreecam", obj );

			API.freezePlayer ( player, true );
			API.setEntityPosition ( player, new Vector3 ( 0.0, 0.0, 200.0 ) );
			API.setEntityTransparency ( player, 0 );
		}

		private void StopPlayerFreecam ( Client player, bool finaly = false ) {
			if ( playerCamObject.ContainsKey ( player ) ) {
				API.deleteEntity ( playerCamObject[player] );
				playerCamObject.Remove ( player );
			}
			if ( finaly )
				API.triggerClientEvent ( player, "stopFreecam" );
		}

		private void SetPlayerFreecamPos ( Client player, Vector3 to ) {
			if ( playerCamObject.ContainsKey ( player ) )
				API.setEntityPosition ( playerCamObject[player], to );
		}
	}

}