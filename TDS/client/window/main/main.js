let moneyText;
let bloodscreen;
let killmessageBox;

function setMoney( money ) {
    moneyText.text( "$" + money );
}

function playSound( soundname ) {
    $( "#audio_" + soundname ).trigger("play").volume = 0.05;
}

function showBloodscreen() {
    bloodscreen.stop()
    bloodscreen.css( "opacity", 1.0 );
    bloodscreen.animate( { opacity: 0.0 }, 2500 );
}

function formatMsg( input ) {
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

function removeThis( element ) {
    element.remove();
}

function addKillMessage( msg ) {
    let child = $( "<text>" + formatMsg( msg ) + "<br></text>" );
    killmessagesBox.append( child );
    child.delay ( 11000 ).fadeOut( 4000, child.remove );
}

$( document ).ready( () => {
    moneyText = $( "#money" );
    bloodscreen = $( "#bloodscreen" );
    killmessagesBox = $( "#kill_messages_box" );
} );