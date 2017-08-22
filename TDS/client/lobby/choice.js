"use strict";
let lobbychoicedata = {
    browser: null
};
function joinArena(isspectator) {
    API.triggerServerEvent("joinLobby", 1, isspectator);
}
function getLobbyChoiceLanguage() {
    log("getLobbyChoiceLanguage start");
    lobbychoicedata.browser.call("getLobbyChoiceLanguage", JSON.stringify(languagelist[languagesetting].lobby_choice));
    log("getLobbyChoiceLanguage end");
}
function createLobby() {
}
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "onClientJoinMainMenu":
            log("onClientJoinMainMenu start");
            lobbychoicedata.browser = API.createCefBrowser(res.Width, res.Height);
            API.waitUntilCefBrowserInit(lobbychoicedata.browser);
            API.setCefBrowserPosition(lobbychoicedata.browser, 0, 0);
            API.setCefBrowserHeadless(lobbychoicedata.browser, false);
            API.loadPageCefBrowser(lobbychoicedata.browser, "client/window/lobby/choice.html");
            API.setHudVisible(false);
            API.showCursor(true);
            nothidecursor++;
            log("onClientJoinMainMenu end");
            break;
        case "onClientPlayerJoinLobby":
            log("onClientPlayerJoinLobby start");
            API.destroyCefBrowser(lobbychoicedata.browser);
            API.setHudVisible(true);
            nothidecursor--;
            if (nothidecursor == 0)
                API.showCursor(false);
            log("onClientPlayerJoinLobby end");
            break;
        case "onClientPlayerJoinRoundlessLobby":
            log("onClientPlayerJoinRoundlessLobby start");
            API.destroyCefBrowser(lobbychoicedata.browser);
            API.setHudVisible(true);
            nothidecursor--;
            if (nothidecursor == 0)
                API.showCursor(false);
            log("onClientPlayerJoinRoundlessLobby end");
            break;
    }
});
