"use strict";
var bloodscreenbrowser;
function Vector3Lerp(start, end, fraction) {
    return new Vector3((start.X + (end.X - start.X) * fraction), (start.Y + (end.Y - start.Y) * fraction), (start.Z + (end.Z - start.Z) * fraction));
}
API.onLocalPlayerShoot.connect(function (weaponUsed, aimCoords) {
    var frompos = API.getEntityPosition(API.getLocalPlayer());
    var dir = aimCoords.Subtract(frompos);
    var distance = dir.Length();
    dir.Normalize();
    dir.X *= distance * 1.05;
    dir.Y *= distance * 1.05;
    dir.Z *= distance * 1.05;
    var topos = frompos.Add(dir);
    var raycast = API.createRaycast(frompos, topos, 8, null);
    if (raycast.didHitEntity) {
        var hitentityhandle = raycast.hitEntity;
        var hithead = false;
        API.triggerServerEvent("onPlayerHitOtherPlayer", hitentityhandle, weaponUsed, hithead);
    }
});
API.onResourceStart.connect(function () {
    API.callNative("NETWORK_SET_FRIENDLY_FIRE_OPTION", false);
});
