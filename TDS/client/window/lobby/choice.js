var clickedbutton;

$(document).ready ( function() {
    $( "button" ).click( function ( event ) {
        clickedbutton = $( this ).attr( "data-eventtype" );
    } );

    $( "form" ).submit( function ( event ) {
        event.preventDefault();
		switch ( clickedbutton ) {

			case "join_arena":
				$( "#lobby_choice" ).hide();
                $( "#team_choice" ).show();
                break;

			case "join_arena_player":
                resourceCall( "joinArena", false );
                break;

            case "join_arena_spectator":
                resourceCall( "joinArena", true );
                break;
                
            case "join_arena_back":
                $( "#team_choice" ).hide();
                $( "#lobby_choice" ).show();
                break;

            case "join_gang":
                // deactivated //
                break;

            case "custom_lobby":
				$( "#lobby_choice" ).hide();
				$( "#custom_lobby" ).show();
                break;
                
			case "lang_english":
				resourceCall( "changeLanguage", "english" );
				resourceCall( "getLobbyChoiceLanguage" );
                break;
            
			case "lang_german":
				resourceCall( "changeLanguage", "german" );
				resourceCall( "getLobbyChoiceLanguage" );
                break;
        }
	} );
    
	resourceCall( "getLobbyChoiceLanguage" );
} );

function getLobbyChoiceLanguage( lang ) {
	var langdata = JSON.parse( lang );
	$( "[data-lang]" ).each( function () {
		$( this ).html( langdata[$( this ).attr( "data-lang" )] );
		//$( this ).html( $( this ).attr( "data-lang" ) );
	} );
}