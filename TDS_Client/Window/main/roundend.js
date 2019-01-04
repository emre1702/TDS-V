let roundEndReason = $("#round_end_reason");
let roundEndReasonContainer = $( "#round_end_reason_container" );
let myMapRating = {}
let currentMap;
let starRating = new SimpleStarRating( $( "#map_rating_stars" )[0], function ( rating ) {
    if ( !( currentMap in myMapRating ) || myMapRating[currentMap] !== rating ) {
        myMapRating[currentMap] = rating;
        mp.trigger( "sendMapRating", currentMap, rating );
        starRating.disable();
    }
} );

function showRoundEndReason( reason, mapname ) {
    roundEndReason.html( reason );
    roundEndReasonContainer.css( "display", "table" );
    currentMap = mapname;
    if ( mapname in myMapRating ) {
        starRating.setCurrentRating( myMapRating[mapname] );
    } else
        starRating.setCurrentRating( 0 );
    starRating.enable();    
}

function hideRoundEndReason() {
    roundEndReasonContainer.hide();
}

function loadMyMapRatings( str ) {
    myMapRating = JSON.parse( str );
}