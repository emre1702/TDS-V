$(document).ready ( function() {
    
    $( '.form' ).find( 'input, textarea' ).on( 'keyup blur focus', function ( e ) {
		var $this = $( this ),
			label = $this.prev( 'label' );

		if ( e.type === 'keyup' ) {
			if ( $this.val() === '' ) {
				label.removeClass( 'active highlight' );
			} else {
				label.addClass( 'active highlight' );
			}
		} else if ( e.type === 'blur' ) {
			if ( $this.val() === '' ) {
				label.removeClass( 'active highlight' );
			} else {
				label.removeClass( 'highlight' );
			}
		} else if ( e.type === 'focus' ) {
			if ( $this.val() === '' ) {
				label.removeClass( 'highlight' );
			} else {
				label.addClass( 'highlight' );
			}
		}
	} );
    
    $( "button" ).click( function ( event ) {
        event.preventDefault();
        var clickedbutton = $( this ).attr( "data-eventtype" );
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
				$( "#custom_lobby" ).fadeIn( 2000 );
                break;
                
			case "lang_english":
				resourceCall( "changeLanguage", "english" );
				resourceCall( "getLobbyChoiceLanguage" );
                break;
            
			case "lang_german":
				resourceCall( "changeLanguage", "german" );
				resourceCall( "getLobbyChoiceLanguage" );
                break;
                
            case "custom_lobby_back":
                $( "#custom_lobby" ).hide();
                $( "#lobby_choice" ).show();
                break;
        }
    } );
    
    $( "form" ).submit( function ( event ) {
		event.preventDefault();
        
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