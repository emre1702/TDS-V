"use strict";
let loginpanel = {
    loginbrowser: null,
    name: null,
    isregistered: null
};
mp.events.add("loginFunc", function (password) {
    mp.events.callRemote("onPlayerTryLogin", password);
});
mp.events.add("registerFunc", function (password, email) {
    mp.events.callRemote("onPlayerTryRegister", password, email);
});
mp.events.add("getRegisterLoginLanguage", () => {
    loginpanel.loginbrowser.execute("loadLanguage ( " + JSON.stringify(getLang("loginregister")) + ");");
});
mp.events.add("startRegisterLogin", function (name, isregistered) {
    log("startRegisterLogin registerlogin start");
    loginpanel.name = name;
    loginpanel.isregistered = isregistered;
    loginpanel.loginbrowser = mp.browsers.new("package://TDS-V/window/registerlogin/registerlogin.html");
    mp.events.add('browserDomReady', (browser) => {
        if (browser == loginpanel.loginbrowser) {
            mp.gui.chat.push("Debug: startRegisterLogin browserDomReady");
            browser.execute("getLoginPanelData ( " + loginpanel.name + ", " + loginpanel.isregistered + ", " + JSON.stringify(getLang("loginregister")) + ");");
        }
    });
    mp.gui.chat.activate(false);
    mp.game.ui.displayHud(false);
    mp.gui.cursor.visible = true;
    nothidecursor++;
    log("startRegisterLogin registerlogin end");
});
mp.events.add("registerLoginSuccessful", function () {
    log("registerLoginSuccessful registerlogin start");
    loginpanel.loginbrowser.destroy();
    loginpanel.loginbrowser = null;
    mp.gui.chat.activate(true);
    nothidecursor--;
    log("registerLoginSuccessful registerlogin end");
});
mp.events.callRemote("onPlayerJoin");
