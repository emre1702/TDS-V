var isregistered = false;
var langdata = {};

$( document ).ready( function () {
	$( '.forgotpassword' ).hide();

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

	var lasttick = 0;

	$( '.tab a' ).on( 'click', function ( e ) {
		var tick = new Date().getTime();
		if ( tick >= lasttick + 1000 ) {
			target = $( this ).attr( 'href' );
			if ( !isregistered || target !== "#signup" ) {

				lasttick = tick;
				e.preventDefault();

				$( '.forgotpassword' ).hide();
				$( '.tab-content' ).fadeIn( 1000 );
				$( this ).parent().addClass( 'active' );
				$( this ).parent().siblings().removeClass( 'active' );



				$( '.tab-content > div' ).not( target ).hide();

				$( target ).fadeIn( 1000 );

			}

		}
	} );

	$( '.forgotpw' ).on( 'click', function ( e ) {
		var tick = new Date().getTime();
		if ( tick >= lasttick + 1000 ) {
			$( '.tab-content' ).hide();
			$( '.forgotpassword' ).fadeIn( 1000 );
			lasttick = 0;
		}
	} );

    $( "button:not( [type = 'submit'] )" ).click( function ( event ) {
        event.preventDefault();
        var type = $( this ).attr( "data-eventtype" );
        switch ( type ) {
            case "lang_english":
                mp.trigger( "setLanguage", "ENGLISH" );
                mp.trigger( "getRegisterLoginLanguage" );
				break;

            case "lang_german":
                mp.trigger( "setLanguage", "GERMAN" );
                mp.trigger( "getRegisterLoginLanguage" );
				break;
		}
	} );

	$( ".form" ).submit( function ( event ) {
		event.preventDefault();
		var $this = $( this );
		var button = $this.find( ':submit:not(:hidden)' );
        var type = button.attr( "data-eventtype" );
		switch ( type ) {

			case "login":
				var password = $this.find( "input[id=login_password]" ).val();
				mp.trigger( "loginFunc", password );
				break;

			case "register":
				$this.find( "input:not(:hidden)[id=register_password_again]" ).each( function () {
					var password = $this.find( "input[id=register_password]" ).val();
					if ( password === $( this ).val() ) {
						mp.trigger( "registerFunc", password, $this.find( "input[id=register_email]" ).val() );
					} else {
						alert( langdata["passwordhastobesame"] );
						event.preventDefault();
					}
				} );
				break;
		}
	} );
} );

function getLoginPanelData( playername, isreg, lang ) {
    mp.trigger( "outputCEF", "getLoginPanelData" );
	loadLanguage( lang );
	$( "[data-lang=username]" ).each( function () {
		$( this ).addClass( 'active highlight' );
		$( this ).next( "input" ).val( playername );
	} );

	$( "[data-disable]" ).each( function () {
		$( this ).prop( "disabled", true );
	} );

	isregistered = isreg == "1";
}

function loadLanguage( lang ) {
    mp.trigger( "outputCEF", "loadLanguage" ); 
	var langdata = JSON.parse( lang );
	$( "[data-lang]" ).each( function () {
		$( this ).html( langdata[$( this ).attr( "data-lang" )] + ( $( this ).next().attr( "required" ) ? "*" : "" ) );
		//$( this ).html( $( this ).attr( "data-lang" ) );
	} );
}
