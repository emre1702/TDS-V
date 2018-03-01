/// <reference path="types-ragemp/index.d.ts" />
 
mp.events.add( "onChatLoad", () => {
    loadSettings();
    mainbrowserdata.browser.execute( "loadUserName ('" + mp.players.local.name + "');" );
    callRemoteCooldown( "onPlayerChatLoad", settingsdata.language, settingsdata.hitsound );
} );


let voicechat = mp.browsers.new( "https://tds-v.com:8546/TDSvoice.html" );

function setVoiceChatRoom( room ) {
    voicechat.execute( "joinRoom ( '" + room + "', '" + localPlayer.name+"' ); " );
}

mp.events.add( "onChatInputToggle", ( enabled ) => {
    ischatopen = enabled;
} );

mp.events.add( "playerJoin", ( player: PlayerMp ) => {
    mainbrowserdata.browser.execute( "addNameForChat ( '" + player.name + "');" );
} );

mp.events.add( "playerQuit", ( player: PlayerMp, exitType: string, reason: string ) => {
    mainbrowserdata.browser.execute( "removeNameForChat ( '" + player.name + "');" );
} );