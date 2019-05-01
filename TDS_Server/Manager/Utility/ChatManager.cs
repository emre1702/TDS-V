namespace TDS_Server.Manager.Utility
{
    using GTANetworkAPI;
    using TDS_Common.Default;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Logs;
    using TDS_Server.Manager.Player;

    internal class ChatManager : Script
    {
        public ChatManager()
        {
        }

        [RemoteEvent(DToServerEvent.LobbyChatMessage)]
        public static void SendLobbyMessage(Client client, string message, bool isDirty)
        {
            TDSPlayer player = client.GetChar();
            if (player.IsPermamuted)
                player.Client.SendNotification(player.Language.STILL_PERMAMUTED);
            else if (player.IsMuted)
                player.Client.SendNotification(player.Language.STILL_MUTED.Replace("{0}", player.MuteTime?.ToString() ?? "?"));
            else
                SendLobbyMessage(player, message, isDirty);
        }

        public static void SendLobbyMessage(TDSPlayer player, string message, bool isDirty)
        {
            if (!player.LoggedIn)
                return;
            //if (!character.MuteTime.HasValue)
            string changedmessage = (player.Team?.ChatColor ?? string.Empty) + player.Client.Name + "!{220|220|220}: " + message;
            if (isDirty)
                changedmessage = "!{160|50|0}[DIRTY] " + changedmessage;
            player.CurrentLobby?.SendAllPlayerChatMessage(changedmessage);
            if ((player.CurrentLobby?.IsOfficial ?? false) && !isDirty)
                ChatLogsManager.Log(message, player);
            //else if (character.IsPermamuted)
            //    player.SendNotification(character.Language.STILL_PERMAMUTED);
            //else
            //    player.SendNotification(Utils.GetReplaced(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public static void SendGlobalMessage(TDSPlayer character, string message)
        {
            string changedmessage = "[GLOBAL] " + (character.Team?.ChatColor ?? string.Empty) + character.Client.Name + "!{220|220|220}: " + message;
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
            if (character.Team == null)
                return;
            string changedmessage = "[TEAM] " + character.Team.ChatColor + character.Client.Name + ": !{220|220|220}" + message;
            character.CurrentLobby?.SendAllPlayerChatMessage(changedmessage, character.Team);
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