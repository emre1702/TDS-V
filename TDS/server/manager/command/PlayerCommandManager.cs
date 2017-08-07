using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;

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
	}
}
