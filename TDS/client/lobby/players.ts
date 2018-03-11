let lobbyplayerdata = {
    playerssamelobby: [] as PlayerMp[],
    playerssamelobbynames: [] as string[]
}

mp.events.add( "syncPlayersSameLobby", ( players: string ) => {
    mp.gui.chat.push( players );
    lobbyplayerdata.playerssamelobby = JSON.parse( players );
    lobbyplayerdata.playerssamelobbynames = [];
    for ( let i = 0; i < lobbyplayerdata.playerssamelobby.length; ++i ) {
        lobbyplayerdata.playerssamelobbynames.push( lobbyplayerdata.playerssamelobby[i].name );
    }
    mainbrowserdata.browser.execute( "loadNamesForChat(`" + JSON.stringify( lobbyplayerdata.playerssamelobbynames ) + "`);" );
} );

mp.events.add( "joinPlayerSameLobby", ( player: PlayerMp ) => {
    lobbyplayerdata.playerssamelobby.push( player );
    lobbyplayerdata.playerssamelobbynames.push( player.name );
    mainbrowserdata.browser.execute( "addNameForChat('" + player.name + "');" );
} );

mp.events.add( "leavePlayerSameLobby", ( player: PlayerMp ) => {
    let index = lobbyplayerdata.playerssamelobby.indexOf( player );
    if ( index != -1 ) {
        lobbyplayerdata.playerssamelobby.splice( index, 1 );
        lobbyplayerdata.playerssamelobbynames.splice( index, 1 );
        mainbrowserdata.browser.execute( "removeNameForChat(" + index + ");" );
    }
} );