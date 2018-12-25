using Newtonsoft.Json;
using RAGE;
using RAGE.Ui;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    static class Choice
    {
        private static HtmlWindow browser;

        public static void JoinLobby(int index, int teamindex)
        {
            Events.CallRemote(DToServerEvent.JoinLobby, index, teamindex);
            //callRemoteCooldown("joinMapCreatorLobby");
        }

        public static void Start()
        {
            browser = new HtmlWindow("package://TDS-V/window/choice/index.html");
            Cursor.Visible = true;
        }

        public static void Stop()
        {
            if (browser == null)
                return;
            browser.Destroy();
            browser = null;
            Cursor.Visible = false;
        }

        public static void SyncLanguageTexts()
        {
            browser.ExecuteJs($"setLobbyChoiceLanguage(`{ JsonConvert.SerializeObject(Settings.Language.LOBBY_CHOICE_TEXTS) }`)");
        }
    }
}
