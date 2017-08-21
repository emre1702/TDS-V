"use strict";
let rounddata = {
    mapinfo: null,
    isspectator: true,
    inround: false
};
function setMapInfo(mapname) {
    rounddata.mapinfo = new cText(mapname, res.Width * 0.5, res.Height * 0.95, 0.5, 255, 255, 255, 255, 0, 2, true);
}
API.onUpdate.connect(function () {
    if (!rounddata.inround) {
        API.disableControlThisFrame(24);
        API.disableControlThisFrame(257);
    }
});
function removeMapInfo() {
    if (rounddata.mapinfo != null) {
        rounddata.mapinfo.remove();
        rounddata.mapinfo = null;
    }
}
function removeRoundThings(removemapinfo) {
    stopSpectate();
    stopMapLimitCheck();
    stopCountdown();
    if (removemapinfo) {
        removeMapInfo();
    }
}
