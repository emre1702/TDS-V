namespace TDS.server.manager.utility {

	using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using instance.player;

	class Scoreboard : Script {

		public Scoreboard () {
			Event.OnClientEventTrigger += OnClientRequestPlayerListDatas;
		}

		public static string GetHoursOpticByMinutes ( uint minutes ) {
			uint hours = minutes / 60;
			minutes = minutes % 60;
			return hours + ":" + ( minutes < 10 ? "0" + minutes : minutes.ToString () );
		}

		private void OnClientRequestPlayerListDatas ( Client player, string eventName, params object[] args ) {
			if ( eventName == "onClientRequestPlayerListDatas" ) {
				List<Client> players = API.GetAllPlayers ();

				List<string> nameList = new List<string> ();
				List<string> playtimeList = new List<string> ();
				List<string> killsList = new List<string> ();
				List<string> assistsList = new List<string> ();
				List<string> deathsList = new List<string> ();
				List<string> teamorlobbyList = new List<string> (); // lobby in mainmenu, team rest
				List<string> otherLobbyNames = new List<string> ();
				List<int> otherLobbyAmounts = new List<int> ();

				Dictionary<int, int> doneLobbyIDs = new Dictionary<int, int> ();
				Character playerCharacter = player.GetChar ();
				int ownLobbyID = playerCharacter.Lobby.ID;

				foreach ( Client target in players ) {
					if ( target.Exists ) {
						Character character = target.GetChar ();
						int lobbyID = character.Lobby.ID;
						if ( lobbyID == ownLobbyID /* he is same lobby */ || ownLobbyID == 0 /* you are in mainmenu */ ) {
							nameList.Add ( target.Name );

							if ( character.LoggedIn ) {
								// character stats //
								playtimeList.Add ( GetHoursOpticByMinutes ( character.Playtime ) );
								killsList.Add ( character.Kills.ToString () );
								assistsList.Add ( character.Assists.ToString () );
								deathsList.Add ( character.Deaths.ToString () );
								teamorlobbyList.Add ( ownLobbyID == 0 ? character.Lobby.Name : character.Lobby.GetTeamName ( character.Team ) );
							} else {
								// default status //
								playtimeList.Add ( "-" );
								killsList.Add ( "-" );
								assistsList.Add ( "-" );
								deathsList.Add ( "-" );
								teamorlobbyList.Add ( player.GetLang ( "connecting" ) );
							}
						} else {
							if ( doneLobbyIDs.ContainsKey ( lobbyID ) ) {
								otherLobbyAmounts[doneLobbyIDs[lobbyID]]++;
							} else {
								otherLobbyNames.Add ( character.Lobby.Name );
								otherLobbyAmounts.Add ( 1 );
								doneLobbyIDs[lobbyID] = otherLobbyNames.Count - 1;
							}
						}
					}
				}

				if ( ownLobbyID != 0 )
					API.TriggerClientEvent ( player, "giveRequestedPlayerListDatas", nameList, playtimeList, killsList, assistsList, deathsList, teamorlobbyList, otherLobbyNames, otherLobbyAmounts );
				else
					API.TriggerClientEvent ( player, "giveRequestedPlayerListDatasMainmenu", nameList, playtimeList, killsList, assistsList, deathsList, teamorlobbyList );
			}
		}
	}

}
