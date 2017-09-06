"use strict";
API.onKeyDown.connect(function (sender, e) {
    if (e.KeyCode == Keys.Menu) {
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
