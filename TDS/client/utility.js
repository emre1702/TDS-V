"use strict";
API.setPedCanRagdoll(true);
API.disableFingerPointing(true);
function vector3Lerp(start, end, fraction) {
    return new Vector3((start.X + (end.X - start.X) * fraction), (start.Y + (end.Y - start.Y) * fraction), (start.Z + (end.Z - start.Z) * fraction));
}
function clampAngle(angle) {
    return (angle + Math.ceil(-angle / 360) * 360);
}
function getPositionInFront(range, pos, zrot, plusangle) {
    var angle = clampAngle(zrot) * (Math.PI / 180);
    plusangle = (clampAngle(plusangle) * (Math.PI / 180));
    pos.X += (range * Math.sin(-angle - plusangle));
    pos.Y += (range * Math.cos(-angle - plusangle));
    return pos;
}
API.onKeyDown.connect(function (sender, e) {
    if (e.KeyCode == Keys.End) {
        if (API.isCursorShown()) {
            API.showCursor(false);
            nothidecursor = 0;
        }
        else {
            API.showCursor(true);
            nothidecursor = 1;
        }
    }
});
