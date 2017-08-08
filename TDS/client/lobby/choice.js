"use strict";
let lobbychoicedata = {
    browser: null
};
function joinArena(isspectator) {
    API.triggerServerEvent("joinLobby", 1, isspectator);
}
function getLobbyChoiceLanguage() {
    lobbychoicedata.browser.call("getLobbyChoiceLanguage", JSON.stringify(languagelist[languagesetting].lobby_choice));
}
function createLobby() {
}
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "onClientJoinMainMenu":
            lobbychoicedata.browser = API.createCefBrowser(res.Width, res.Height);
            API.waitUntilCefBrowserInit(lobbychoicedata.browser);
            API.setCefBrowserPosition(lobbychoicedata.browser, 0, 0);
            API.setCefBrowserHeadless(lobbychoicedata.browser, false);
            API.loadPageCefBrowser(lobbychoicedata.browser, "client/window/lobby/choice.html");
            API.setHudVisible(false);
            API.showCursor(true);
            nothidecursor++;
            break;
        case "onClientPlayerJoinLobby":
            API.destroyCefBrowser(lobbychoicedata.browser);
            API.setHudVisible(true);
            nothidecursor--;
            if (nothidecursor == 0)
                API.showCursor(false);
            break;
    }
});
