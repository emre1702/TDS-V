let amountentries = 0;
let maxentries = 40;
let active = true;
let inputshowing = false;
let mainInput = null;

function updateScroll() {
	var body = $( "#chat-body" );
	body.animate( { scrollTop: body.prop ( "scrollHeight" ) }, "slow" );	
}

function enableChatInput( enable, cmd = "" ) {
    if ( !active && enable )
        return;

    if ( enable !== inputshowing ) {
        mp.invoke( "focus", enable );

        if ( enable ) {
            mainInput.fadeIn();
            mainInput.val( cmd );
            setTimeout( () => {
                mainInput.focus();
            }, 100 );
        } else {
            mainInput.hide();
            mainInput.val( "" );
        }

        inputshowing = enable;
    }
}

var chatAPI = {
    push: addMessage,

    clear: () => {
        chat.container.html( "" );
    },

    activate: ( toggle ) => {
        //don't do anything from RageMP, only do it by yourself in your script with browser.execute
    },

    show: ( toggle ) => {
        if ( toggle )
            $( "#chat-body" ).show();
        else
            $( "#chat-body" ).hide();

        active = toggle;
    }
};

function formatMsg(input) {
	var start = '<span style="color: white;">';

    let replaced = input;
    if ( input.indexOf( "~" ) !== -1 ) {
        replaced = replaced.replace( /~r~/g, '</span><span style="color: rgb(222, 50, 50);">' )
                            .replace( /~b~/g, '</span><span style="color: rgb(92, 180, 227);">' )
                            .replace( /~g~/g, '</span><span style="color: rgb(113, 202, 113);">' )
                            .replace( /~y~/g, '</span><span style="color: rgb(238, 198, 80);">' )
                        //  .replace( /~p~/g, '</span><span style="color: rgb(131, 101, 224);">' )
                        //  .replace( /~q~/g, '</span><span style="color: rgb(226, 79, 128);">' )
                            .replace( /~o~/g, '</span><span style="color: rgb(253, 132, 85);">' )
                        //  .replace( /~c~/g, '</span><span style="color: rgb(139, 139, 139);">' )
                        //  .replace( /~m~/g, '</span><span style="color: rgb(99, 99, 99);">' )
                        //  .replace( /~u~/g, '</span><span style="color: rgb(0, 0, 0);">' )
                            .replace( /~s~/g, '</span><span style="color: rgb(220, 220, 220);">' )
                            .replace( /~w~/g, '</span><span style="color: white;">' )		// white
                            .replace( /~dr~/g, '</span><span style="color: rgb( 169, 25, 25 );">' )		// dark red
                            .replace( /~n~/g, '<br>' );
    }

    return start + replaced + "</span>";
}

function addMessage(msg) {
	var child = $( "<text>" + formatMsg( msg ) + "<br></text>");
	child.hide();
	var chatbody = $( "#chat-body" );
	chatbody.append( child );
	child.fadeIn();

    if ( ++amountentries >= maxentries ) {
        --amountentries;
		chatbody.find( "text:first" ).remove();
	}

	updateScroll();
}

$( document ).ready( function () {
    mainInput = $( "#main-input" );

    $( "body" ).keydown( function ( event ) {
        if ( event.which == 84 && !inputshowing && active ) {   // open chat-input
            event.preventDefault();
            enableChatInput( true );
        } else if ( event.which == 90 && !inputshowing && active ) {    // open globalchat-input
            event.preventDefault();
            enableChatInput( true, "/globalsay " );
        } else if ( event.which == 13 && inputshowing ) {   // send message and close input
            event.preventDefault();
            var m = mainInput.val();
            if ( m && m.length > 0 ) {
                if ( m[0] == "/" ) {
                    m = m.substr( 1 );
                    if ( m.length > 0 )
                        mp.invoke( "command", m );
                } else
                    mp.invoke( "chatMessage", m );
            }

            enableChatInput( false );	
        }
    } );

    mp.trigger( "onChatLoad" );
} );