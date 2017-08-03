using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;

namespace Manager {
	class Scoreboard {

		public static void ScoreboardOnStart ( API api ) {
			api.onClientEventTrigger += OnClientRequestPlayerListDatas;
		}

		public static string GetHoursOpticByMinutes ( int minutes ) {
			int hours = minutes / 60;
			minutes = minutes % 60;
			return hours.ToString() + ":" + ( minutes < 10 ? "0" + minutes : minutes.ToString() );
		}

		private static void OnClientRequestPlayerListDatas ( Client player, string eventName, params object[] args ) {
			if ( eventName == "onClientRequestPlayerListDatas" ) {
				List<Client> players = API.shared.getAllPlayers ();

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
					Class.Character character = players[i].GetChar ();
					int lobbyID = character.lobby.id;
					if ( lobbyID == ownLobbyID /* he is same lobby */ || ownLobbyID == 0 /* you are Mainmenu */) {
						nameList.Add ( players[i].name );
						if ( character.loggedIn ) {
							playtimeList.Add ( GetHoursOpticByMinutes ( character.playtime ) );
							killsList.Add ( character.kills.ToString () );
							assistsList.Add ( character.assists.ToString () );
							deathsList.Add ( character.deaths.ToString () );
							if ( ownLobbyID == 0 )
								teamorlobbyList.Add ( character.lobby.name );
							else
								teamorlobbyList.Add ( character.lobby.GetTeamName ( character.team ) );
						} else {
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

				if ( ownLobbyID != 0 ) 
					API.shared.triggerClientEvent ( player, "giveRequestedPlayerListDatas", nameList, playtimeList, killsList, assistsList, deathsList, teamorlobbyList, otherLobbyNames, otherLobbyAmounts );
				else
					API.shared.triggerClientEvent ( player, "giveRequestedPlayerListDatasMainmenu", nameList, playtimeList, killsList, assistsList, deathsList, teamorlobbyList );
			}
		}
	}
}