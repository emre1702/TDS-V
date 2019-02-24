﻿namespace TDS_Server.Manager.Utility
{

    using GTANetworkAPI;
    using TDS_Common.Default;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Logs;

    class ChatManager : Script
    {
        public ChatManager() { }

        [RemoteEvent(DToServerEvent.LobbyChatMessage)]
        public static void SendLobbyMessage(TDSPlayer player, string message, bool isDirty)
        {
            //if (!character.MuteTime.HasValue)
            string changedmessage = player.Team.ChatColor + player.Client.Name + "!{220|220|220}: " + message;
            player.CurrentLobby.SendAllPlayerChatMessage(changedmessage);
            if (player.CurrentLobby.IsOfficial && !isDirty)
                ChatLogsManager.Log(message, player);
            //else if (character.IsPermamuted)
            //    player.SendNotification(character.Language.STILL_PERMAMUTED);
            //else
            //    player.SendNotification(Utils.GetReplaced(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public static void SendGlobalMessage(TDSPlayer character, string message)
        {
            string changedmessage = "[GLOBAL] " + character.Team.ChatColor + character.Client.Name + "!{220|220|220}: " + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            ChatLogsManager.Log(message, character, isglobal: true);   
        }

       public static void SendAdminMessage(TDSPlayer character, string message)
       {
            string changedmessage = character.AdminLevel.FontColor + "[" + character.AdminLevelName + "] !{255|255|255}" + character.Client.Name + ": !{220|220|220}" + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            ChatLogsManager.Log(message, character, isglobal: true, isadminchat: true);
       }

        public static void SendAdminChat(TDSPlayer character, string message)
        {
            string changedmessage = "[ADMINCHAT] " + character.AdminLevel.FontColor + character.Client.Name + ": !{220|220|220}" + message;
            AdminsManager.SendChatMessageToAdmins(changedmessage);
            ChatLogsManager.Log(message, character, isadminchat: true);
        }

        public static void SendTeamChat(TDSPlayer character, string message)
        {
            string changedmessage = "[TEAM] " + character.Team.ChatColor + character.Client.Name + ": !{220|220|220}" + message;
            character.CurrentLobby.SendAllPlayerChatMessage(changedmessage, character.Team);
            ChatLogsManager.Log(message, character, isteamchat: true);
        }

        public static void SendPrivateMessage(TDSPlayer character, TDSPlayer targetcharacter, string message)
        {
            string changedmessage = "[PM] !{253|132|85}" + character.Client.SocialClubName + ": !{220|220|220}" + message;
            NAPI.Chat.SendChatMessageToPlayer(targetcharacter.Client, changedmessage);
            ChatLogsManager.Log(message, character, target: targetcharacter);
        }

    }

}
