using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Lobby : Script {

		public bool gotRounds = true;
		private int countdownTime = 3;
		private int roundTime = 4 * 60;
		public int roundEndTime = 8;
		private string status;

		public void Start ( ) {
			if ( this.isPlayable )
				this.StartMapChoose ();
		}

		public void StartMapChoose ( ) {
			this.status = "mapchoose";
			API.shared.consoleOutput ( this.status );

			this.currentMap = this.GetNextMap ();

			this.CreateTeamSpawnBlips ();
			this.CreateMapLimitBlips ();
			
			if ( this.mixTeamsAfterRound )
				this.MixTeams ();
			this.roundStartTimer = Timer.SetTimer ( this.StartRoundCountdown, this.roundEndTime * 1000 / 2, 1 );
			if ( this.currentMap.mapLimits.Count > 0 )
				this.SendAllPlayerEvent ( "sendClientMapData", -1, this.currentMap.mapLimits );
		}

		private void StartRoundCountdown ( ) {
			this.status = "countdown";
			API.shared.consoleOutput ( this.status );
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			for ( int i = 0; i < this.players.Count; i++ )
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					this.SetPlayerReadyForRound ( this.players[i][j], i );
					API.shared.sendNativeToPlayer ( this.players[i][j], Hash.DO_SCREEN_FADE_IN, this.countdownTime * 1000 );
					this.players[i][j].triggerEvent ( "onClientCountdownStart", this.currentMap.name );
					if ( i == 0 )
						this.SpectateAllTeams ( this.players[i][j], true );
				}
			this.countdownTimer = Timer.SetTimer ( this.StartRound, this.countdownTime * 1000 + 200, 1 );
		}

		private void StartRound ( ) {
			this.status = "round";
			API.shared.consoleOutput ( this.status );
			if ( this.gotRounds )
				this.roundEndTimer = Timer.SetTimer ( this.EndRound, this.roundTime * 1000, 1 );
			this.alivePlayers = new List<List<Client>> ();
			for ( int i = 0; i < this.players.Count; i++ ) {
				this.alivePlayers.Add ( new List<Client> () );
				for ( int j = 0; j < this.players[i].Count; j++ ) {
					Client player = this.players[i][j];
					Class.Character character = player.GetChar ();
					player.triggerEvent ( "onClientRoundStart", i == 0, this.players[i] );
					character.team = i;
					if ( i != 0 ) {
						character.lifes = this.lifes;
						this.alivePlayers[i].Add ( player );
						player.freeze ( false );
						this.GivePlayerWeapons ( player );
					}
				}
			}
		}

		private void EndRound ( ) {
			this.status = "roundend";
			API.shared.consoleOutput ( this.status );
			this.roundStartTimer.Kill ();
			for ( int i = 0; i < this.mapBlips.Count; i++ ) {
				this.mapBlips[i].delete ();
			}
			this.mapBlips = new List<Blip> ();
			bool foundone = false;
			API.shared.sendNativeToPlayersInDimension ( this.dimension, Hash.DO_SCREEN_FADE_OUT, this.roundEndTime / 2 * 1000 );
			for ( int i = 0; i < this.players.Count && !foundone; i++ ) {
				if ( this.players[i].Count > 0 )
					foundone = true;
			}
			if ( foundone ) {
				this.roundStartTimer = Timer.SetTimer ( this.StartMapChoose, this.roundEndTime * 1000 / 2, 1 );
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
