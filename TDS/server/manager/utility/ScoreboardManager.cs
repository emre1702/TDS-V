using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;

namespace Manager {
	class Scoreboard : Script {

		public Scoreboard () {
			API.onClientEventTrigger += this.OnClientRequestPlayerListDatas;
		}

		public static string GetHoursOpticByMinutes ( int minutes ) {
			int hours = minutes / 60;
			minutes = minutes % 60;
			return hours.ToString() + ":" + ( minutes < 10 ? "0" + minutes : minutes.ToString() );
		}

		private void OnClientRequestPlayerListDatas ( Client player, string eventName, params object[] args ) {
			if ( eventName == "onClientRequestPlayerListDatas" ) {
				List<Client> players = API.getAllPlayers ();

				List<string> nameList = new List<string> ();
				List<string> playtimeList = new List<string> ();
				List<string> killsList = new List<string> ();
				List<string> assistsList = new List<string> ();
				List<string> deathsList = new List<string> ();
				List<string> teamorlobbyList = new List<string> ();		// lobby in mainmenu, team rest
				List<string> otherLobbyNames = new List<string> ();
				List<int> otherLobbyAmounts = new List<int> ();

				Dictionary<int, int> doneLobbyIDs = new Dictionary<int, int> ();
				Class.Character playerCharacter = player.GetChar ();
				int ownLobbyID = playerCharacter.lobby.id;

				for ( int i=0; i < players.Count; i++ ) {
					if ( players[i].exists ) {
						Class.Character character = players[i].GetChar ();
						int lobbyID = character.lobby.id;
						if ( lobbyID == ownLobbyID /* he is same lobby */ || ownLobbyID == 0 /* you are in mainmenu */) {
							nameList.Add ( players[i].name );

							if ( character.loggedIn ) {
								// character stats //
								playtimeList.Add ( GetHoursOpticByMinutes ( character.playtime ) );
								killsList.Add ( character.kills.ToString () );
								assistsList.Add ( character.assists.ToString () );
								deathsList.Add ( character.deaths.ToString () );
								if ( ownLobbyID == 0 )
									teamorlobbyList.Add ( character.lobby.name );
								else
									teamorlobbyList.Add ( character.lobby.GetTeamName ( character.team ) );
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
								otherLobbyNames.Add ( character.lobby.name );
								otherLobbyAmounts.Add ( 1 );
								doneLobbyIDs[lobbyID] = otherLobbyNames.Count - 1;
							}

						}
					}
				}

				if ( ownLobbyID != 0 ) 
					API.triggerClientEvent ( player, "giveRequestedPlayerListDatas", nameList, playtimeList, killsList, assistsList, deathsList, teamorlobbyList, otherLobbyNames, otherLobbyAmounts );
				else
					API.triggerClientEvent ( player, "giveRequestedPlayerListDatasMainmenu", nameList, playtimeList, killsList, assistsList, deathsList, teamorlobbyList );
			}
		}
	}
}