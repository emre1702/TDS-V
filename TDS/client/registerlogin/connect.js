"use strict";
let ConnectFunc = function () {
    API.onResourceStart.connect(function () {
        API.triggerServerEvent("onPlayerJoin");
    });
};
ConnectFunc();
