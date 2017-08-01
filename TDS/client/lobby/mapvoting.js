"use strict";
let mapmenu = API.createMenu("Map-Vote", "Maps", 0, 0, 4);
let mapvotes = API.createMenu("Votes", "", 0, 0, 9, false);
mapmenu.ResetKey(menuControl.Back);
mapvotes.ResetKey(menuControl.Back);
mapvotes.ResetKey(menuControl.Select);
mapmenu.Visible = false;
mapvotes.Visible = false;
let clickmapmenuevent = null;
let cooldown = 0;
let mapvotings = {};
let mapvotingitems = [];
let showmapmenu = false;
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
API.onUpdate.connect(function () {
    if (showmapmenu)
        API.drawMenu(mapmenu);
    if (mapvotingitems[0] != undefined)
        API.drawMenu(mapvotes);
});
function mapMenuItemClick(sender, item, index) {
    API.triggerServerEvent("onMapVotingRequest", item.Text);
    mapMenuClose();
    API.showCursor(false);
}
function putMapSortedByAmountVotes(mapitem, amountvotes) {
    for (let i = mapvotingitems.length - 1; i >= 0; i--) {
        if (mapvotings[mapvotingitems[i].Description] < amountvotes) {
            mapvotingitems.splice(i, 0, mapitem);
        }
    }
}
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "onMapMenuOpen":
            for (let j = 0; j < 10; j++)
                for (let i = 0; i < args[0].Count; i++) {
                    let mapitem = API.createMenuItem(args[0][i], "placeholder");
                    mapmenu.AddItem(mapitem);
                }
            mapmenu.Visible = true;
            showmapmenu = true;
            clickmapmenuevent = mapmenu.OnItemSelect.connect(mapMenuItemClick);
            API.showCursor(true);
            break;
        case "onNewMapForVoting":
            let index = mapvotingitems.length;
            mapvotingitems[index] = API.createMenuItem(args[0] + " [0]", args[0]);
            mapvotings[args[0]] = 0;
            if (index == 0)
                mapvotes.Visible = true;
            break;
        case "onAddVoteToMap":
            mapvotings[args[0]]++;
            let mapitem = undefined;
            let oldmapitem = undefined;
            for (let i = mapvotingitems.length - 1; i >= 0; i--) {
                if (mapvotingitems[i].Description == args[0]) {
                    mapitem = mapvotingitems[i];
                    mapvotingitems.splice(i, 1);
                    if (oldmapitem != undefined || args[1] == undefined)
                        break;
                }
                else if (args[1] != undefined && mapvotingitems[i].Description == args[1]) {
                    oldmapitem = mapvotingitems[i];
                    mapvotingitems.splice(i, 1);
                    if (mapitem != undefined)
                        break;
                }
            }
            mapitem.Text = args[0] + " [" + mapvotes[args[0]] + "]";
            putMapSortedByAmountVotes(mapitem, mapvotings[args[0]]);
            if (oldmapitem != undefined) {
                mapvotings[args[1]]--;
                oldmapitem.Text = args[1] + " [" + mapvotes[args[1]] + "]";
                putMapSortedByAmountVotes(oldmapitem, mapvotings[args[1]]);
            }
            break;
        case "onClientRoundEnd":
            mapvotes.Clear();
            mapvotes.Visible = false;
            mapvotings = {};
            mapvotingitems = [];
            break;
    }
});
function mapMenuClose() {
    mapmenu.Visible = false;
    showmapmenu = false;
    if (clickmapmenuevent != null) {
        clickmapmenuevent.disconnect();
        clickmapmenuevent = null;
    }
}
