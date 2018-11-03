namespace TDS.Manager.Utility
{

    using GTANetworkAPI;
    using TDS.Instance.Player;
    using TDS.Manager.Logs;
    using TDS.Manager.Player;

    class ChatManager : Script
    {
        // TODO: Add dirty chat

        public ChatManager() { }

        private static void OnLobbyChatMessageFunc(Character character, string message)
        {
            string changedmessage = character.TeamChatColor + character.Player.Name + "{220|220|220}: " + message;
            character.CurrentLobby.SendAllPlayerChatMessage(changedmessage);
            if (character.CurrentLobby.IsOfficial)
                Chat.Log(message, character); 
        }

        // [DisableDefaultChat] 
        [ServerEvent(Event.ChatMessage)]
        public static void OnLobbyChatMessage(Client player, string message)
        {
            Character character = player.GetChar();
            if (character.Entity != null)
                if (!character.MuteTime.HasValue)
                    OnLobbyChatMessageFunc(character, message);
                else if (character.IsPermamuted)
                    player.SendNotification(character.Language.STILL_PERMAMUTED);
                else
                    player.SendNotification(Utils.GetReplaced(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public static void SendGlobalMessage(Character character, string message)
        {
            string changedmessage = "[GLOBAL] " + character.TeamChatColor + character.Player.Name + "{220|220|220}: " + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            Chat.Log(message, character, isglobal: true);   
        }

       public static void SendAdminMessage(Character character, string message)
       {
            string changedmessage = character.AdminLevel.FontColor + "[" + character.AdminLevelName + "] {255|255|255}" + character.Player.Name + ": {220|220|220}" + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            Chat.Log(message, character, isglobal: true, isadminchat: true);
       }

        public static void SendAdminChat(Character character, string message)
        {
            string changedmessage = "[ADMINCHAT] " + character.AdminLevel.FontColor + character.Player.Name + ": {220|220|220}" + message;
            AdminsManager.SendChatMessageToAdmins(changedmessage);
            Chat.Log(message, character, isadminchat: true);
        }

        public static void SendTeamChat(Character character, string message)
        {
            string changedmessage = "[TEAM] " + character.TeamChatColor + character.Player.Name + ": {220|220|220}" + message;
            character.CurrentLobby.SendAllPlayerChatMessage(changedmessage, character.Team.Id);
            Chat.Log(message, character, isteamchat: true);
        }

        public static void SendPrivateMessage(Character character, Character targetcharacter, string message)
        {
            string changedmessage = "[PM] {253|132|85}" + character.Player.SocialClubName + ": {220|220|220}" + message;
            NAPI.Chat.SendChatMessageToPlayer(targetcharacter.Player, changedmessage);
            Chat.Log(message, character, target: targetcharacter);
        }

    }

}
