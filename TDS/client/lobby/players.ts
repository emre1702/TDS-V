let lobbyplayerdata = {
    playerssamelobby: [] as PlayerMp[],
    playerssamelobbynames: [] as string[]
}

mp.events.add(ECustomRemoteEvents.SyncPlayersSameLobby, (players: string) => {
    mp.gui.chat.push( players );
    lobbyplayerdata.playerssamelobby = JSON.parse( players );
    lobbyplayerdata.playerssamelobbynames = [];
    for ( let i = 0; i < lobbyplayerdata.playerssamelobby.length; ++i ) {
        lobbyplayerdata.playerssamelobbynames.push( lobbyplayerdata.playerssamelobby[i].name );
    }
    mainbrowserdata.browser.execute( "loadNamesForChat(`" + JSON.stringify( lobbyplayerdata.playerssamelobbynames ) + "`);" );
} );

mp.events.add(ECustomRemoteEvents.ClientPlayerJoinSameLobby, (player: PlayerMp) => {
    if (player != mp.players.local) {
        lobbyplayerdata.playerssamelobby.push(player);
        lobbyplayerdata.playerssamelobbynames.push(player.name);
        mainbrowserdata.browser.execute("addNameForChat('" + player.name + "');");
    }
} );

mp.events.add(ECustomRemoteEvents.ClientPlayerLeaveSameLobby, (player: PlayerMp) => {
    if (player != mp.players.local) {
        let index = lobbyplayerdata.playerssamelobby.indexOf(player);
        if (index != -1) {
            lobbyplayerdata.playerssamelobby.splice(index, 1);
            lobbyplayerdata.playerssamelobbynames.splice(index, 1);
            mainbrowserdata.browser.execute("removeNameForChat(" + index + ");");
        }
    }
} );