namespace TDS.server.instance.lobby {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;
	using player;
	using extend;

	partial class Lobby {

		private void SendPlayerAmountInFightInfo ( Client player ) {
			List<uint> amountinteams = new List<uint> ();
			List<uint> amountaliveinteams = new List<uint> ();
			for ( int i = 0; i < this.Players.Count; i++ ) {
				amountinteams.Add ( (uint) this.Players[i].Count );
				amountaliveinteams.Add ( (uint) this.alivePlayers[i].Count );
			}
			player.PlayerAmountInFightSync ( amountinteams, amountaliveinteams );
		}

		private void RewardAllPlayer () {
			foreach ( KeyValuePair<Client, int> entry in this.DmgSys.PlayerDamage ) {
				if ( entry.Key.Exists ) {
					Client player = entry.Key;
					Character character = player.GetChar ();
					if ( character.Lobby == this ) {
						List<uint> reward = new List<uint> ();
						if ( this.DmgSys.PlayerKills.ContainsKey ( player ) ) {
							reward.Add ( (uint) ( Money.MoneyForDict["kill"] * this.DmgSys.PlayerKills[player] ) );
						} else
							reward.Add ( 0 );
						if ( this.DmgSys.PlayerAssists.ContainsKey ( player ) ) {
							reward.Add ( (uint) ( Money.MoneyForDict["assist"] * this.DmgSys.PlayerAssists[player] ) );
						} else
							reward.Add ( 0 );
						reward.Add ( (uint) ( Money.MoneyForDict["damage"] * entry.Value ) );

						uint total = reward[0] + reward[1] + reward[2];
						player.GiveMoney ( total, character );
						player.SendLangNotification ( "round_reward", reward[0].ToString (), reward[1].ToString (), reward[2].ToString (),
													total.ToString () );
					}
				}
			}
		}

		private void SetAllPlayersInCountdown () {
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				this.SetPlayerReadyForRound ( player, (uint) teamID );
				player.TriggerEvent ( "onClientCountdownStart", this.currentMap.Name );
				if ( teamID == 0 )
					this.SpectateAllTeams ( player, true );
			} );
			if ( this.currentMap.Type == "bomb" )
				this.GiveBombToRandomTerrorist ();
		}

		internal void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
			if ( teamindex == -1 ) {
				this.FuncIterateAllPlayers ( ( player, teamID ) => { player.TriggerEvent ( eventName, args ); } );
			} else
				this.FuncIterateAllPlayers ( ( player, teamID ) => { player.TriggerEvent ( eventName, args ); }, teamindex );
		}

		internal void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
			Dictionary<string, string> texts = Language.GetLangDictionary ( langstr, args );
			this.FuncIterateAllPlayers (
				( player, teamID ) => { API.SendNotificationToPlayer ( player, texts[player.GetChar ().Language] ); }, teamindex );
		}

		internal void SendAllPlayerLangMessage ( string langstr, int teamindex = -1, params string[] args ) {
			Dictionary<string, string> texts = Language.GetLangDictionary ( langstr, args );
			this.FuncIterateAllPlayers ( ( player, teamID ) => { player.SendChatMessage ( texts[player.GetChar ().Language] ); },
										teamindex );
		}

		internal void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
			this.FuncIterateAllPlayers ( ( player, teamID ) => { player.SendChatMessage ( message ); }, teamindex );
		}

		internal void FuncIterateAllPlayers ( Action<Client, int> func, int teamID = -1 ) {
			if ( teamID == -1 ) {
				for ( int i = 0; i < this.Players.Count; i++ )
					for ( int j = this.Players[i].Count - 1; j >= 0; j-- )
						if ( this.Players[i][j].Exists ) {
							if ( this.Players[i][j].GetChar ().Lobby == this ) {
								func ( this.Players[i][j], i );
							} else
								this.Players[i].RemoveAt ( j );
						} else
							this.Players[i].RemoveAt ( j );
			} else
				for ( int j = this.Players[teamID].Count - 1; j >= 0; j-- )
					if ( this.Players[teamID][j].Exists ) {
						if ( this.Players[teamID][j].GetChar ().Lobby == this ) {
							func ( this.Players[teamID][j], teamID );
						} else
							this.Players[teamID].RemoveAt ( j );
					} else
						this.Players[teamID].RemoveAt ( j );
		}
	}

}
