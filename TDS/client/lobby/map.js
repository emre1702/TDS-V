"use strict";
let mapmenu = API.createMenu("Map-Vote", "Maps", res.Width / 2, res.Height / 2, 4);
mapmenu.ResetKey(menuControl.Back);
let drawmapmenuevent = null;
let clickmapmenuevent = null;
let cooldown = 0;
API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode == Keys.M) {
        if (!mapmenu.Visible) {
            let tick = API.getGlobalTime();
            if (cooldown <= tick) {
                cooldown = tick + 3000;
                API.triggerServerEvent("onMapMenuOpen");
            }
        }
        else {
            mapMenuClose();
            API.showCursor(false);
        }
    }
});
function mapMenuDraw() {
    API.drawMenu(mapmenu);
}
function mapMenuItemClick(sender, item, index) {
    API.triggerServerEvent("onMapVotingRequest", item.Text);
}
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "onMapMenuOpen") {
        for (let i = 0; i < args[0].Count; i++) {
            var mapitem = API.createMenuItem(args[0][i], "placeholder");
            mapmenu.AddItem(mapitem);
        }
        mapmenu.Visible = true;
        drawmapmenuevent = API.onUpdate.connect(mapMenuDraw);
        clickmapmenuevent = mapmenu.OnItemSelect.connect(mapMenuItemClick);
        API.showCursor(true);
    }
});
function mapMenuClose() {
    mapmenu.Visible = false;
    if (drawmapmenuevent != null) {
        drawmapmenuevent.disconnect();
        clickmapmenuevent.disconnect();
        drawmapmenuevent = null;
        clickmapmenuevent = null;
    }
}
