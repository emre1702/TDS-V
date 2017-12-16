"use strict";
mp.events.add("onClientMoneyChange", money => {
    log("onClientMoneyChange start");
    currentmoney = money;
    log("onClientMoneyChange end");
});
mp.events.add("render", () => {
    if (currentmoney != null) {
        mp.game.graphics.drawText("$" + currentmoney, 7, [115, 186, 131, 255], 1.0, 1.0, true, res.x - 90, 50);
    }
});
