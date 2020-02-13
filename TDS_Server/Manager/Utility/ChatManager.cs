using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Admin;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;

namespace TDS_Server.Manager.Utility
{
    internal class ChatManager : Script
    {
        public ChatManager()
        {
        }

        [RemoteEvent(DToServerEvent.LobbyChatMessage)]
        public static void SendLobbyMessage(Player client, string message, int chatTypeNumber)
        {
            TDSPlayer player = client.GetChar();
            if (player.IsPermamuted)
            {
                client.SendNotification(player.Language.STILL_PERMAMUTED);
                return;
            }
            if (player.IsMuted)
            {
                client.SendNotification(player.Language.STILL_MUTED.Replace("{0}", player.MuteTime?.ToString() ?? "?"));
                return;
            }

            var chatType = (EChatType)chatTypeNumber;
            switch (chatType)
            {
                case EChatType.Normal:
                    SendLobbyMessage(player, message, false);
                    break;
                case EChatType.Dirty:
                    SendLobbyMessage(player, message, true);
                    break;
                case EChatType.Team:
                    SendTeamChat(player, message);
                    break;
                case EChatType.Global:
                    SendGlobalMessage(player, message);
                    break;
            }
        }

        public static void SendLobbyMessage(TDSPlayer player, string message, bool isDirty)
        {
            if (!player.LoggedIn)
                return;
            //if (!character.MuteTime.HasValue)
            string changedmessage = (player.Team?.ChatColor ?? string.Empty) + player.DisplayName + "!$220|220|220$: " + message;
            if (isDirty)
                changedmessage = "!$160|50|0$[DIRTY] " + changedmessage + "$Dirty$";
            player.CurrentLobby?.SendAllPlayerChatMessage(changedmessage, player.BlockingPlayerIds);
            if ((player.CurrentLobby?.IsOfficial ?? false) && !isDirty)
                ChatLogsManager.Log(message, player);
            //else if (character.IsPermamuted)
            //    player.SendNotification(character.Language.STILL_PERMAMUTED);
            //else
            //    player.SendNotification(Utils.GetReplaced(character.Language.STILL_MUTED, character.MuteTime.Value));
        }

        public static void SendGlobalMessage(TDSPlayer player, string message)
        {
            string changedmessage = "[GLOBAL] " + (player.Team?.ChatColor ?? string.Empty) + player.DisplayName + "!$220|220|220$: " + message + "$Global$";
            var blockingIds = player.BlockingPlayerIds;
            foreach (var target in PlayerManager.PlayerManager.LoggedInPlayers)
            {
                if (blockingIds.Contains(target.Entity?.Id ?? 0))
                    continue;
                target.SendMessage(changedmessage);
            }
            ChatLogsManager.Log(message, player, isglobal: true);
        }

        public static void SendAdminMessage(TDSPlayer player, string message)
        {
            string changedmessage = player.AdminLevel.FontColor + "[" + player.AdminLevelName + "] !$255|255|255$" + player.DisplayName + ": !$220|220|220$" + message;
            NAPI.Chat.SendChatMessageToAll(changedmessage);
            ChatLogsManager.Log(message, player, isglobal: true, isadminchat: true);
        }

        public static void SendAdminChat(TDSPlayer player, string message)
        {
            string changedmessage = "[ADMINCHAT] " + player.AdminLevel.FontColor + player.DisplayName + ": !$220|220|220$" + message;
            AdminsManager.SendChatMessageToAdmins(changedmessage);
            ChatLogsManager.Log(message, player, isglobal: true, isadminchat: true);
        }

        public static void SendTeamChat(TDSPlayer player, string message)
        {
            if (player.Team is null)
                return;
            string changedmessage = "[TEAM] " + player.Team.ChatColor + player.DisplayName + ": !$220|220|220$" + message + "$Team$";
            player.CurrentLobby?.SendAllPlayerChatMessage(changedmessage, player.BlockingPlayerIds, player.Team);
            ChatLogsManager.Log(message, player, isteamchat: true);
        }

        public static void SendPrivateMessage(TDSPlayer player, TDSPlayer target, string message)
        {
            string changedmessage = "[PM] !$253|132|85$" + player.DisplayName + ": !$220|220|220$" + message;
            target.SendMessage(changedmessage);
            ChatLogsManager.Log(message, player, target: target);
        }
    }
}
