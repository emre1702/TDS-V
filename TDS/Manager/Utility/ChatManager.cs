namespace TDS.Manager.Utility
{

    using GTANetworkAPI;
    using TDS.Instance.Player;
    using TDS.Manager.Logs;
    using TDS.Manager.Player;

    class ChatManager : Script
    {
#warning TODO: Add dirty chat

        public ChatManager() { }

        private static void OnLobbyChatMessageFunc(TDSPlayer character, string message)
        {
            string changedmessage = character.TeamChatColor + character.Client.Name + "{220|220|220}: " + message;
            character.CurrentLobby.SendAllPlayerChatMessage(changedmessage);
            if (character.CurrentLobby.IsOfficial)
                Chat.Log(message, character); 
        }

        // [DisableDefaultChat] 
        [ServerEvent(Event.ChatMessage)]
        public static void OnLobbyChatMessage(Client player, string message)
        {
            TDSPlayer character = player.GetChar();
            if (character.Entity != null)
                if (!character.MuteTime.HasValue)
                    OnLobbyChatMessageFunc(character, message);
                else if (character.IsPermamuted)
                    player.SendNotification(character.Language.STILL_PERMAMUTED);
                else
                    player.SendNotification(Utils.GetReplaced(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public static void SendGlobalMessage(TDSPlayer character, string message)
        {
            string changedmessage = "[GLOBAL] " + character.TeamChatColor + character.Client.Name + "{220|220|220}: " + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            Chat.Log(message, character, isglobal: true);   
        }

       public static void SendAdminMessage(TDSPlayer character, string message)
       {
            string changedmessage = character.AdminLevel.FontColor + "[" + character.AdminLevelName + "] {255|255|255}" + character.Client.Name + ": {220|220|220}" + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            Chat.Log(message, character, isglobal: true, isadminchat: true);
       }

        public static void SendAdminChat(TDSPlayer character, string message)
        {
            string changedmessage = "[ADMINCHAT] " + character.AdminLevel.FontColor + character.Client.Name + ": {220|220|220}" + message;
            AdminsManager.SendChatMessageToAdmins(changedmessage);
            Chat.Log(message, character, isadminchat: true);
        }

        public static void SendTeamChat(TDSPlayer character, string message)
        {
            string changedmessage = "[TEAM] " + character.TeamChatColor + character.Client.Name + ": {220|220|220}" + message;
            character.CurrentLobby.SendAllPlayerChatMessage(changedmessage, character.Team.Index);
            Chat.Log(message, character, isteamchat: true);
        }

        public static void SendPrivateMessage(TDSPlayer character, TDSPlayer targetcharacter, string message)
        {
            string changedmessage = "[PM] {253|132|85}" + character.Client.SocialClubName + ": {220|220|220}" + message;
            NAPI.Chat.SendChatMessageToPlayer(targetcharacter.Client, changedmessage);
            Chat.Log(message, character, target: targetcharacter);
        }

    }

}
