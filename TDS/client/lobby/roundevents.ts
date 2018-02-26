/// <reference path="../types-ragemp/index.d.ts" />


mp.events.add( "onClientMapChange", function ( mapname, maplimit, mapmidx, mapmidy, mapmidz ) {
    log( "onClientMapChange" );
    setMapInfo( mapname );
    hideRoundEndReason();
	mp.game.cam.doScreenFadeIn( lobbysettings.roundendtime / 2 );
	maplimit = JSON.parse( maplimit );
	if ( maplimit.length > 0 )
		loadMapLimitData( maplimit );
    loadMapMiddleForCamera( new mp.Vector3( mapmidx, mapmidy, mapmidz ) );
    toggleFightControls( false );
} );


mp.events.add( "onClientCountdownStart", function ( resttime ) {
	log( "onClientCountdownStart" );
    if ( cameradata.timer != null ) {
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
    toggleFightControls( false );
} );


mp.events.add( "onClientRoundStart", function ( isspectator, wastedticks ) {
	log( "onClientRoundStart" );
	mp.game.cam.doScreenFadeIn( 50 );
	stopCountdownCamera();
	endCountdown();
	rounddata.isspectator = isspectator == 1;
	if ( !rounddata.isspectator ) {
		startMapLimit();
		toggleFightMode( true );
	}
    roundStartedRoundInfo( wastedticks );
    toggleFightControls( true );
    damagesysdata.shotsdoneinround = 0;
} );


mp.events.add( "onClientRoundEnd", function ( reason ) {
	log( "onClientRoundEnd" );
	mp.game.cam.doScreenFadeOut ( lobbysettings.roundendtime / 2 );
	toggleFightMode( false );
	removeBombThings();
	emptyMapLimit();
	removeRoundThings( false );
	stopCountdown();
	stopCountdownCamera();
    removeRoundInfo();
    toggleFightControls( false );
    clearMapVotingsInBrowser();
    showRoundEndReason( reason, rounddata.currentMap );
} );


mp.events.add( "onClientPlayerSpectateMode", function () {
	log( "onClientPlayerSpectateMode" );
	rounddata.isspectator = true;
	startSpectate();
} );


mp.events.add( "onClientPlayerDeath", function ( playerID: number, teamID: number, killstr: string ) {
    log( "onClientPlayerDeath" );
    let player = mp.players.at( playerID );
	if ( mp.players.local == player ) {
		toggleFightMode( false );
		removeBombThings();
	}
	playerDeathRoundInfo( teamID, killstr );
} );


mp.events.add( "PlayerQuit", function ( player: PlayerMp, exitType: string, reason: string ) {
	log( "PlayerQuit" );
	//removeTeammateFromTeamBlips( player.name );
} );


mp.events.add( "onClientPlayerGotBomb", function ( placestoplant ) {
	localPlayerGotBomb( placestoplant );
} );


mp.events.add( "onClientPlayerPlantedBomb", function ( ) {
	localPlayerPlantedBomb();
} );


mp.events.add( "onClientBombPlanted", function ( pos, candefuse ) {
	bombPlanted( pos, candefuse );
} );


mp.events.add( "onClientBombDetonated", function ( ) {
	bombDetonated();
} );


mp.events.add( "onClientPlayerTeamChange", function ( teamID, teamUID ) {
    setVoiceChatRoom( teamUID );
} );