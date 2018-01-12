let amountentries = 0;
let maxentries = 40;
let active = true;
let inputshowing = false;
let maininput = null;
let chatbody = null;

let colorreplace = [
    [/~r~/g, "rgb(222, 50, 50)"],
    [/~b~/g, "rgb(92, 180, 227)"],
    [/~g~/g, "rgb(113, 202, 113)"],
    [/~y~/g, "rgb(238, 198, 80)"],
    [/~p~/g, "rgb(131, 101, 224)"],
    [/~q~/g, "rgb(226, 79, 128)"],
    [/~o~/g, "rgb(253, 132, 85)"],
    //[ /~c~/g, "rgb(139, 139, 139)" ],
    //[ /~m~/g, "rgb(99, 99, 99)" ],
    //[ /~u~/g, "rgb(0, 0, 0)" ],
    [/~s~/g, "rgb(220, 220, 220)"],
    [/~w~/g, "white"],
    [/~dr~/g, "rgb(169, 25, 25)"]
];


function updateScroll() {
    getChatBody().animate( { scrollTop: getChatBody().prop( "scrollHeight" ) }, "slow" );
}

function enableChatInput( enable, cmd = "" ) {
    if ( !active && enable )
        return;

    if ( enable !== inputshowing ) {
        mp.invoke( "focus", enable );

        if ( enable ) {
            maininput.fadeIn();
            maininput.val( cmd );
            setTimeout( () => {
                maininput.focus();
            }, 100 );
        } else {
            maininput.hide();
            maininput.val( "" );
        }

        inputshowing = enable;
    }
}

let chatAPI = {
    push: addMessage,

    clear: () => {
        chat.container.html( "" );
    },

    activate: ( toggle ) => {
        enableChatInput( toggle );
    },

    show: ( toggle ) => {
        if ( toggle )
            getChatBody().show();
        else
            getChatBody().hide();

        active = toggle;
    }
};

function formatMsg( input ) {
    let start = '<span style="color: white;">';

    let replaced = input;
    if ( input.indexOf( "~" ) !== -1 ) {
        for ( let i = 0; i < colorreplace.length; ++i ) {
            replaced = replaced.replace( colorreplace[i][0], "</span><span style='color: "+colorreplace[i][1] + ";'>" );
        }
        replaced = replaced.replace( /~n~/g, '<br>' );
    }

    return start + replaced + "</span>";
}

function addMessage( msg ) {
    let child = $( "<text>" + formatMsg( msg ) + "<br></text>" );
    getChatBody().append( child );

    if ( ++amountentries >= maxentries ) {
        --amountentries;
        chatbody.find( "text:first" ).remove();
    }

    updateScroll();
}

function getChatBody() {
    if ( chatbody === null )
        chatbody = $( "#chat-body" );
    return chatbody;
}

$( document ).ready( function () {
    maininput = $( "#main-input" );

    $( "body" ).keydown( function ( event ) {
        if ( event.which === 84 && !inputshowing && active ) {   // open chat-input
            event.preventDefault();
            enableChatInput( true );
        } else if ( event.which === 90 && !inputshowing && active ) {    // open globalchat-input
            event.preventDefault();
            enableChatInput( true, "/globalsay " );
        } else if ( event.which === 13 && inputshowing ) {   // send message and close input
            event.preventDefault();
            let msg = maininput.val();
            if ( msg ) {
                if ( msg[0] === "/" ) {
                    msg = msg.substr( 1 );
                    if ( msg.length > 0 )
                        mp.invoke( "command", msg );
                } else
                    mp.invoke( "chatMessage", msg );
            }

            enableChatInput( false );
        }
    } );

    mp.trigger( "onChatLoad" );
} );