var img;

$( document ).ready( function () {
	img = $( "img:first" );
} );

function showBloodscreen() {
	img.stop();
	img.css( "opacity", 1.0 );
	img.animate( { "opacity": "0.0" }, 2000 );
}