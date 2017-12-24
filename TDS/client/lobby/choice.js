"use strict";
let lobbychoicedata = {
    browser: null
};
mp.events.add("joinArena", function (isspectator) {
    mp.events.callRemote("joinLobby", 1, isspectator);
});
mp.events.add("getLobbyChoiceLanguage", function () {
    log("getLobbyChoiceLanguage start");
    lobbychoicedata.browser.execute("getLobbyChoiceLanguage (" + JSON.stringify(getLang("lobby_choice")) + ")");
    log("getLobbyChoiceLanguage end");
});
mp.events.add("createLobby", function () {
});
mp.events.add("onClientJoinMainMenu", (args) => {
    log("onClientJoinMainMenu start");
    lobbychoicedata.browser = mp.browsers.new("client/window/lobby/choice.html");
    mp.events.add('browserDomReady', (browser) => {
        if (browser == lobbychoicedata.browser) {
            lobbychoicedata.browser.execute("getLobbyChoiceLanguage (" + JSON.stringify(getLang("lobby_choice")) + ")");
        }
    });
    mp.gui.cursor.visible = true;
    nothidecursor++;
    log("onClientJoinMainMenu end");
});
function destroyLobbyChoiceBrowser() {
    lobbychoicedata.browser.destroy();
    nothidecursor--;
    if (nothidecursor == 0)
        mp.gui.cursor.visible = false;
}
mp.events.add("onClientPlayerJoinLobby", (args) => {
    log("onClientPlayerJoinLobby start");
    destroyLobbyChoiceBrowser();
    log("onClientPlayerJoinLobby end");
});
mp.events.add("onClientPlayerJoinRoundlessLobby", (args) => {
    log("onClientPlayerJoinRoundlessLobby start");
    destroyLobbyChoiceBrowser();
    log("onClientPlayerJoinRoundlessLobby end");
});
