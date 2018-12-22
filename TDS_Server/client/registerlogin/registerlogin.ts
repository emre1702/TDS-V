/// <reference path="../types-ragemp/index.d.ts" />
/// <reference path="../enum/custombrowserremoteevents.ts" />
/// <reference path="../enum/customremoteevents.ts" />

let loginpanel = {
    loginbrowser: null as BrowserMp,
    name: null as string,
    isregistered: null
}


mp.events.add(ECustomBrowserRemoteEvents.LoginFunc, function (password) {
    callRemoteCooldown("onPlayerTryLogin", password);
});

mp.events.add(ECustomBrowserRemoteEvents.RegisterFunc, function (password, email) {
    callRemoteCooldown("onPlayerTryRegister", password, email);
});

mp.events.add(ECustomBrowserRemoteEvents.GetRegisterLoginLanguage, () => {
    loginpanel.loginbrowser.execute("loadLanguage ( `" + JSON.stringify(getLang("loginregister")) + "` );");
});

mp.events.add(ECustomRemoteEvents.StartRegisterLogin, function (name: string, isregistered) {
    log("startRegisterLogin registerlogin");
    loginpanel.name = name;
    loginpanel.isregistered = isregistered;
    loginpanel.loginbrowser = mp.browsers.new("package://TDS-V/window/registerlogin/index.html");
    mp.gui.chat.activate(false);
    toggleCursor(true);
});

mp.events.add(ECustomRemoteEvents.RegisterLoginSuccessful, function () {
    log("registerLoginSuccessful registerlogin");
    loginpanel.loginbrowser.destroy();
    loginpanel.loginbrowser = null;
    mp.gui.chat.activate(true);
    --nothidecursor;
});