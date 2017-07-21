using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Shared;


namespace Manager {
	class Utility : Script {

		public Utility ( ) {
			API.setGamemodeName ( "TDS" );
		}

		public static string ConvertToSHA512 ( string input ) {
			byte[] hashbytes = SHA512Managed.Create ().ComputeHash ( Encoding.Default.GetBytes ( input ) );
			StringBuilder sb = new StringBuilder ();
			for ( int i = 0; hashbytes != null && i < hashbytes.Length; i++ ) {
				sb.AppendFormat ( "{0:x2}", hashbytes[i] );
			}
			return sb.ToString ();
		}

		public static int GetTimespan ( int seconds = 0 ) {
			TimeSpan t = DateTime.Now.AddSeconds ( seconds ) - new DateTime ( 2017, 6, 24 );
			return (int) t.TotalSeconds;
		}

		public static string GetTimestamp ( int seconds = 0 ) {
			return DateTime.Now.AddSeconds ( seconds ).ToString ( "de-DE" );
		}

		[Command ( "xyz" )]
		public void Gotoxyz ( Client player, float x, float y, float z ) {
			API.setEntityPosition ( player, new Vector3 ( x, y, z ) );
		}

		[Command ( "cveh" )]
		public void SpawnCarCommand ( Client sender, string name ) {
			VehicleHash model = API.vehicleNameToModel ( name );

			Vector3 rot = API.getEntityRotation ( sender.handle );
			Vehicle veh = API.createVehicle ( model, sender.position, new Vector3 ( 0, 0, rot.Z ), 0, 0 );

			API.setPlayerIntoVehicle ( sender, veh, -1 );
		}

		[Command ( "pos" )]
		public void SendPlayerPosition ( Client sender ) {
			if ( API.isPlayerInAnyVehicle ( sender ) ) {
				NetHandle veh = API.getPlayerVehicle ( sender );
				Vector3 pos = API.getEntityPosition ( veh );
				API.sendChatMessageToPlayer ( sender, "Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z );
				Vector3 rot = API.getEntityRotation ( veh );
				API.sendChatMessageToPlayer ( sender, "Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z );
			} else {
				Vector3 pos = API.getEntityPosition ( sender );
				API.sendChatMessageToPlayer ( sender, "Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z );
				Vector3 rot = API.getEntityRotation ( sender );
				API.sendChatMessageToPlayer ( sender, "Player ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z );
			}
		}

		[Command ( "goto" )]
		public void GotoPlayer ( Client player, string name ) {
			Client target = API.getPlayerFromName ( name );
			if ( target != null ) {
				Vector3 playerpos = API.getEntityPosition ( target );
				if ( player.isInVehicle ) {
					API.setEntityPosition ( player.vehicle, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else if ( target.isInVehicle ) {
					Client[] usersInCar = target.vehicle.occupants;
					if ( usersInCar.Length < API.getVehicleMaxOccupants ( (VehicleHash) ( target.vehicle.model ) ) ) {
						Dictionary<int, bool> occupiedseats = new Dictionary<int, bool> ();
						foreach ( Client occupant in usersInCar ) {
							occupiedseats[occupant.vehicleSeat] = true;
						}
						for ( int i = 0; i < API.getVehicleMaxOccupants ( (VehicleHash) ( target.vehicle.model ) ); i++ ) {
							if ( !occupiedseats.ContainsKey ( i ) ) {
								API.setPlayerIntoVehicle ( player, target.vehicle, i );
								return;
							}
						}
					}
					API.setEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else {
					API.setEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y, playerpos.Z ) );

				}

			} else
				API.sendChatMessageToPlayer ( player, "Der Spieler existiert nicht!" );
		}
	}
}
