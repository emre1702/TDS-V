"use strict";
function Vector3Lerp(start, end, fraction) {
    return new Vector3((start.X + (end.X - start.X) * fraction), (start.Y + (end.Y - start.Y) * fraction), (start.Z + (end.Z - start.Z) * fraction));
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
API.setPedCanRagdoll(true);
API.disableFingerPointing(true);
