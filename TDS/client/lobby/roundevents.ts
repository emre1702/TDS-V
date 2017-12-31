/// <reference path="../types-ragemp/index.d.ts" />


mp.events.add( "onClientMapChange", function ( maplimit, mapmidx, mapmidy, mapmidz ) {
	log( "onClientMapChange" );
	mp.game.cam.doScreenFadeIn( lobbysettings.roundendtime / 2 );
	maplimit = JSON.parse( maplimit );
	if ( maplimit.length > 0 )
		loadMapLimitData( maplimit );
	loadMapMiddleForCamera( new mp.Vector3 ( mapmidx, mapmidy, mapmidz ) );
} );


mp.events.add( "onClientCountdownStart", function ( mapname: string, resttime ) {
	log( "onClientCountdownStart" );
	if ( cameradata.timer != null )
		cameradata.timer.kill();
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
	rounddata.mapinfo.setText( mapname );
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
	roundStartedRoundInfo( wastedticks )
} );


mp.events.add( "onClientRoundEnd", function () {
	log( "onClientRoundEnd" );
	mp.game.cam.doScreenFadeOut ( lobbysettings.roundendtime / 2 );
	toggleFightMode( false );
	removeBombThings();
	emptyMapLimit();
	removeRoundThings( false );
	stopCountdown();
	stopCountdownCamera();
	removeRoundInfo();
	//stopMapVoting();
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


mp.events.add( "PlayerQuit", function ( player: MpPlayer, exitType: string, reason: string ) {
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
