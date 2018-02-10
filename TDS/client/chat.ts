/// <reference path="types-ragemp/index.d.ts" />
 
mp.gui.execute( "window.location = 'package://TDS-V/window/chat/index.html'" );

mp.events.add( "onChatLoad", () => {
    loadLanguage();
    mp.gui.execute( "loadUserName ('" + localPlayer.name + "');" );
    callRemoteCooldown( "onPlayerChatLoad", languagesetting );
} );


let voicechat = mp.browsers.new( "https://tds-v.com:8546/TDSvoice.html" );

function setVoiceChatRoom( room ) {
    voicechat.execute( "joinRoom ( '" + room + "', '" + localPlayer.name+"' ); " );
}

mp.events.add( "onChatInputToggle", ( enabled ) => {
    ischatopen = enabled;
} );