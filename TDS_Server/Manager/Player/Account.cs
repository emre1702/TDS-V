﻿using GTANetworkAPI;
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

namespace TDS_Server.Manager.Player
{
    internal class Account : Script
    {
        public Account()
        {
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
            if (player.Entity == null)
                return;

            if (!player.LoggedIn)
                return;

            player.Entity.PlayerStats.LoggedIn = false;
            if (player.AdminLevel.Level > 0)
                AdminsManager.SetOffline(player);
            player.ClosePrivateChat(true);

            CustomEventManager.SetPlayerLoggedOut(player);

            await player.SaveData();
            player.Logout();

            LangUtils.SendAllNotification(lang => string.Format(lang.PLAYER_LOGGED_OUT, client.Name));
        }

        [RemoteEvent(DToServerEvent.TryRegister)]
        public static async void OnPlayerTryRegisterEvent(Client player, string username, string password, string email)
        {
            if (await Player.DoesPlayerWithScnameExist(player.SocialClubName))
                return;
            if (await Player.DoesPlayerWithNameExist(username))
            {
                player.SendNotification(player.GetChar().Language.PLAYER_WITH_NAME_ALREADY_EXISTS);
                return;
            }
            Register.RegisterPlayer(player, username, password, email.Length != 0 ? email : null);
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

        //[DisableDefaultOnConnectSpawn] TODO on new Version 0.4.0.1
        [ServerEvent(Event.PlayerConnected)]
        public static async void OnPlayerConnected(Client player)
        {
            while (!TDSNewContext.IsConfigured)
                await Task.Delay(1000);
            player.Position = new Vector3(0, 0, 1000).Around(10);
            Workaround.FreezePlayer(player, true);

            var playerIDName = await Player.DbContext.Players.Where(p => p.Name == player.Name || p.SCName == player.SocialClubName).Select(p => new { p.Id, p.Name }).FirstOrDefaultAsync();
            if (playerIDName == null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.StartRegisterLogin, player.SocialClubName, false);
                return;
            }

            PlayerBans? ban = await BansManager.DbContext.PlayerBans.FindAsync(playerIDName.Id, 0);    // MainMenu ban => server ban
            if (ban != null && ban.EndTimestamp.HasValue && ban.EndTimestamp.Value <= DateTime.Now)
            {
                BansManager.DbContext.Remove(ban);
                await BansManager.DbContext.SaveChangesAsync();
                ban = null;
            }

            if (ban != null)
            {
                string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
                string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
                //todo Test line break and display
                player.Kick($"Banned!\nAdmin: {ban.Admin}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}");
                return;
            }

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.StartRegisterLogin, playerIDName.Name, true);            
        }

        public static void ChangePlayerMuteTime(TDSPlayer admin, TDSPlayer target, int minutes, string reason)
        {
            if (target.Entity == null)
                return;
            OutputMuteInfo(admin.Client.Name, target.Entity.Name, minutes, reason);
            target.MuteTime = minutes == -1 ? (int?)null : minutes;
        }

        public static async void ChangePlayerMuteTime(TDSPlayer admin, Players target, int minutes, string reason)
        {
            OutputMuteInfo(admin.Client.Name, target.Name, minutes, reason);

            using var dbcontext = new TDSNewContext();
            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;
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
    }
}