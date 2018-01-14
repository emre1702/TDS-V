namespace TDS.server.manager.utility {
    using System;
    using System.Collections.Generic;
	using extend;
	using GTANetworkAPI;
	using instance.player;
    using Newtonsoft.Json;

    [Serializable]
    class ScoreboardPlayerData {
        public string Name;
        public string PlayTime;
        public int Kills;
        public int Assists;
        public int Deaths;
        public string TeamOrLobby;   
    }

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
                List<Client> players = NAPI.Pools.GetAllPlayers ();

                List<ScoreboardPlayerData> playersdata = new List<ScoreboardPlayerData> ();

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
                            ScoreboardPlayerData data = new ScoreboardPlayerData ();
                            data.Name = target.Name;

							if ( character.LoggedIn ) {
								// character stats //
								data.PlayTime = GetHoursOpticByMinutes ( character.Playtime );
								data.Kills = (int) character.Kills;
								data.Assists = (int) character.Assists;
								data.Deaths = (int) character.Deaths;
								data.TeamOrLobby = ownLobbyID == 0 ? character.Lobby.Name : character.Lobby.GetTeamName ( character.Team );
							} else {
                                // default status //
                                data.PlayTime = "-";
                                data.Kills = -1;
                                data.Assists = -1;
                                data.Deaths = -1;
                                data.TeamOrLobby = player.GetLang ( "connecting" );
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
                    NAPI.ClientEvent.TriggerClientEvent ( player, "giveRequestedPlayerListDatas", JsonConvert.SerializeObject ( playersdata ), otherLobbyNames, otherLobbyAmounts );
				else
                    NAPI.ClientEvent.TriggerClientEvent ( player, "giveRequestedPlayerListDatasMainmenu", JsonConvert.SerializeObject ( playersdata ) );
			}
		}
	}

}
