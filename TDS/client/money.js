"use strict";
API.onServerEventTrigger.connect(function (name, args) {
    if (name === "onClientMoneyChange") {
        log("onClientMoneyChange start");
        currentmoney = args[0];
        log("onClientMoneyChange end");
    }
});
API.onUpdate.connect(function () {
    if (currentmoney != null) {
        API.drawText("$" + currentmoney, res.Width - 15, 50, 1, 115, 186, 131, 255, 4, 2, true, true, 0);
    }
});
