"use strict";
var bloodscreenbrowser;
function Vector3Lerp(start, end, fraction) {
    return new Vector3((start.X + (end.X - start.X) * fraction), (start.Y + (end.Y - start.Y) * fraction), (start.Z + (end.Z - start.Z) * fraction));
}
API.onLocalPlayerShoot.connect(function (weaponUsed, aimCoords) {
    var pos = API.getEntityPosition(API.getLocalPlayer());
    var endpos = Vector3Lerp(pos, aimCoords, 1.1);
    var raycast = API.createRaycast(pos, endpos, 8, null);
    if (raycast.didHitEntity) {
        var hitentityhandle = raycast.hitEntity;
        var hithead = false;
        var neckpos = API.returnNative("GET_PED_BONE_COORDS", 5, hitentityhandle, 39317);
        if (aimCoords.Z > neckpos.Z) {
            hithead = true;
        }
        API.triggerServerEvent("onPlayerHitOtherPlayer", hitentityhandle, weaponUsed, hithead);
    }
});
API.onResourceStart.connect(function () {
    API.callNative("NETWORK_SET_FRIENDLY_FIRE_OPTION", false);
});
