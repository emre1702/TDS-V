/// <reference path="types-ragemp/index.d.ts" />
 
mp.events.add( "onChatLoad", () => {
    loadLanguage();
    mainbrowserdata.browser.execute( "loadUserName ('" + mp.players.local.name + "');" );
    callRemoteCooldown( "onPlayerChatLoad", languagesetting );
} );


let voicechat = mp.browsers.new( "https://tds-v.com:8546/TDSvoice.html" );

function setVoiceChatRoom( room ) {
    voicechat.execute( "joinRoom ( '" + room + "', '" + localPlayer.name+"' ); " );
}

mp.events.add( "onChatInputToggle", ( enabled ) => {
    ischatopen = enabled;
} );