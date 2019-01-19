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

            player.Entity.PlayerStats.LoggedIn = false;
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
            #warning TODO Make the name settable
            player.Name = player.SocialClubName;

            bool isPlayerRegistered = false;
            using (var dbcontext = new TDSNewContext())
            {
                uint playerID = await dbcontext.Players.Where(p => p.Name == player.Name).Select(p => p.Id).FirstOrDefaultAsync();
                if (playerID != 0)
                {
                    isPlayerRegistered = true;
                    var ban = await dbcontext.PlayerBans.FindAsync(playerID, 0);    // MainMenu ban => server ban
                    if (ban != null)
                    {
                        if (!ban.EndTimestamp.HasValue || ban.EndTimestamp.Value > DateTime.Now)
                        {
                            string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
                            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
                            #warning Test line break and display
                            player.Kick($"Banned!\nAdmin: {ban.Admin}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}");
                            return;
                        }
                        dbcontext.Remove(ban); 
                    }
                }
            }

            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.StartRegisterLogin, player.SocialClubName, isPlayerRegistered);
        }

        /*public static void ChangePlayerMuteTime(Character admincharacter, Client target, uint targetUID, int minutes, string reason)
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
