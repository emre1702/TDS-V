"use strict";
let rounddata = {
    mapinfo: null,
    isspectator: true,
};
function setMapInfo(mapname) {
    rounddata.mapinfo = new cText(mapname, res.Width * 0.5, res.Height * 0.95, 0.5, 255, 255, 255, 255, 0, 2, true);
}
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
