namespace TDS.server.instance.lobby {

	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using extend;
	using GTANetworkAPI;
	using player;
	using utility;

	partial class Lobby {

		public bool GotRounds = true;
		private uint countdownTime = 5 * 1000;
		private uint roundTime = 4 * 60 * 1000;
		public uint RoundEndTime = 8 * 1000;
		private string status = "loading";
		private long startTick;

		public void Start () {
			if ( this.IsPlayable ) {
				if ( this.GotRounds )
					this.StartMapChoose ();
			}
		}

		public async void StartMapChoose () {
			try {
				this.status = "mapchoose";
				this.API.ConsoleOutput ( this.status );
				if ( this.IsOfficial )
					this.RewardAllPlayer ();
				this.DmgSys.EmptyDamagesysData ();

				await Task.Run ( async ( ) => {
					if ( this.currentMap != null && this.currentMap.Type == "bomb" )
						this.StopRoundBomb ();
					this.currentMap = await this.GetNextMap ().ConfigureAwait ( false );
					if ( this.currentMap.Type == "bomb" )
						this.BombMapChose ();
					this.CreateTeamSpawnBlips ();
					this.CreateMapLimitBlips ();
					if ( this.mixTeamsAfterRound )
						this.MixTeams ();
					this.SendAllPlayerEvent ( "onClientMapChange", -1, this.currentMap.MapLimits, this.currentMap.MapCenter );
				} );

				this.roundStartTimer = Timer.SetTimer ( this.StartRoundCountdown, this.RoundEndTime / 2 );
			} catch ( Exception ex ) {
				this.API.ConsoleOutput ( "Error in StartMapChoose:" + ex.Message );
			}
		}

		private void StartRoundCountdown () {
			this.status = "countdown";
			this.API.ConsoleOutput ( this.status );
			this.spectatingMe = new Dictionary<Client, List<Client>> ();
			this.SetAllPlayersInCountdown ();
			this.startTick = Environment.TickCount;

			this.countdownTimer = Timer.SetTimer ( this.StartRound, this.countdownTime + 400 );
		}

		private void StartRoundForPlayer ( Client player, uint teamID ) {
			Character character = player.GetChar ();
			player.TriggerEvent ( "onClientRoundStart", teamID == 0, this.Players[(int)teamID] );
			character.Team = teamID;
			if ( teamID != 0 ) {
				character.Lifes = this.Lifes;
				this.alivePlayers[(int)teamID].Add ( player );
				player.Freeze ( false );
			}
		}

		private void StartRoundNormal () {
			this.SendAllPlayerLangNotification ( "round_mission_normal" );
		}

		private void StartRound () {
			this.status = "round";
			this.API.ConsoleOutput ( this.status );
			this.startTick = Environment.TickCount;
			if ( this.GotRounds )
				this.roundEndTimer = Timer.SetTimer ( this.EndRound, this.roundTime );
			this.alivePlayers = new List<List<Client>> ();
			List<uint> amountinteams = new List<uint> ();
			for ( int i = 0; i < this.Players.Count; i++ ) {
				uint amountinteam = (uint) this.Players[i].Count;
				if ( i != 0 )
					amountinteams.Add ( amountinteam );
				this.alivePlayers.Add ( new List<Client> () );
				for ( int j = 0; j < amountinteam; j++ ) {
					this.StartRoundForPlayer ( this.Players[i][j], (uint) i );
				}
			}

			this.PlayerAmountInFightSync ( amountinteams );

			if ( this.currentMap.Type == "normal" )
				this.StartRoundNormal ();
			else if ( this.currentMap.Type == "bomb" )
				this.StartRoundBomb ();
		}

		private void EndRound () {
			this.status = "roundend";
			this.API.ConsoleOutput ( this.status );
			this.roundStartTimer.Kill ();
			this.DeleteMapBlips ();
			if ( this.currentMap.Type == "bomb" )
				this.StopRoundBombAtRoundEnd ();
			if ( this.IsSomeoneInLobby () ) {
				this.roundStartTimer = Timer.SetTimer ( this.StartMapChoose, this.RoundEndTime / 2 );
				this.SendAllPlayerEvent ( "onClientRoundEnd" );
			} else if ( this.DeleteWhenEmpty ) {
				this.Remove ();
			}
		}

		public void EndRoundEarlier () {
			if ( this.roundEndTimer != null )
				this.roundEndTimer.Kill ();
			if ( this.countdownTimer != null )
				this.countdownTimer.Kill ();
			this.EndRound ();
		}

		private void CheckLobbyForEnoughAlive () {
			int teamsinround = this.GetTeamAmountStillInRound ();
			if ( teamsinround < 2 ) {
				this.EndRoundEarlier ();
			}
		}
	}

}
