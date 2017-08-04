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
		public static readonly Random rnd = new Random ();
		private static DateTime startDateTime = new DateTime ( 2017, 7, 24 );

		public static string ConvertToSHA512 ( string input ) {
			byte[] hashbytes = SHA512Managed.Create ().ComputeHash ( Encoding.Default.GetBytes ( input ) );
			StringBuilder sb = new StringBuilder ();
			for ( int i = 0; hashbytes != null && i < hashbytes.Length; i++ ) {
				sb.AppendFormat ( "{0:x2}", hashbytes[i] );
			}
			return sb.ToString ();
		}

		public static int GetTimespan ( int seconds = 0 ) {
			TimeSpan t = DateTime.Now.AddSeconds ( seconds ) - startDateTime;
			return (int) t.TotalSeconds;
		}

		public static string GetTimestamp ( int seconds = 0 ) {
			return DateTime.Now.AddSeconds ( seconds ).ToString ( "de-DE" );
		}

		[Command ( "xyz" )]
		public void Gotoxyz ( Client player, float x, float y, float z ) {
			API.shared.setEntityPosition ( player, new Vector3 ( x, y, z ) );
		}

		[Command ( "cveh" )]
		public void SpawnCarCommand ( Client sender, string name ) {
			VehicleHash model = API.shared.vehicleNameToModel ( name );

			Vector3 rot = API.shared.getEntityRotation ( sender.handle );
			Vehicle veh = API.shared.createVehicle ( model, sender.position, new Vector3 ( 0, 0, rot.Z ), 0, 0 );

			API.shared.setPlayerIntoVehicle ( sender, veh, -1 );
		}

		[Command ( "pos" )]
		public void SendPlayerPosition ( Client sender ) {
			if ( API.shared.isPlayerInAnyVehicle ( sender ) ) {
				NetHandle veh = API.shared.getPlayerVehicle ( sender );
				Vector3 pos = API.shared.getEntityPosition ( veh );
				API.shared.sendChatMessageToPlayer ( sender, "Vehicle X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z );
				Vector3 rot = API.shared.getEntityRotation ( veh );
				API.shared.sendChatMessageToPlayer ( sender, "Vehicle ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z );
			} else {
				Vector3 pos = API.shared.getEntityPosition ( sender );
				API.shared.sendChatMessageToPlayer ( sender, "Player X: " + pos.X + " Y: " + pos.Y + " Z: " + pos.Z );
				Vector3 rot = API.shared.getEntityRotation ( sender );
				API.shared.sendChatMessageToPlayer ( sender, "Player ROT RX: " + rot.X + " RY: " + rot.Y + " RZ: " + rot.Z );
			}
		}

		[Command ( "goto" )]
		public void GotoPlayer ( Client player, string name ) {
			Client target = API.shared.getPlayerFromName ( name );
			if ( target != null ) {
				Vector3 playerpos = API.shared.getEntityPosition ( target );
				if ( player.isInVehicle ) {
					API.shared.setEntityPosition ( player.vehicle, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else if ( target.isInVehicle ) {
					Client[] usersInCar = target.vehicle.occupants;
					if ( usersInCar.Length < API.shared.getVehicleMaxOccupants ( (VehicleHash) ( target.vehicle.model ) ) ) {
						Dictionary<int, bool> occupiedseats = new Dictionary<int, bool> ();
						foreach ( Client occupant in usersInCar ) {
							occupiedseats[occupant.vehicleSeat] = true;
						}
						for ( int i = 0; i < API.shared.getVehicleMaxOccupants ( (VehicleHash) ( target.vehicle.model ) ); i++ ) {
							if ( !occupiedseats.ContainsKey ( i ) ) {
								API.shared.setPlayerIntoVehicle ( player, target.vehicle, i );
								return;
							}
						}
					}
					API.shared.setEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y + 1, playerpos.Z + 1 ) );
				} else {
					API.shared.setEntityPosition ( player, new Vector3 ( playerpos.X + 1, playerpos.Y, playerpos.Z ) );

				}

			} else
				API.shared.sendChatMessageToPlayer ( player, "Der Spieler existiert nicht!" );
		}

		[Command ( "checkmapname" )]
		public void CheckMapName ( Client player, string mapname ) {
			if ( Map.mapByName.ContainsKey ( mapname ) ) {
				API.sendNotificationToPlayer ( player, "map-name already taken" );
			} else {
				API.sendNotificationToPlayer ( player, "map-name is available" );
			}
		}
	}
}
