"use strict";
let mappointsdata = {
    menu: null,
    peds: {},
    markers: {},
};
mappointsdata.menu.Visible = false;
API.onResourceStart.connect(function () {
    mappointsdata.menu = API.createMenu("Map-Creator", res.Width, res.Height * 0.5, 6);
    mappointsdata.menu.Visible = false;
});
