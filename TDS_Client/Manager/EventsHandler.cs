using RAGE;
using RAGE.Elements;
using Cam = RAGE.Game.Cam;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw.Scaleform;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using static RAGE.Events;

namespace TDS_Client.Manager
{
    class EventsHandler : Script
    {
        public EventsHandler()
        {
            LoadOnStart();
            AddRAGEEvents();
            AddToClientEvents();
            AddFromBrowserEvents();
        }

        #region Load on start
        private void LoadOnStart()
        {
            Settings.Load();
        }
        #endregion Load on start

        #region RAGE events
        private void AddRAGEEvents()
        {
            Tick += OnTickMethod;
            OnPlayerWeaponShot += OnPlayerWeaponShotMethod;
            OnPlayerSpawn += OnPlayerSpawnMethod;
            OnPlayerDeath += OnPlayerDeathMethod;
        }

        private void OnTickMethod(List<TickNametagData> nametags)
        {
            Damagesys.ShowBloodscreenIfNecessary();
            ScaleformMessage.Render();
            if (Bomb.CheckPlantDefuseOnTick)
                Bomb.CheckPlantDefuse();
            if (RoundInfo.RefreshOnTick)
                RoundInfo.RefreshTime();
        }

        private void OnPlayerWeaponShotMethod(Vector3 targetPos, Player target, CancelEventArgs cancel)
        {
            Damagesys.OnWeaponShot(targetPos, target, cancel);
        }

        private void OnPlayerSpawnMethod(CancelEventArgs cancel)
        {
            Death.PlayerSpawn();
        }

        private void OnPlayerDeathMethod(Player player, uint reason, Player killer, CancelEventArgs cancel)
        {
            Death.PlayerDeath(player);
        }
        #endregion RAGE events

        #region From Server events 
        private void AddToClientEvents()
        {
            Add(DToClientEvent.LoadOwnMapRatings, OnLoadOwnMapRatingsMethod);
            Add(DToClientEvent.HitOpponent, OnHitOpponentMethod);
            Add(DToClientEvent.LoadSettings, OnLoadSettingsMethod);
            Add(DToClientEvent.JoinLobby, OnJoinLobbyMethod);
            Add(DToClientEvent.JoinSameLobby, OnJoinSameLobbyMethod);
            Add(DToClientEvent.LeaveSameLobby, OnLeaveSameLobbyMethod);
            Add(DToClientEvent.MapChange, OnMapChangeMethod);
            Add(DToClientEvent.CountdownStart, OnCountdownStartMethod);
            Add(DToClientEvent.RoundStart, OnRoundStartMethod);
            Add(DToClientEvent.RoundEnd, OnRoundEndMethod);
            Add(DToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            Add(DToClientEvent.Death, OnDeathMethod);
            Add(DToClientEvent.PlayerGotBomb, OnPlayerGotBombMethod);
            Add(DToClientEvent.PlayerPlantedBomb, OnPlayerPlantedBombMethod);
            Add(DToClientEvent.BombPlanted, OnBombPlantedMethod);
            Add(DToClientEvent.BombDetonated, OnBombDetonatedMethod);
            Add(DToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            Add(DToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(DToClientEvent.SyncCurrentMapName, OnSyncCurrentMapNameMethod);
        }

        private void OnLoadOwnMapRatingsMethod(object[] args)
        {
            string datajson = (string) args[0];
            MainBrowser.OnLoadOwnMapRatings(datajson);
        }

        private void OnHitOpponentMethod(object[] args)
        {
            DeathmatchInfo.HittedOpponent();
        }

        private void OnLoadSettingsMethod(object[] args)
        {
            Settings.LoadSyncedSettings((SyncedSettingsDto)args[0]);
        }

        private void OnJoinLobbyMethod(object[] args)
        {
            SyncedLobbySettingsDto settings = (SyncedLobbySettingsDto)args[0];
            Settings.LoadSyncedLobbySettings(settings);
            Players.Load((List<Player>)args[1]);
            Team.CurrentLobbyTeams = (SyncedTeamDataDto[]) args[2];
            Lobby.Lobby.Joined(settings);
        }

        private void OnJoinSameLobbyMethod(object[] args)
        {
            Players.Load((Player)args[0]);
        }

        private void OnLeaveSameLobbyMethod(object[] args)
        {
            Players.Remove((Player)args[0]);
        }

        private void OnMapChangeMethod(object[] args)
        {
            /*
             * setMapInfo( mapname );
    hideRoundEndReason();
	mp.game.cam.doScreenFadeIn( lobbysettings.roundendtime / 2 );
	maplimit = JSON.parse( maplimit );
	if ( maplimit.length > 0 )
		loadMapLimitData( maplimit );
    setCameraToMapCenter( new mp.Vector3( mapmidx, mapmidy, mapmidz ) );
    toggleFightControls( false );*/
        }

        private void OnCountdownStartMethod(object[] args)
        {
            /*
             * if ( cameradata.timer != null ) {
        cameradata.timer.kill();
        cameradata.timer = null;
    }
	if ( resttime == null ) {
		startCountdown();
		cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1, 1 );
	} else {
		startCountdownAfterwards( Math.ceil( ( lobbysettings.countdowntime - resttime ) / 1000 ) );
		if ( resttime > lobbysettings.countdowntime * 0.1 ) {
			setCameraGoTowardsPlayer( lobbysettings.countdowntime - resttime );
		} else {
			cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1 - resttime, 1 );
		}
	}
	if ( rounddata.isspectator )
        startSpectate();
    hideRoundEndReason();
    toggleFightControls( false );*/
        }

        private void OnRoundStartMethod(object[] args)
        {
            Cam.DoScreenFadeIn(50);

            /*
	stopCountdownCamera();
	endCountdown();
	rounddata.isspectator = isspectator == 1;
	if ( !rounddata.isspectator ) {
		startMapLimit();
		toggleFightMode( true );
	}
    roundStartedRoundInfo( wastedticks );
    toggleFightControls( true );
    damagesysdata.shotsdoneinround = 0;*/
        }

        private void OnRoundEndMethod(object[] args)
        {
            Cam.DoScreenFadeOut(Settings.RoundEndTime / 2);
            /*
	toggleFightMode( false );
	removeBombThings();
	emptyMapLimit();
	removeRoundThings( false );
	stopCountdown();
	stopCountdownCamera();
    removeRoundInfo();
    toggleFightControls( false );
    clearMapVotingsInBrowser();
    showRoundEndReason( reason, rounddata.currentMap );*/
        }

        private void OnPlayerSpectateModeMethod(object[] args)
        {
            /*
             * rounddata.isspectator = true;
	startSpectate();*/
        }

        private void OnDeathMethod(object[] args)
        {
            Player player = (Player)args[0];
            if (player == Player.LocalPlayer)
            {
                Round.InFight = false;
                Bomb.Reset();
            }
            RoundInfo.OnePlayerDied((int)args[1], (string)args[2]);

            /*
	if ( mp.players.local == player ) {
		toggleFightMode( false );
		removeBombThings();
	}
	playerDeathRoundInfo( teamID, killstr );*/
        }

        private void OnPlayerGotBombMethod(object[] args)
        {
            Bomb.LocalPlayerGotBomb((List<Vector3>)args[0]);
        }

        private void OnPlayerPlantedBombMethod(object[] args)
        {
            Bomb.LocalPlayerPlantedBomb();
        }

        private void OnBombPlantedMethod(object[] args)
        {
            Bomb.BombPlanted((Vector3)args[0], (bool)args[1]);
        }

        private void OnBombDetonatedMethod(object[] args)
        {
            Bomb.Detonate();
        }

        private void OnPlayerTeamChangeMethod(object[] args)
        {
            // setVoiceChatRoom( teamUID );
        }

        private void OnAmountInFightSyncMethod(object[] args)
        {
            foreach (var team in Team.CurrentLobbyTeams)
            {
                team.AmountPlayers = (SyncedTeamPlayerAmountDto)args[0];
            }
        }

        private void OnSyncCurrentMapNameMethod(object[] args)
        {
            MapInfo.SetMapInfo((string)args[0]);
        }
        #endregion From Server events

        #region From Browser events 
        private void AddFromBrowserEvents()
        {
            Add(DFromBrowserEvent.AddRatingToMap, OnAddRatingToMapMethod);
            Add(DFromBrowserEvent.ChooseLobbyToJoin, OnChooseLobbyToJoinMethod);
            Add(DFromBrowserEvent.SyncChoiceLanguageTexts, OnSyncChoiceLanguageTextsMethod);
        }

        private void OnAddRatingToMapMethod(object[] args)
        {
            string currentmap = (string)args[0];
            int rating = (int)args[1];
            MainBrowser.OnSendMapRating(currentmap, rating);
        }

        private void OnChooseLobbyToJoinMethod(object[] args)
        {
            Choice.JoinLobby((int)args[0], (int)args[1]);
        }

        private void OnSyncChoiceLanguageTextsMethod(object[] args)
        {
            Choice.SyncLanguageTexts();
        }
        #endregion Browser events 
    }
}
