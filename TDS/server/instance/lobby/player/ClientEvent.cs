namespace TDS.server.instance.lobby {

	using GTANetworkAPI;
	using player;
	using extend;

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
						player.TriggerEvent ( "onClientJoinMainMenu" );
					}
					break;
				#endregion

				#region Spectate
				case "spectateNext":
					Character character = player.GetChar ();
					if ( character.Lifes == 0 &&
						( character.Lobby.status == "round" || character.Team == 0 && character.Lobby.status == "countdown" ) ) {
						if ( character.Team == 0 )
							character.Lobby.SpectateAllTeams ( player, args[0] );
						else
							character.Lobby.SpectateTeammate ( player, args[0] );
					}
					break;
				#endregion

				#region Round
				case "onPlayerWasTooLongOutsideMap":
					Character character2 = player.GetChar ();
					if ( character2.Lobby.IsPlayable ) {
						character2.Lobby.KillPlayer ( player, "too_long_outside_map" );
					}
					break;
				#endregion

				#region MapVote
				case "onMapMenuOpen":
					player.GetChar ().Lobby.SendMapsForVoting ( player );
					break;

				case "onMapVotingRequest":
					player.GetChar ().Lobby.AddMapToVoting ( player, args[0] );
					break;

				case "onVoteForMap":
					player.GetChar ().Lobby.AddVoteToMap ( player, args[0] );
					break;
				#endregion

				#region Bomb
				case "onPlayerStartPlanting":
					player.GetChar ().Lobby.StartBombPlanting ( player );
					break;

				case "onPlayerStopPlanting":
					player.GetChar ().Lobby.StopBombPlanting ( player );
					break;

				case "onPlayerStartDefusing":
					player.GetChar ().Lobby.StartBombDefusing ( player );
					break;

				case "onPlayerStopDefusing":
					player.GetChar ().Lobby.StopBombDefusing ( player );
					break;
				#endregion

				#region Freecam
				case "setFreecamObjectPositionTo":
					player.GetChar ().Lobby.SetPlayerFreecamPos ( player, args[0] );
					break;
				#endregion
			}
		}
	}

}
