using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility
{
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
            string changedmessage = (player.Team?.ChatColor ?? string.Empty) + player.DisplayName + "!{220|220|220}: " + message;
            if (isDirty)
                changedmessage = "!{160|50|0}[DIRTY] " + changedmessage;
            player.CurrentLobby?.SendAllPlayerChatMessage(changedmessage, player.BlockingPlayerIds);
            if ((player.CurrentLobby?.IsOfficial ?? false) && !isDirty)
                ChatLogsManager.Log(message, player);
            //else if (character.IsPermamuted)
            //    player.SendNotification(character.Language.STILL_PERMAMUTED);
            //else
            //    player.SendNotification(Utils.GetReplaced(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public static void SendGlobalMessage(TDSPlayer character, string message)
        {
            string changedmessage = "[GLOBAL] " + (character.Team?.ChatColor ?? string.Empty) + character.DisplayName + "!{220|220|220}: " + message;
            var blockingIds = character.BlockingPlayerIds;
            foreach (var target in Player.Player.LoggedInPlayers)
            {
                if (blockingIds.Contains(target.Entity?.Id ?? 0))
                    continue;
                NAPI.Chat.SendChatMessageToPlayer(target.Client, changedmessage);
            }
            ChatLogsManager.Log(message, character, isglobal: true);
        }

        public static void SendAdminMessage(TDSPlayer character, string message)
        {
            string changedmessage = character.AdminLevel.FontColor + "[" + character.AdminLevelName + "] !{255|255|255}" + character.DisplayName + ": !{220|220|220}" + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            ChatLogsManager.Log(message, character, isglobal: true, isadminchat: true);
        }

        public static void SendAdminChat(TDSPlayer character, string message)
        {
            string changedmessage = "[ADMINCHAT] " + character.AdminLevel.FontColor + character.DisplayName + ": !{220|220|220}" + message;
            AdminsManager.SendChatMessageToAdmins(changedmessage);
            ChatLogsManager.Log(message, character, isadminchat: true);
        }

        public static void SendTeamChat(TDSPlayer character, string message)
        {
            if (character.Team is null)
                return;
            string changedmessage = "[TEAM] " + character.Team.ChatColor + character.DisplayName + ": !{220|220|220}" + message;
            character.CurrentLobby?.SendAllPlayerChatMessage(changedmessage, character.BlockingPlayerIds, character.Team);
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