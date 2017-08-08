using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace Manager {
	class PlayerCommand : Script {

		[Command ( "leave", Alias = "leavelobby,lobbyleave", Description = "Leaves the lobby", AddToHelpmanager = true )]
		public void Leave ( Client player ) {
			Class.Lobby lobby = player.GetChar ().lobby;
			if ( lobby != MainMenu.lobby ) {
				lobby.RemovePlayer ( player );
			}
		}

		[Command ( "kill", Alias = "suicide", Description = "Commits suicide", AddToHelpmanager = true )]
		public void Kill ( Client player ) {
			Class.Character character = player.GetChar ();
			Class.Lobby lobby = character.lobby;
			if ( lobby.isPlayable ) {
				if ( character.lifes > 0 ) {
					lobby.KillPlayer ( player, "commited_suicide" );
				}
			}
		}

		[Command ( "globalchat", Alias = "globalsay,global", Description = "Writes in global-chat", AddToHelpmanager = true )]
		public void GlobalChat ( Client player, string text ) {
			Chat.SendGlobalMessage ( player, text );
		}

		[Command ( "pos", Alias = "getpos,rot,getrot", Description = "Gets your position and rotation", AddToHelpmanager = true )]
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

		[Command ( "checkmapname", Description = "Checks if a map-name is already taken (needed to now for new maps)", AddToHelpmanager = true )]
		public void CheckMapName ( Client player, string mapname ) {
			if ( Map.mapByName.ContainsKey ( mapname ) ) {
				API.sendNotificationToPlayer ( player, "map-name already taken" );
			} else {
				API.sendNotificationToPlayer ( player, "map-name is available" );
			}
		}

		[Command ( "hitsound", Alias = "hitglocke,togglehitsound", Description = "Activates/deactivates the hitsound", AddToHelpmanager = true )]
		public void ToggleHitsound ( Client player, int activate = -1 ) {
			Class.Character character = player.GetChar ();
			if ( activate == 1 || activate != 0 && !character.hitsoundOn ) {
				character.hitsoundOn = true;
				player.SendLangNotification ( "activated_hitsound" );
			} else {
				character.hitsoundOn = false;
				player.SendLangNotification ( "deactivated_hitsound" );
			}
		}
	}
}
