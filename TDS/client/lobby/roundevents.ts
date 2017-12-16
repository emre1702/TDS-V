/// <reference path="../types-ragemp/index.d.ts" />


mp.events.add( "onClientMapChange", function ( args ) {
	log( "onClientMapChange start" );
	mp.game.cam.doScreenFadeIn ( lobbysettings.roundendtime / 2 );
	if ( args[0].Count > 0 )
		loadMapLimitData( args[0] );
	loadMapMiddleForCamera( args[1] );
	log( "onClientMapChange end" );
} );


mp.events.add( "onClientCountdownStart", function ( args ) {
	log( "onClientCountdownStart start " );
	if ( cameradata.timer != null )
		cameradata.timer.kill();
	if ( !( 1 in args ) ) {
		startCountdown();
		cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1, 1 );
	} else {
		startCountdownAfterwards( Math.ceil(( lobbysettings.countdowntime - args[1] ) / 1000 ) );
		if ( args[1] > lobbysettings.countdowntime * 0.1 ) {
			setCameraGoTowardsPlayer( lobbysettings.countdowntime - args[1] );
		} else {
			cameradata.timer = new Timer( setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1 - args[1], 1 );
		}
	}
	if ( rounddata.isspectator )
		startSpectate();
	rounddata.mapinfo.setText( args[0] );
	log( "onClientCountdownStart end" );
} );


mp.events.add( "onClientRoundStart", function ( args ) {
	log( "onClientRoundStart start" );
	mp.game.cam.doScreenFadeIn ( 50 );
	stopCountdownCamera();
	endCountdown();
	rounddata.isspectator = args[0];
	if ( !rounddata.isspectator ) {
		startMapLimit();
		createTeamBlips( args[1] );
		toggleFightMode( true );
	}
	roundStartedRoundInfo( args )
	log( "onClientRoundStart end" );
} );


mp.events.add( "onClientRoundEnd", function ( args ) {
	log( "onClientRoundEnd start" );
	mp.game.cam.doScreenFadeOut ( lobbysettings.roundendtime / 2 );
	toggleFightMode( false );
	removeBombThings();
	emptyMapLimit();
	removeRoundThings( false );
	stopCountdown();
	stopCountdownCamera();
	stopTeamBlips();
	removeRoundInfo();
	stopMapVoting();
	log( "onClientRoundEnd end" );
} );


mp.events.add( "onClientPlayerSpectateMode", function ( args ) {
	log( "onClientPlayerSpectateMode start" );
	rounddata.isspectator = true;
	startSpectate();
	log( "onClientPlayerSpectateMode end" );
} );


mp.events.add( "onClientPlayerDeath", function ( player: MpPlayer, teamID: number, killstr: string ) {
	log( "onClientPlayerDeath start" );
	if ( mp.players.local == player ) {
		toggleFightMode( false );
		removeBombThings();
	} else {
		removeTeammateFromTeamBlips( player.name );
	}
	playerDeathRoundInfo( teamID, killstr );
	log( "onClientPlayerDeath end" );
} );


mp.events.add( "PlayerQuit", function ( player: MpPlayer, exitType: string, reason: string ) {
	log( "onClientPlayerQuit start" );
	removeTeammateFromTeamBlips( player.name );
	log( "onClientPlayerQuit end" );
} );


mp.events.add( "onClientPlayerGotBomb", function ( args ) {
	localPlayerGotBomb( args[0] );
} );


mp.events.add( "onClientPlayerPlantedBomb", function ( args ) {
	localPlayerPlantedBomb();
} );


mp.events.add( "onClientBombPlanted", function ( args ) {
	bombPlanted ( args[0], args[1] );
} );


mp.events.add( "onClientBombDetonated", function ( args ) {
	bombDetonated();
} );

