using GTANetworkAPI;
using GTANetworkMethods;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;
using Task = System.Threading.Tasks.Task;

namespace TDS_Server.Manager.Player
{
    internal class Account : Script
    {
        private static BansManager? _bansManager;

        public static void Init()
        {
            _bansManager = BansManager.Get();
        }

        private static void SendWelcomeMessage(Client player)
        {
            ILanguage lang = player.GetLang();
            NAPI.Chat.SendChatMessageToPlayer(player, "#o#__________________________________________");
            NAPI.Chat.SendChatMessageToPlayer(player, string.Join("#n#", lang.WELCOME_MESSAGE));
            NAPI.Chat.SendChatMessageToPlayer(player, "#o#__________________________________________");
        }

        [ServerEvent(Event.PlayerDisconnected)]
#pragma warning disable IDE0060 // Remove unused parameter
        public static async void OnPlayerDisconnected(Client client, DisconnectionType type, string reason)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            TDSPlayer player = client.GetChar();
            if (player.Entity is null)
                return;

            if (!player.LoggedIn)
                return;

            player.Entity.PlayerStats.LoggedIn = false;
            player.ClosePrivateChat(true);

            CustomEventManager.SetPlayerLoggedOut(player);

            await player.SaveData(true);
            player.Logout();

            LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_OUT, player.DisplayName));
        }

        [RemoteEvent(DToServerEvent.TryRegister)]
        public static async void OnPlayerTryRegisterEvent(Client client, string username, string password, string email)
        {
            if (await Player.DoesPlayerWithScnameExist(client.SocialClubName))
                return;
            TDSPlayer player = client.GetChar();
            if (await Player.DoesPlayerWithNameExist(username))
            {
                client.SendNotification(player.Language.PLAYER_WITH_NAME_ALREADY_EXISTS);
                return;
            }
            char? invalidChar = Utils.CheckNameValid(username);
            if (invalidChar.HasValue)
            {
                client.SendNotification(string.Format(player.Language.CHAR_IN_NAME_IS_NOT_ALLOWED, invalidChar.Value));
                return;
            }
            Register.RegisterPlayer(client, username, password, email.Length != 0 ? email : null);
        }

        [RemoteEvent(DToServerEvent.ChatLoaded)]
        public static void OnPlayerChatLoadEvent(Client player)
        {
            SendWelcomeMessage(player);
            TDSPlayer character = player.GetChar();
            character.ChatLoaded = true;
            if (character.Entity != null)
                OfflineMessagesManager.CheckOfflineMessages(character);
        }

        [RemoteEvent(DToServerEvent.TryLogin)]
        public static async void OnPlayerTryLoginEvent(Client player, string username, string password)
        {
            int id = await Player.GetPlayerIDByName(username);
            if (id != 0)
            {
                Login.LoginPlayer(player, id, password);
            }
            else
                NAPI.Notification.SendNotificationToPlayer(player, LangUtils.GetLang(ELanguage.English).ACCOUNT_DOESNT_EXIST);
        }

        [RemoteEvent(DToServerEvent.LanguageChange)]
        public void OnPlayerLanguageChangeEvent(Client player, int language)
        {
            if (System.Enum.IsDefined(typeof(ELanguage), language))
                player.GetChar().LanguageEnum = (ELanguage)language;
        }

        [ServerEvent(Event.IncomingConnection)]
        public static async void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            while (_bansManager is null)
                await Task.Delay(1000);

            var ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, null, ip, serial, socialClubName, socialClubId, true);
            if (ban is { })
                cancel.Cancel = true;
        }

        //[DisableDefaultOnConnectSpawn] TODO on new Version 0.4.0.1
        [ServerEvent(Event.PlayerConnected)]
        public static async void OnPlayerConnected(Client player)
        {
            while (_bansManager is null)
                await Task.Delay(1000);

            player.Position = new Vector3(0, 0, 1000).Around(10);
            Workaround.FreezePlayer(player, true);

            var ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, null, player.Address, player.Serial, player.SocialClubName, player.SocialClubId, false);
            if (!HandlePlayerBan(player, ban))
                return;

            using var dbContext = new TDSNewContext();
            var playerIDName = await dbContext.Players.Where(p => p.Name == player.Name || p.SCName == player.SocialClubName).Select(p => new { p.Id, p.Name }).FirstOrDefaultAsync();
            if (playerIDName is null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.StartRegisterLogin, player.SocialClubName, false);
                return;
            }

            ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, playerIDName.Id);
            if (!HandlePlayerBan(player, ban))
                return;

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.StartRegisterLogin, playerIDName.Name, true);            
        }

        private static bool HandlePlayerBan(Client player, PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr =  ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
            //todo Test line break and display
            player.Kick($"Banned!\nName: {ban.Player?.Name ?? player.Name}\nAdmin: {ban.Admin}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}");

            return false;
        }

        public static void ChangePlayerMuteTime(TDSPlayer admin, TDSPlayer target, int minutes, string reason)
        {
            if (target.Entity is null)
                return;
            OutputMuteInfo(admin.DisplayName, target.Entity.Name, minutes, reason);
            target.MuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);
        }

        public static async void ChangePlayerMuteTime(TDSPlayer admin, Players target, int minutes, string reason)
        {
            OutputMuteInfo(admin.DisplayName, target.Name, minutes, reason);

            using var dbcontext = new TDSNewContext();
            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;
            dbcontext.Entry(target.PlayerStats).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync();
        }

        public static void ChangePlayerVoiceMuteTime(TDSPlayer admin, TDSPlayer target, int minutes, string reason)
        {
            if (target.Entity is null)
                return;
            OutputVoiceMuteInfo(admin.DisplayName, target.Entity.Name, minutes, reason);
            target.VoiceMuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);

            if (target.Team != null)
            {
                foreach (var player in target.Team.Players)
                {
                    target.Client.DisableVoiceTo(player.Client);
                }
            }
            
        }

        public static async void ChangePlayerVoiceMuteTime(TDSPlayer admin, Players target, int minutes, string reason)
        {
            OutputVoiceMuteInfo(admin.DisplayName, target.Name, minutes, reason);

            using var dbcontext = new TDSNewContext();
            target.PlayerStats.VoiceMuteTime = minutes == -1 ? (int?)null : minutes;
            dbcontext.Entry(target.PlayerStats).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync();
        }

        private static void OutputMuteInfo(string adminName, string targetName, float minutes, string reason)
        {
            switch (minutes)
            {
                case -1:
                    LangUtils.SendAllChatMessage(lang => lang.PERMAMUTE_INFO.Formatted(targetName, adminName, reason));
                    break;

                case 0:
                    LangUtils.SendAllChatMessage(lang => lang.UNMUTE_INFO.Formatted(targetName, adminName, reason));
                    break;

                default:
                    LangUtils.SendAllChatMessage(lang => lang.TIMEMUTE_INFO.Formatted(targetName, adminName, minutes, reason));
                    break;
            }
        }

        private static void OutputVoiceMuteInfo(string adminName, string targetName, float minutes, string reason)
        {
            switch (minutes)
            {
                case -1:
                    LangUtils.SendAllChatMessage(lang => lang.PERMAVOICEMUTE_INFO.Formatted(targetName, adminName, reason));
                    break;

                case 0:
                    LangUtils.SendAllChatMessage(lang => lang.UNVOICEMUTE_INFO.Formatted(targetName, adminName, reason));
                    break;

                default:
                    LangUtils.SendAllChatMessage(lang => lang.TIMEVOICEMUTE_INFO.Formatted(targetName, adminName, minutes, reason));
                    break;
            }
        }
    }
}