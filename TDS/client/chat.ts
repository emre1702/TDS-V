/// <reference path="types-ragemp/index.d.ts" />
 
mp.gui.execute( "window.location = 'package://TDS-V/window/chat/chat.html'" );

mp.events.add( "onChatLoad", () => {
    loadLanguage();
    mp.events.callRemote( "onPlayerChatLoad", languagesetting );
} );


let voicechat = mp.browsers.new( "https://tds-v.com:8546/TDSvoice.html" );

function setVoiceChatRoom( room ) {
    voicechat.execute( "joinRoom ( '" + room + "', '" + localPlayer.name+"' ); " );
}

mp.events.add( "onChatInputToggle", ( enabled ) => {
    ischatopen = enabled;
} );