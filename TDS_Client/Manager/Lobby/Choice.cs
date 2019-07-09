using Newtonsoft.Json;
using RAGE.Ui;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class Choice
    {
        private static HtmlWindow Browser;

        public static void JoinLobby(int index, int teamindex)
        {
            EventsSender.Send(DToServerEvent.JoinLobby, index, teamindex);
            //EventsSender.SendCooldown("joinMapCreatorLobby");
        }

        public static void JoinArena(bool spectator)
        {
            EventsSender.Send(DToServerEvent.JoinArena, spectator);
        }

        public static void JoinMapCreator()
        {
            EventsSender.Send(DToServerEvent.JoinMapCreator);
        }

        public static void SyncLanguageTexts()
        {
            Browser?.ExecuteJs($"setLobbyChoiceLanguage(`{ JsonConvert.SerializeObject(Settings.Language.LOBBY_CHOICE_TEXTS) }`)");
        }
    }
}