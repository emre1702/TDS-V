"use strict";
let loginpanel = {
    loginbrowser: null,
    name: null,
    isregistered: null
};
function loginFunc(password) {
    API.triggerServerEvent("onPlayerTryLogin", password);
}
function registerFunc(password, email) {
    API.triggerServerEvent("onPlayerTryRegister", password, email);
}
function getLoginPanelData() {
    loginpanel.loginbrowser.call("getLoginPanelData", loginpanel.name, loginpanel.isregistered, JSON.stringify(languagelist[languagesetting].loginregister));
}
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "startRegisterLogin") {
        var res = API.getScreenResolution();
        loginpanel.name = args[0];
        loginpanel.isregistered = args[1];
        loginpanel.loginbrowser = API.createCefBrowser(res.Width, res.Height);
        API.waitUntilCefBrowserInit(loginpanel.loginbrowser);
        API.setCefBrowserPosition(loginpanel.loginbrowser, 0, 0);
        API.setCefBrowserHeadless(loginpanel.loginbrowser, false);
        API.loadPageCefBrowser(loginpanel.loginbrowser, "client/window/registerlogin/registerlogin.html");
        API.setCanOpenChat(false);
        API.setHudVisible(false);
        API.showCursor(true);
    }
    else if (eventName == "registerLoginSuccessful") {
        API.destroyCefBrowser(loginpanel.loginbrowser);
        API.setCanOpenChat(true);
    }
});
