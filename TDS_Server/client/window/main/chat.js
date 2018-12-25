let chatdata = {
    amountentries: [0, 0],
    maxentries: 40,
    active: true,
    inputshowing: false,
    maininput: $( "#main-input" ),
    bodies: [
        $( "#normal-chat-body" ),
        $( "#dirty-chat-body" )
    ],
    chatends: ["$normal$", "$dirty$"],
    chosentab: null,
    chosenchatbody: 0,
    myname: null,
    globalsaykeycode: String.fromCharCode( 90 ) === "Z" ? 90 : Y,
    playernames: [],
    autocompleteon: false
};


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
    chatbody.finish().animate( { scrollTop: chatbody.prop( "scrollHeight" ) }, "slow" );
}

function enableChatInput( enable, cmd = "" ) {
    if ( !chatdata.active && enable )
        return;

    if ( enable !== chatdata.inputshowing ) {
        mp.invoke( "focus", enable );

        if ( enable ) {
            chatdata.maininput.fadeIn();
            chatdata.maininput.val( cmd );
            setTimeout( () => {
                chatdata.maininput.focus();
            }, 100 );
        } else {
            chatdata.maininput.hide();
            chatdata.maininput.val( "" );
        }

        chatdata.inputshowing = enable;
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
    chatdata.active = toggle;
};

chatAPI["show"] = ( toggle ) => {
    if ( toggle ) {
        getChatBody().show();
        $( "#chat_choice" ).show();
    } else {
        getChatBody().hide();
        $( "#chat_choice" ).hide();
    }

    if ( !toggle && chatdata.inputshowing )
        enableChatInput( false );

    chatdata.active = toggle;
};

function formatMsg( input, ismentioned ) {
    let start = ""; 
    if ( ismentioned )
        start = '<span style="background-color: rgba(255,178,102,0.6);">';

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
    if ( chatdata.myname === null )
        return false;
    let firstindex = msg.indexOf( "@" );
    if ( firstindex === -1 )
        return false;
    let lastindex = msg.indexOf( ":", firstindex + 1 );
    if ( lastindex === -1 )
        return false;
    let name = msg.substring( firstindex + 1, lastindex );
    if ( name === chatdata.myname )
        return true;
    return false;
}

function addChildToChatBody( child, chatbody, index ) {
    chatbody.append( child );
    if ( ++chatdata.amountentries[index] >= chatdata.maxentries ) {
        --chatdata.amountentries[index];
        chatbody.find( "text:first" ).remove();
    }
    updateScroll ( chatbody );
}

function addMessage( msg ) {
    //if ( !msg.startsWith( "RAGE MP" ) )
    //    alert( msg );
    let ismentioned = isMentioned( msg );

    // output in the chatbody when ending with one of chatends //
    for ( let i = 0; i < chatdata.chatends.length; ++i ) {
        if ( msg.endsWith( chatdata.chatends[i] ) ) {
            msg = msg.slice( 0, -chatdata.chatends[i].length );
            let chatbody = getChatBody( i );
            let formattedmsg = formatMsg( msg, ismentioned );
            let child = $( "<text>" + formattedmsg + "</text>" );
            addChildToChatBody( child, chatbody, i );
            return;
        }
    }

    let formattedmsg = formatMsg( msg, ismentioned );
    // else output in all chatbodies //
    for ( let i = 0; i < chatdata.bodies.length; ++i ) {
        let chatbody = getChatBody( i );
        let child = $( "<text>" + formattedmsg + "</text>" );
        addChildToChatBody( child, chatbody, i );
    }

}

function getChatBody( index = -1 ) {
    let theindex = index === -1 ? chatdata.chosenchatbody : index;
    return chatdata.bodies[theindex];
}

function loadUserName( username ) {
    chatdata.myname = username;
    chatdata.playernames.push( username );
}

function loadNamesForChat( names ) {
    chatdata.playernames = JSON.parse( names );
}

function addNameForChat( name ) {
    chatdata.playernames.push( username );
}

function removeNameForChat(name) {
    let index = chatdata.playernames.indexOf(name);
    if (index != -1)
        chatdata.playernames.splice( index, 1 );
}

$( document ).ready( function () {

    addAutocomplete( chatdata.maininput, chatdata.playernames, () => { chatdata.autocompleteon = true; return false; }, () => {
        setTimeout( function () { chatdata.autocompleteon = false; }, 500 ); } );

    $( "body" ).keydown( function ( event ) {
        if ( event.which === 84 && !chatdata.inputshowing && chatdata.active ) {   // open chat-input
            event.preventDefault();
            enableChatInput( true );
        } else if ( event.which === chatdata.globalsaykeycode && !chatdata.inputshowing && chatdata.active ) {    // open globalchat-input
            event.preventDefault();
            enableChatInput( true, "/globalsay " );
        } else if ( event.which === 13 && chatdata.inputshowing && !chatdata.autocompleteon ) {   // send message and close input
            event.preventDefault();
            let msg = chatdata.maininput.val();
            if ( msg ) {
                msg = msg.replace( /\\/g, "\\\\" ).replace( /\"/g, "\\\"" );
                if ( msg[0] === "/" ) {
                    msg = msg.substr( 1 );
                    if (msg.length > 0) {
                        mp.trigger("customCommand", msg);
                    }
                } else {
                    mp.invoke( "chatMessage", msg + chatdata.chatends[chatdata.chosenchatbody] );
                }
            }

            enableChatInput( false );
        }
    } );

    chatdata.chosentab = $( "#chat_choice div[data-chatID=0]" );
    chatdata.chosentab.css( "background", "#04074e" );

    $( "#chat_choice div" ).click( function () {
        if ( chatdata.chosenchatbody === $( this ).attr ( "data-chatID" ) )
            return;

        chatdata.chosentab.css( "background", "#A9A9A9" );
        chatdata.bodies[chatdata.chosenchatbody].hide( 400 );

        chatdata.chosentab = $( this );
        chatdata.chosentab.css( "background", "#04074e" );
        chatdata.chosenchatbody = chatdata.chosentab.attr( "data-chatID" );
        chatdata.bodies[chatdata.chosenchatbody].show( 400 );
    } );

    mp.trigger( "onChatLoad" );
} );