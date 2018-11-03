/// <reference path="../types-ragemp/index.d.ts" />
/// <reference path="../enum/customremoteevents.ts" />

mp.events.add(ECustomRemoteEvents.ClientPlayerJoinLobby, (lobbyid, isspectator, mapname, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime, lobbywithmaps) => {
    log( "onClientPlayerJoinLobby" );
    if ( lobbysettings.id == 0 && lobbyid != 0 ) // not mainmenu
        destroyLobbyChoiceBrowser();
    else if ( lobbyid == 0 )  // mainmenu
        mainMenuJoined();

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

mp.events.add(ECustomRemoteEvents.ClientPlayerLeaveSameLobby, (playerID: number) => {
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
        hideRoundEndReason();
	}
} );

function mainMenuJoined() {
    mp.game.cam.doScreenFadeIn( 100 );
    startLobbyChoiceBrowser();
}

mp.events.add( "onClientPlayerJoinMapCreatorLobby", () => {
    startMapCreator();
} );