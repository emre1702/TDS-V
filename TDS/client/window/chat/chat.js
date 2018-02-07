let amountentries = [0, 0];
let maxentries = 40;
let active = true;
let inputshowing = false;
let maininput = $( "#main-input" );
let chatbodies = [
    $( "#normal-chat-body" ),
    $( "#dirty-chat-body" )
];
let chatends = ["$normal$", "$dirty$"];
let chosentab;
let chosenchatbody = 0;
let myname = null;
let globalsaykeycode = String.fromCharCode( 90 ) === "Z" ? 90 : Y;

let colorreplace = [
    [/#r#/g, "rgb(222, 50, 50)"],
    [/#b#/g, "rgb(92, 180, 227)"],
    [/#g#/g, "rgb(113, 202, 113)"],
    [/#y#/g, "rgb(238, 198, 80)"],
    [/#p#/g, "rgb(131, 101, 224)"],
    [/#q#/g, "rgb(226, 79, 128)"],
    [/#o#/g, "rgb(253, 132, 85)"],
    //[ /#c#/g, "rgb(139, 139, 139)" ],
    //[ /#m#/g, "rgb(99, 99, 99)" ],
    //[ /#u#/g, "rgb(0, 0, 0)" ],
    [/#s#/g, "rgb(220, 220, 220)"],
    [/#w#/g, "white"],
    [/#dr#/g, "rgb(169, 25, 25)"]
];


function updateScroll( chatbody = null ) {
    chatbody = chatbody === null ? getChatBody() : chatbody;
    chatbody.animate( { scrollTop: chatbody.prop( "scrollHeight" ) }, "slow" );
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
        mp.trigger( "onChatInputToggle", enable );
    }
}
let chatAPI = {};

chatAPI["push"] = addMessage;

chatAPI["clear"] = () => {
    getChatBody().html( "" );
};

chatAPI["activate"] = ( toggle ) => {
    enableChatInput( toggle );
    active = toggle;
};

chatAPI["show"] = ( toggle ) => {
    if ( toggle ) {
        getChatBody().show();
        $( "#chat_choice" ).show();
    } else {
        getChatBody().hide();
        $( "#chat_choice" ).hide();
    }

    if ( !toggle && inputshowing )
        enableChatInput( false );

    active = toggle;
};

function formatMsg( input, ismentioned ) {
    let start = ""; 
    if ( ismentioned )
        start = '<span style="background-color: rgba(255,178,102,0.5);">';

    start += '<span style="color: white;">';

    let replaced = input;
    if ( input.indexOf( "#" ) !== -1 ) {
        for ( let i = 0; i < colorreplace.length; ++i ) {
            replaced = replaced.replace( colorreplace[i][0], "</span><span style='color: "+colorreplace[i][1] + ";'>" );
        }
        replaced = replaced.replace( /#n#/g, '<br>' );
    }

    if ( ismentioned )
        replaced += "</span>";

    return start + replaced + "</span>";
}

function isMentioned( msg ) {
    if ( myname === null )
        return false;
    let firstindex = msg.indexOf( "@" );
    if ( firstindex === -1 )
        return false;
    let lastindex = msg.indexOf( ":", firstindex + 1 );
    if ( lastindex === -1 )
        return false;
    let name = msg.substring( firstindex + 1, lastindex );
    if ( name === myname )
        return true;
    return false;
}

function addChildToChatBody( child, chatbody, index ) {
    chatbody.append( child );
    if ( ++amountentries[index] >= maxentries ) {
        --amountentries[index];
        chatbody.find( "text:first" ).remove();
    }
    updateScroll ( chatbody );
}

function addMessage( msg ) {
    let ismentioned = isMentioned( msg );

    // output in the chatbody when ending with one of chatends //
    for ( let i = 0; i < chatends.length; ++i ) {
        if ( msg.endsWith( chatends[i] ) ) {
            msg = msg.slice( 0, -chatends[i].length );
            let chatbody = getChatBody( i );
            let formattedmsg = formatMsg( msg, ismentioned );
            let child = $( "<text>" + formattedmsg + "</text>" );
            addChildToChatBody( child, chatbody, i );
            return;
        }
    }

    let formattedmsg = formatMsg( msg, ismentioned );
    // else output in all chatbodies //
    for ( let i = 0; i < chatbodies.length; ++i ) {
        let chatbody = getChatBody( i );
        let child = $( "<text>" + formattedmsg + "</text>" );
        addChildToChatBody( child, chatbody, i );
    }

}

function getChatBody( index = -1 ) {
    let theindex = index === -1 ? chosenchatbody : index;
    return chatbodies[theindex];
}

function loadUserName( username ) {
    myname = username;
}

$( document ).ready( function () {

    $( "body" ).keydown( function ( event ) {
        if ( event.which === 84 && !inputshowing && active ) {   // open chat-input
            event.preventDefault();
            enableChatInput( true );
        } else if ( event.which === globalsaykeycode && !inputshowing && active ) {    // open globalchat-input
            event.preventDefault();
            enableChatInput( true, "/globalsay " );
        } else if ( event.which === 13 && inputshowing ) {   // send message and close input
            event.preventDefault();
            let msg = maininput.val();
            if ( msg ) {
                if ( msg[0] === "/" ) {
                    msg = msg.substr( 1 );
                    if ( msg.length > 0 ) {
                        mp.invoke( "command", msg );
                    }
                } else {
                    mp.invoke( "chatMessage", msg + chatends[chosenchatbody] );
                }
            }

            enableChatInput( false );
        }
    } );

    chosentab = $( "#chat_choice div[data-chatID=0]" );
    chosentab.css( "background", "#04074e" );

    $( "#chat_choice div" ).click( function () {
        if ( chosenchatbody === $( this ).attr ( "data-chatID" ) )
            return;

        chosentab.css( "background", "#A9A9A9" );
        chatbodies[chosenchatbody].hide( 400 );

        chosentab = $( this );
        chosentab.css( "background", "#04074e" );
        chosenchatbody = chosentab.attr( "data-chatID" );
        chatbodies[chosenchatbody].show( 400 );
    } );

    mp.trigger( "onChatLoad" );
} );