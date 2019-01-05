namespace TDS_Server.Manager.Player
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS_Server.Entity;
    using TDS_Server.Interface;
    using TDS_Server.Manager.Utility;
    using TDS_Common.Default;
    using TDS_Common.Enum;
    using TDS_Server.Instance.Player;

    class Account : Script
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
        public static void OnPlayerDisconnected(Client client, DisconnectionType type, string reason)
        {
            TDSPlayer player = client.GetChar();
            if (player.Entity == null)
                return;

            if (!player.LoggedIn)
                return;

            player.Entity.Playerstats.LoggedIn = false;
            if (player.AdminLevel.Level > 0)
                AdminsManager.SetOffline(player);

            using (var dbcontext = new TDSNewContext())
            {
                dbcontext.Entry(player.Entity).State = EntityState.Modified;
                dbcontext.SaveChangesAsync();
            }
#warning Check that after Client implementation
            //NAPI.ClientEvent.TriggerClientEventForAll ( "onClientPlayerQuit", player.Value );   
        }

        [RemoteEvent(DToServerEvent.TryRegister)]
        public static async void OnPlayerTryRegisterEvent(Client player, string password, string email)
        {
            if (await Player.DoesPlayerWithScnameExist(player.SocialClubName))
                return;
            Register.RegisterPlayer(player, password, email);
        }

# warning TODO check parameter (hitsoundon and language removed) + check if event gets triggered AFTER Login!!
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

        [RemoteEvent(DToServerEvent.LanguageChange)]
        public void OnPlayerLanguageChangeEvent(Client player, byte language)
        {
            if (Enum.IsDefined(typeof(ELanguage), language))
                player.GetChar().LanguageEnum = (ELanguage) language;
        }

        //[DisableDefaultOnConnectSpawn] TODO on new Version 0.4.0.1
        [ServerEvent(Event.PlayerConnected)]
        public static async void OnPlayerConnected(Client player)
        {
            player.Position = new Vector3(0, 0, 1000).Around(10);
            player.Freeze(true);
            #warning TODO Make it settable
            player.Name = player.SocialClubName;  

            using (var dbcontext = new TDSNewContext())
            {
                var ban = await dbcontext.Playerbans
                    .Where(b => b.ForLobby == 0)
                    .Where(b => b.Scname == player.SocialClubName || b.Serial == player.Serial || b.Ip == player.Address)
#warning Todo: Is that correct this way?
                    .Where(b => !b.EndTimestamp.HasValue || b.EndTimestamp.Value > DateTime.Now)
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
#warning Todo: Test line break and display
                    player.Kick($"Banned! Admin: {ban.Admin} - Reason: {ban.Reason} - End: {ban.End} - Start: {ban.Start}");
                    return;
                }
            }

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.StartRegisterLogin, player.SocialClubName, await Player.DoesPlayerWithScnameExist(player.SocialClubName));
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
