namespace TDS.Manager.Player
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS.Entity;
    using TDS.Interface;
    using TDS.Enum;
    using TDS.Manager.Utility;
    using TDS.Default;

    class Account : Script
    {
        public Account()
        {
        }

        private static void SendWelcomeMessage(Client player)
        {
            StringBuilder builder = new StringBuilder();
            ILanguage lang = player.GetLang();
            builder.Append("#o#__________________________________________#w#");
            for (int i = 1; i <= lang.WELCOME_MESSAGE.Length; i++)
            {
                builder.Append(lang.WELCOME_MESSAGE[i]);
            }
            builder.Append("#n##o#__________________________________________");
            player.SendChatMessage(builder.ToString());
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            Players entity = player.GetEntity();
            if (entity == null)
                return;

            if (!entity.Playerstats.LoggedIn)
                return;

            entity.Playerstats.LoggedIn = false;
            if (entity.AdminLvl > 0)
                AdminsManager.SetOffline(player);

            using (var dbcontext = new TDSNewContext())
            {
                dbcontext.SaveChangesAsync();
                dbcontext.Entry(entity).State = EntityState.Detached;
            }
            //NAPI.ClientEvent.TriggerClientEventForAll ( "onClientPlayerQuit", player.Value );   //TODO NOT USED RIGHT NOW
        }

        [RemoteEvent("onPlayerTryRegister")]
        public static async void OnPlayerTryRegisterEvent(Client player, string password, string email)
        {
            if (await Player.DoesPlayerWithScnameExist(player.SocialClubName))
                return;
            Register.RegisterPlayer(player, password, email);
        }

        //TODO check parameter (hitsoundon and language removed) + check if event gets triggered AFTER Login!!
        [RemoteEvent("onPlayerChatLoad")]
        public static void OnPlayerChatLoadEvent(Client player)
        {
            SendWelcomeMessage(player);
            OfflineMessagesManager.CheckOfflineMessages(player);
        }

        [RemoteEvent("onPlayerTryLogin")]
        public static async void OnPlayerTryLoginEvent(Client player, string password)
        {
            uint id = await Player.GetPlayerIDByScname(player.SocialClubName);
            if (id != 0)
            {
                Login.LoginPlayer(player, id, password);
            }
            else
                NAPI.Chat.SendChatMessageToPlayer(player, player.GetLang().ACCOUNT_DOESNT_EXIST);
        }

        [RemoteEvent("onPlayerLanguageChange")]
        public void OnPlayerLanguageChangeEvent(Client player, byte language)
        {
            if (Enum.IsDefined(typeof(ELanguage), language))
                player.GetEntity().Playersettings.Language = language;
        }

        //[DisableDefaultOnConnectSpawn] TODO on new Version 0.4.0.1
        [ServerEvent(Event.PlayerConnected)]
        public static async void OnPlayerConnected(Client player)
        {
            player.Position = new Vector3(0, 0, 1000).Around(10);
            player.Freeze(true);
            player.Name = player.SocialClubName;    //TODO make it settable

            using (var dbcontext = new TDSNewContext())
            {
                var ban = await dbcontext.Playerbans
                    .Where(b => b.ForLobby == 0)
                    .Where(b => b.Scname == player.SocialClubName || b.Serial == player.Serial || b.Ip == player.Address)
                    .Where(b => !b.EndTimestamp.HasValue || b.EndTimestamp.Value > DateTime.Now)    //TODO: Is that correct this way?
                    .AsNoTracking()
                    .Select(b => new {
                        b.Reason,
                        Admin = b.AdminNavigation.Name,
                        Start = b.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo),
                        End = b.EndTimestamp.HasValue ? b.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never",
                     })
                    .FirstOrDefaultAsync();

                if (ban != null)
                {
                    player.Kick($"Banned! Admin: {ban.Admin} - Reason: {ban.Reason} - End: {ban.End} - Start: {ban.Start}");  //Todo: Test line break and display
                    return;
                }
            }

            NAPI.ClientEvent.TriggerClientEvent(player, DCustomEvents.StartRegisterLogin, player.SocialClubName, await Player.DoesPlayerWithScnameExist(player.SocialClubName));
        }

        /*public static void PermaBanPlayer(Character admincharacter, Client target, string targetname, string targetaddress, string reason, uint targetuid)
        {
            Client admin = admincharacter.Player;
            Database.ExecPrepared($"REPLACE INTO ban (uid, socialclubname, address, type, startsec, startoptic, admin, reason) VALUES " +
                $"({targetuid}, @socialclubname, @address, 0, '{Utility.GetTimespan()}', '{Utility.GetTimestamp()}', {admincharacter.UID}, @reason)",

                new Dictionary<string, string> {
                    { "@socialclubname", targetname }, { "@address", targetaddress }, { "@reason", reason } }
            );
            socialClubNameBanDict[targetname] = true;
            if (targetaddress != "-")
                addressBanDict[targetaddress] = true;
            ServerLanguage.SendMessageToAll("permaban", targetname, admin.Name, reason);
            if (target != null)
                target.Kick(target.GetLang("youpermaban", admin.Name, reason));
            // LOG //
            AdminLog.Log(AdminLogType.PERMABAN, admincharacter.UID, PlayerUIDs[targetname], reason);
            /////////
        }

        public static void TimeBanPlayer(Character admincharacter, Client target, string targetname, string targetaddress, string reason, int hours, uint targetuid)
        {
            Client admin = admincharacter.Player;
            Database.ExecPrepared($"REPLACE INTO ban (uid, socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES " +
                $"({targetuid}, @socialclubname, @address, 1, {Utility.GetTimespan()}, '{Utility.GetTimestamp()}', " +
                $"{Utility.GetTimespan(hours * 3600)}, '{Utility.GetTimestamp(hours * 3600)}', {admincharacter.UID}, @reason)", new Dictionary<string, string> {
                    { "@socialclubname", targetname },
                    { "@address", targetaddress },
                    { "@admin", admin.SocialClubName },
                    { "@reason", reason }
            });
            socialClubNameBanDict[targetname] = true;
            if (targetaddress != "-")
                addressBanDict[targetaddress] = true;
            ServerLanguage.SendMessageToAll("timeban", targetname, hours.ToString(), admin.Name, reason);
            if (target != null)
                target.Kick(target.GetLang("youtimeban", hours.ToString(), admin.Name, reason));
            // LOG //
            AdminLog.Log(AdminLogType.TIMEBAN, admincharacter.UID, PlayerUIDs[targetname], reason, hours);
            /////////
        }

        public static async Task UnBanPlayer(Character admincharacter, Client target, string targetname, string reason, uint uid)
        {
            DataTable result = await Database.ExecResult($"SELECT address FROM ban WHERE uid = {uid}").ConfigureAwait(false);
            string targetaddress = result.Rows[0]["address"].ToString();
            Database.Exec($"DELETE FROM ban WHERE uid = {uid}");
            socialClubNameBanDict.Remove(targetname);
            if (targetaddress != "-")
                addressBanDict.Remove(targetaddress);

            ServerLanguage.SendMessageToAll("unban", targetname, admincharacter.Player.Name, reason);
            // LOG //
            AdminLog.Log(AdminLogType.UNBAN, admincharacter.UID, PlayerUIDs[targetname], reason);
            /////////
        }

        public static void ChangePlayerMuteTime(Character admincharacter, Client target, uint targetUID, int minutes, string reason)
        {
            Client admin = admincharacter.Player;
            Database.Exec($"UPDATE player SET mutetime={targetUID} WHERE uid = {targetUID}");
            if (target != null && target.Exists && target.GetChar().LoggedIn)
                target.GetChar().MuteTime = minutes;
            switch (minutes)
            {
                case -1:
                    ServerLanguage.SendMessageToAll("permamute", GetNameByUID(targetUID), admin.Name, reason);
                    AdminLog.Log(AdminLogType.PERMAMUTE, admincharacter.UID, targetUID, reason);
                    break;
                case 0:
                    ServerLanguage.SendMessageToAll("unmute", GetNameByUID(targetUID), admin.Name, reason);
                    AdminLog.Log(AdminLogType.UNMUTE, admincharacter.UID, targetUID, reason);
                    break;
                default:
                    ServerLanguage.SendMessageToAll("timemute", GetNameByUID(targetUID), minutes.ToString(), admin.Name, reason);
                    AdminLog.Log(AdminLogType.TIMEMUTE, admincharacter.UID, targetUID, reason, minutes);
                    break;
            }


        }*/

    }

}
