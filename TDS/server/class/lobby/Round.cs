using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using Manager;

namespace Class {
	partial class Lobby {

		public bool gotRounds = true;
		private int countdownTime = 5 * 1000;
		private int roundTime = 4 * 60 * 1000;
		public int roundEndTime = 8 * 1000;
		private string status = "loading";
		private int startTick = 0;

		public void Start ( ) {
			if ( this.isPlayable ) {
				if ( this.gotRounds )
					this.StartMapChoose ();
			}
		}

		public async void StartMapChoose ( ) {
			this.status = "mapchoose";
			API.shared.consoleOutput ( this.status );

			API.shared.sendNativeToPlayersInDimension ( this.dimension, Hash.DO_SCREEN_FADE_IN, this.roundEndTime / 2 );

			await Task.Run ( async ( ) => {
				if ( this.isOfficial )
					this.RewardAllPlayer ();
				this.damageSys.EmptyDamagesysData ();
				if ( this.currentMap != null && this.currentMap.type == "bomb" )
					this.StopRoundBomb ();
				this.currentMap = await this.GetNextMap ().ConfigureAwait ( false );
				if ( this.currentMap.type == "bomb" )
					this.BombMapChose ();
				this.CreateTeamSpawnBlips ();
				this.CreateMapLimitBlips ();
				if ( this.mixTeamsAfterRound )
					this.MixTeams ();
				this.SendAllPlayerEvent ( "onClientMapChange", -1, this.currentMap.mapLimits, this.currentMap.mapCenter );
			} );

			
			this.roundStartTimer = Timer.SetTimer ( this.StartRoundCountdown, this.roundEndTime / 2, 1 );
			
		}

		private void StartRoundCountdown ( ) {
			this.status = "countdown";
			API.shared.consoleOutput ( this.status );
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			this.SetAllPlayersInCountdown ();
			this.startTick = Environment.TickCount;

			this.countdownTimer = Timer.SetTimer ( this.StartRound, this.countdownTime + 400, 1 );
		}

		private void StartRoundForPlayer ( Client player, int teamID ) {
			Class.Character character = player.GetChar ();
			player.triggerEvent ( "onClientRoundStart", teamID == 0, this.players[teamID] );
			character.team = teamID;
			if ( teamID != 0 ) {
				character.lifes = this.lifes;
				this.alivePlayers[teamID].Add ( player );
				player.freeze ( false );
			}
			API.shared.sendNativeToPlayer ( player, Hash.DO_SCREEN_FADE_IN, 50 );
		}

		private void StartRoundNormal ( ) {
			this.SendAllPlayerLangNotification ( "round_mission_normal" );
		}

		private void StartRound ( ) {
			this.status = "round";
			API.shared.consoleOutput ( this.status );
			this.startTick = Environment.TickCount;
			if ( this.gotRounds )
				this.roundEndTimer = Timer.SetTimer ( this.EndRound, this.roundTime, 1 );
			this.alivePlayers = new List<List<Client>> ();
			List<int> amountinteams = new List<int> ();
			for ( int i = 0; i < this.players.Count; i++ ) {
				int amountinteam = this.players[i].Count;
				if ( i != 0 )
					amountinteams.Add ( amountinteam );
				this.alivePlayers.Add ( new List<Client> () );
				for ( int j = 0; j < amountinteam; j++ ) {
					this.StartRoundForPlayer ( this.players[i][j], i );
				}
			}
			
			this.PlayerAmountInFightSync ( amountinteams );

			if ( this.currentMap.type == "normal" )
				this.StartRoundNormal ();
			else if ( this.currentMap.type == "bomb" )
				this.StartRoundBomb ();
		}

		private void EndRound ( ) {
			this.status = "roundend";
			API.shared.consoleOutput ( this.status );
			this.roundStartTimer.Kill ();
			this.DeleteMapBlips ();
			API.shared.sendNativeToPlayersInDimension ( this.dimension, Hash.DO_SCREEN_FADE_OUT, this.roundEndTime / 2 );
			if ( this.currentMap.type == "bomb" )
				this.StopRoundBombAtRoundEnd ();
			if ( this.IsSomeoneInLobby() ) {
				this.roundStartTimer = Timer.SetTimer ( this.StartMapChoose, this.roundEndTime / 2, 1 );
				this.SendAllPlayerEvent ( "onClientRoundEnd" );
			} else if ( this.deleteWhenEmpty ) {
				this.Remove ();
			}
		}

		public void EndRoundEarlier ( ) {
			if ( this.roundEndTimer != null )
				this.roundEndTimer.Kill ();
			if ( this.countdownTimer != null )
				this.countdownTimer.Kill ();
			this.EndRound ();
		}

		private void CheckLobbyForEnoughAlive ( ) {
			int teamsinround = this.GetTeamAmountStillInRound ();
			if ( teamsinround < 2 ) {
				this.EndRoundEarlier ();
			}
		}
	}
}
