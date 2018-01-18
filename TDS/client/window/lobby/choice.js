var createlobby = "<div class='text-field'>	\
                        <label for='lobby_name' data-lang='lobby_name'>lobby-name*</label>	\
                        <input type='text' id='lobby_name' required minlength='3' maxlength='20' />	\
                    </div>	\
                    <div class='select-field'>	\
                        <select class='select-field-select' required id='lobby_mode' size='1'>	\
                            <option selected data-lang='arena'>arena</option>	\
                            <option data-lang='bomb'>bomb</option>	\
                        </select>	\
                        <label class='select-field-label' data-lang='mode'>mode*</label>	\
                    </div>	\
                     <div class='text-field'>	\
                        <label for='lobby_password' data-lang='lobby_password'>lobby-password</label>	\
                        <input type='text' id='lobby_password' required minlength='3' maxlength='20' />	\
                    </div>	\
                    <div class='number-field'>	\
                        <label for='max_players' data-lang='max_players'>max-players*</label>	\
                        <input type='number' id='max_players' required min='1' max='1000' value='100' step='1' />	\
                    </div> 	\
                     <div class='number-field'>	\
                        <label for='round_time' data-lang='round_time'>round-time (seconds)*</label>	\
                        <input type='number' id='round_time' required min='30' max='99999999' value='240' step='1' />	\
                    </div>   	\
                     <div class='number-field'>	\
                        <label for='countdown_time' data-lang='countdown_time'>countdown-time (seconds)*</label>	\
                        <input type='number' id='countdown_time'required min='0' max='99999' value='3' step='1' />	\
                    </div>  	\
                     	\
                    <div class='number-field'>	\
                        <label for='armor' data-lang='armor'>armor*</label>	\
                        <input type='number' id='armor' required min='0' max='100' value='100' step='1' />	\
                    </div> 	\
                     <div class='number-field'>	\
                        <label for='health' data-lang='health'>health*</label>	\
                        <input type='number' id='health' required min='1' max='100' value='100' step='1' />	\
                    </div> 	\
                    <div class='number-field'>	\
                        <label for='time-scale' data-lang='time-scale'>time-scale*</label>	\
                        <input type='number' id='time-scale' required min='0' max='1' value='1.0' step='0.1' />	\
                    </div>";

$(document).ready ( function() {
    
    function hightlightOnFocus ( e ) {
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
    }
    
    $( '.form' ).find( 'input, textarea' ).on( 'keyup blur focus', hightlightOnFocus );
    
    function addHighlight ( ) {
        var $this = $( this ),
			label = $this.prev( 'label' );

         if ( $this.val() === '' ) {
            label.removeClass( 'active highlight' );
         } else {
             label.addClass( 'active highlight' );
         }
    }
    $( '.form' ).find( 'input, textarea' ).each( addHighlight );
     
    
    $( "button:not([type='submit'])" ).click( function ( event ) {
        event.preventDefault();
        var clickedbutton = $( this ).attr( "data-eventtype" );
		switch ( clickedbutton ) {

			case "join_arena":
				$( "#lobby_choice" ).hide();
                $( "#team_choice" ).show();
                break;

			case "join_arena_player":
				mp.trigger( "joinArena", false );
                break;

            case "join_arena_spectator":
				mp.trigger( "joinArena", true );
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
				mp.trigger( "setLanguage", "ENGLISH" );
				mp.trigger( "getLobbyChoiceLanguage" );
                break;
            
			case "lang_german":
				mp.trigger( "setLanguage", "GERMAN" );
				mp.trigger( "getLobbyChoiceLanguage" );
                break;
                
            case "custom_lobby_back":
                $( "#custom_lobby" ).hide();
                $( "#lobby_choice" ).show();
                break;
                
            case "custom_lobby_own":
                $( "#custom_lobby_setting_form" ).empty();
                $( "#custom_lobby_setting_form" ).append ( createlobby );
                $( '.form' ).find( 'input, textarea' ).on( 'keyup blur focus', hightlightOnFocus );
                $( '.form' ).find( 'input, textarea' ).each( addHighlight );
                break;
        }
    } );
    
	$( "form" ).submit( function ( e ) {
		e.preventDefault();
		var name = $( '#lobby_name' ).val();
		var mode = $( '#lobby_mode :selected' ).text();
		var password = $( '#lobby_password' ).val();
		var roundtime = $( '#round_time' ).val();
		var countdowntime = $( '#countdown_time' ).val();
		var maxplayers = $( '#max_players' ).val();
		var armor = $( '#armor' ).val();
		var health = $( '#health' ).val();
		var timescale = $( '#time-scale' ).val();
		mp.trigger( "createLobby", name, mode, password, roundtime, countdowntime, maxplayers, armor, health, timescale );
		
	} );
    
    $( "form" ).validate ({
        errorPlacement: function(error, element) {
            $('#validate-error').html ( "<div class='validate-error'>"+error.text()+"</div>" );
        }
    });
} );

function getLobbyChoiceLanguage( lang ) {
	var langdata = JSON.parse( lang );
	$( "[data-lang]" ).each( function () {
		$( this ).html( langdata[$( this ).attr( "data-lang" )] + ( $( this ).prop("required" ) ? "*" : "" ) );
		//$( this ).html( $( this ).attr( "data-lang" ) );
	} );
}