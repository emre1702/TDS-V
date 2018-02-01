/// <reference path="../types-ragemp/index.d.ts" />

mp.events.add( "onClientPlayerJoinLobby", ( lobbyid, isspectator, mapname, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime, lobbywithmaps ) => {
    log( "onClientPlayerJoinLobby" );
    lobbysettings.id = lobbyid;
    if ( typeof isspectator !== "undefined" ) {
        rounddata.isspectator = isspectator;
        setMapInfo( mapname );
        teamnames = JSON.parse( teamnames );
        teamcolors = JSON.parse( teamcolors );
        addTeamInfos( teamnames, teamcolors );
        lobbysettings.countdowntime = countdowntime;
        roundinfo.roundtime = roundtime;
        lobbysettings.bombdetonatetime = bombdetonatetime;
        lobbysettings.bombplanttime = bombplanttime;
        lobbysettings.bombdefusetime = bombdefusetime;
        lobbysettings.roundendtime = roundendtime;
        mapvotingdata.inmaplobby = lobbywithmaps;
    } else {
        mapvotingdata.inmaplobby = false;
    }
} );

mp.events.add( "onClientPlayerLeaveLobby", ( playerID: number ) => {
    log( "onClientPlayerLeaveLobby" );
    let player = mp.players.at( playerID );
	if ( mp.players.local == player ) {
		toggleFightMode( false );
		removeBombThings();
		removeRoundThings( true );
        stopCountdownCamera();
        closeMapVotingMenu();  
        clearMapVotingsInBrowser();
        removeRoundInfo();
        stopMapCreator();
	}
} );

mp.events.add( "onClientJoinMainMenu", () => {
	mp.game.cam.doScreenFadeIn( 100 );
} );

mp.events.add( "onClientPlayerJoinMapCreatorLobby", () => {
    startMapCreator();
} );