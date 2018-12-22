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
        public uint Kills = 0;
        public uint Assists = 0;
        public uint Deaths = 0;
        public string TeamOrLobby;

		public static string GetHoursOpticByMinutes ( uint minutes ) {
			uint hours = minutes / 60;
			minutes = minutes % 60;
			return hours + ":" + ( minutes < 10 ? "0" + minutes : minutes.ToString () );
		}

		public void AddData ( Character character, int ownLobbyID ) {
			PlayTime = GetHoursOpticByMinutes ( character.Playtime );
			Kills = character.CurrentStats.Kills;
			Assists = character.CurrentStats.Assists;
			Deaths = character.CurrentStats.Deaths;
			TeamOrLobby = ownLobbyID == 0 ? character.Lobby.Name : character.Lobby.GetTeamName ( character.Team );
		}
    }

    [Serializable]
    class ScoreboardOtherLobbyData {
        public string Name;
        public int Amount = 1;
    }

	class Scoreboard: Script {

        public Scoreboard ( ) { }

        [RemoteEvent ( "onClientRequestPlayerListDatas" )]
        public void OnClientRequestPlayerListDatas ( Client player ) {
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
						data.AddData ( character, ownLobbyID );
					} else {
						// default status //
						data.TeamOrLobby = player.GetLang ( "connecting" );
					}
				} else {   // is in another lobby and you aren't in mainmenu
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
