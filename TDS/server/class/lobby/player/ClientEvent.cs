using GrandTheftMultiplayer.Server.Elements;

namespace Class {

	partial class Lobby {

		private static void OnClientEventTrigger ( Client player, string eventName, params dynamic[] args ) {
			switch ( eventName ) {

				#region Lobby
				case "joinLobby":
					if ( lobbysbyindex.ContainsKey ( args[0] ) ) {
						Lobby lobby = lobbysbyindex[args[0]];
						lobby.AddPlayer ( player, args[1] );
					} else {
						/* player.sendNotification (  lobby doesn't exist ); */
						player.triggerEvent ( "onClientJoinMainMenu" );
					}
					break;
				#endregion

				#region Spectate
				case "spectateNext":
					Class.Character character = player.GetChar ();
					if ( character.lifes == 0 && ( character.lobby.status == "round" || character.team == 0 && character.lobby.status == "countdown" ) ) {
						if ( character.team == 0 )
							character.lobby.SpectateAllTeams ( player, args[0] );
						else
							character.lobby.SpectateTeammate ( player, args[0] );
					}
					break;
				#endregion

				#region Round
				case "onPlayerWasTooLongOutsideMap":
					Class.Character character2 = player.GetChar ();
					if ( character2.lobby.isPlayable ) {
						character2.lobby.KillPlayer ( player, "too_long_outside_map" );
					}
					break;
				#endregion

				#region MapVote
				case "onMapMenuOpen":
					player.GetChar ().lobby.SendMapsForVoting ( player );
					break;

				case "onMapVotingRequest":
					player.GetChar ().lobby.AddMapToVoting ( player, args[0] );
					break;

				case "onVoteForMap":
					player.GetChar ().lobby.AddVoteToMap ( player, args[0] );
					break;
				#endregion

				#region Bomb
				case "onPlayerStartPlanting":
					player.GetChar ().lobby.StartBombPlanting ( player );
					break;

				case "onPlayerStopPlanting":
					player.GetChar ().lobby.StopBombPlanting ( player );
					break;

				case "onPlayerStartDefusing":
					player.GetChar ().lobby.StartBombDefusing ( player );
					break;

				case "onPlayerStopDefusing":
					player.GetChar ().lobby.StopBombDefusing ( player );
					break;
				#endregion

				#region Freecam
				case "setFreecamObjectPositionTo":
					player.GetChar ().lobby.SetPlayerFreecamPos ( player, args[0] );
					break;
				#endregion
			}
		}
	}

}