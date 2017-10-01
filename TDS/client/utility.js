"use strict";
API.onKeyDown.connect(function (sender, e) {
    if (e.KeyCode == Keys.Delete) {
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
