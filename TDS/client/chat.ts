/// <reference path="types-ragemp/index.d.ts" />
 
mp.gui.execute( "window.location = 'package://TDS-V/window/chat/chat.html'" );

mp.events.add( "onChatLoad", () => {
    mp.events.callRemote( "onPlayerChatLoad", languagesetting );
} );
