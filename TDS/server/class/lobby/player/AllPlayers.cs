using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Manager;

namespace Class {

	partial class Lobby {

		private void SendPlayerAmountInFightInfo ( Client player ) {
			List<int> amountinteams = new List<int> ();
			List<int> amountaliveinteams = new List<int> ();
			for ( int i = 0; i < this.players.Count; i++ ) {
				amountinteams.Add ( this.players[i].Count );
				amountaliveinteams.Add ( this.alivePlayers[i].Count );
			}
			player.PlayerAmountInFightSync ( amountinteams, amountaliveinteams );
		}

		private void RewardAllPlayer ( ) {
			foreach ( KeyValuePair<Client, int> entry in this.damageSys.playerDamage ) {
				if ( entry.Key.exists ) {
					Client player = entry.Key;
					Character character = player.GetChar ();
					if ( character.lobby == this ) {
						List<int> reward = new List<int> ();
						if ( this.damageSys.playerKills.ContainsKey ( player ) ) {
							reward.Add ( (int) ( Manager.Money.moneyForDict["kill"] * (double) this.damageSys.playerKills[player] ) );
						} else
							reward.Add ( 0 );
						if ( this.damageSys.playerAssists.ContainsKey ( player ) ) {
							reward.Add ( (int) ( Manager.Money.moneyForDict["assist"] * (double) this.damageSys.playerAssists[player] ) );
						} else
							reward.Add ( 0 );
						reward.Add ( (int) ( Manager.Money.moneyForDict["damage"] * (double) entry.Value ) );

						int total = reward[0] + reward[1] + reward[2];
						player.GiveMoney ( total, character );
						player.SendLangNotification ( "round_reward", reward[0].ToString (), reward[1].ToString (), reward[2].ToString (), total.ToString () );
					}
				}
			}
		}

		private void SetAllPlayersInCountdown ( ) {
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				this.SetPlayerReadyForRound ( player, teamID );
				player.triggerEvent ( "onClientCountdownStart", this.currentMap.name );
				if ( teamID == 0 )
					this.SpectateAllTeams ( player, true );
			} );
			if ( this.currentMap.type == "bomb" )
				this.GiveBombToRandomTerrorist ();
		}

		internal void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
			if ( teamindex == -1 ) {
				this.FuncIterateAllPlayers ( ( player, teamID ) => {
					player.triggerEvent ( eventName, args );
				} );
			} else
				this.FuncIterateAllPlayers ( ( player, teamID ) => {
					player.triggerEvent ( eventName, args );
				}, teamindex );
		}

		internal void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
			Dictionary<string, string> texts = Language.GetLangDictionary ( langstr, args );
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				API.sendNotificationToPlayer ( player, texts[player.GetChar().language] );
			}, teamindex );
		}

		internal void SendAllPlayerLangMessage ( string langstr, int teamindex = -1, params string[] args ) {
			Dictionary<string, string> texts = Language.GetLangDictionary ( langstr, args );
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				player.sendChatMessage ( texts[player.GetChar ().language] );
			}, teamindex );
		}

		internal void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				player.sendChatMessage ( message );
			}, teamindex );
		}

		internal void FuncIterateAllPlayers ( Action<Client, int> func, int teamID = -1 ) {
			if ( teamID == -1 ) {
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = this.players[i].Count - 1; j >= 0; j-- )
						if ( this.players[i][j].exists ) {
							if ( this.players[i][j].GetChar ().lobby == this ) {
								func ( this.players[i][j], i );
							} else
								this.players[i].RemoveAt ( j );
						} else
							this.players[i].RemoveAt ( j );

			} else
				for ( int j = this.players[teamID].Count - 1; j >= 0; j-- )
					if ( this.players[teamID][j].exists ) {
						if ( this.players[teamID][j].GetChar ().lobby == this ) {
							func ( this.players[teamID][j], teamID );
						} else
							this.players[teamID].RemoveAt ( j );
					} else
						this.players[teamID].RemoveAt ( j );

		}
	}
}