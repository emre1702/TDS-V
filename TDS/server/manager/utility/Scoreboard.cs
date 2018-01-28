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
        public string PlayTime = "-";
        public int Kills = -1;
        public int Assists = -1;
        public int Deaths = -1;
        public string TeamOrLobby;   
    }

    [Serializable]
    class ScoreboardOtherLobbyData {
        public string Name;
        public int Amount = 1;
    }

	class Scoreboard: Script {

        public Scoreboard ( ) { }

        public static string GetHoursOpticByMinutes ( uint minutes ) {
			uint hours = minutes / 60;
			minutes = minutes % 60;
			return hours + ":" + ( minutes < 10 ? "0" + minutes : minutes.ToString () );
		}

        [RemoteEvent ( "onClientRequestPlayerListDatas" )]
        public void OnClientRequestPlayerListDatas ( Client player, params object[] args ) {
            List<Client> players = NAPI.Pools.GetAllPlayers ();

            List<ScoreboardPlayerData> playersdata = new List<ScoreboardPlayerData> ();
            List<ScoreboardOtherLobbyData> otherlobbydata = new List<ScoreboardOtherLobbyData> ();
			Dictionary<int, int> doneLobbyIDs = new Dictionary<int, int> ();

			Character playerCharacter = player.GetChar ();
			int ownLobbyID = playerCharacter.Lobby.ID;

			foreach ( Client target in players ) {
				Character character = target.GetChar ();
				int lobbyID = character.Lobby.ID;
				if ( lobbyID == ownLobbyID /* he is same lobby */ || ownLobbyID == 0 /* you are in mainmenu */ ) {
                    ScoreboardPlayerData data = new ScoreboardPlayerData ();
                    playersdata.Add ( data );
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
                        data.TeamOrLobby = player.GetLang ( "connecting" );
					}
				} else {
					if ( doneLobbyIDs.ContainsKey ( lobbyID ) ) {
                        ++otherlobbydata[doneLobbyIDs[lobbyID]].Amount;
					} else {
                        ScoreboardOtherLobbyData lobbydata = new ScoreboardOtherLobbyData {
                            Name = character.Lobby.Name
                        };
                        otherlobbydata.Add ( lobbydata );
                        doneLobbyIDs[lobbyID] = otherlobbydata.Count - 1;
					}
				}
			}
			if ( ownLobbyID != 0 )
                NAPI.ClientEvent.TriggerClientEvent ( player, "giveRequestedPlayerListDatas", JsonConvert.SerializeObject ( playersdata ), JsonConvert.SerializeObject ( otherlobbydata ) );
			else
                NAPI.ClientEvent.TriggerClientEvent ( player, "giveRequestedPlayerListDatas", JsonConvert.SerializeObject ( playersdata ) );
		}
	}

}
