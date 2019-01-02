using Newtonsoft.Json;
using RAGE;
using RAGE.Ui;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    static class Choice
    {
        public static HtmlWindow Browser;

        public static void JoinLobby(int index, int teamindex)
        {
            Events.CallRemote(DToServerEvent.JoinLobby, index, teamindex);
            //callRemoteCooldown("joinMapCreatorLobby");
        }

        public static void Start()
        {
            Browser = new HtmlWindow(Constants.LobbyChoiceBrowserPath);
            CursorManager.Visible = true;
        }

        public static void Stop()
        {
            if (Browser == null)
                return;
            Browser.Destroy();
            Browser = null;
            CursorManager.Visible = false;
        }

        public static void SyncLanguageTexts()
        {
            Browser.ExecuteJs($"setLobbyChoiceLanguage(`{ JsonConvert.SerializeObject(Settings.Language.LOBBY_CHOICE_TEXTS) }`)");
        }
    }
}
