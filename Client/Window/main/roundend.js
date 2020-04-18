let roundEndReason = $("#round_end_reason");
let roundEndReasonContainer = $("#round_end_reason_container");
let myMapRating = {}
let currentMapId;
let starRating = new SimpleStarRating($("#map_rating_stars")[0], function (rating) {
    if (!(currentMapId in myMapRating) || myMapRating[currentMapId] !== rating) {
        myMapRating[currentMapId] = rating;
        mp.trigger("b34", currentMapId, rating);	// SendMapRating_Browser
        // starRating.disable();
    }
});

function showRoundEndReason(reason, mapId) {
    roundEndReason.html(reason);
    roundEndReasonContainer.css("display", "table");
    currentMapId = mapId;
    if (mapId in myMapRating) {
        starRating.setCurrentRating(myMapRating[mapId]);
    } else
        starRating.setCurrentRating(0);
    starRating.enable();
}

function hideRoundEndReason() {
    roundEndReasonContainer.hide();
}

function loadMyMapRatings(str) {
    myMapRating = JSON.parse(str);
}
