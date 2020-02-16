using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;
using Task = System.Threading.Tasks.Task;

namespace TDS_Server.Manager.PlayerManager
{
    class Account : Script
    {
        private static BansManager? _bansManager;

        public static void Init()
        {
            _bansManager = BansManager.Get();
        }

        private static void SendWelcomeMessage(Player client)
        {
            var player = client.GetChar();
            player.SendMessage("#o#__________________________________________");
            player.SendMessage(string.Join("#n#", player.Language.WELCOME_MESSAGE));
            player.SendMessage("#o#__________________________________________");
        }

        [RemoteEvent(DToServerEvent.TryRegister)]
        public static async void OnPlayerTryRegisterEvent(Player client, string username, string password, string email)
        {
            if (username.Length < 3 || username.Length > 20)
                return;
            if (await PlayerManager.DoesPlayerWithScnameExist(client.SocialClubName).ConfigureAwait(true))
                return;
            TDSPlayer player = client.GetChar();
            if (await PlayerManager.DoesPlayerWithNameExist(username).ConfigureAwait(true))
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
        public static void OnPlayerChatLoadEvent(Player player)
        {
            SendWelcomeMessage(player);
            TDSPlayer character = player.GetChar();
            character.ChatLoaded = true;
            if (character.Entity != null)
                OfflineMessagesManager.CheckOfflineMessages(character);
        }

        [RemoteEvent(DToServerEvent.TryLogin)]
        public static async void OnPlayerTryLoginEvent(Player client, string username, string password)
        {
            int id = await PlayerManager.GetPlayerIDByName(username).ConfigureAwait(false);
            if (id != 0)
            {
                Login.LoginPlayer(client, id, password);
            }
            else
                client.GetChar()?.SendNotification(LangUtils.GetLang(ELanguage.English).ACCOUNT_DOESNT_EXIST);
        }

        [RemoteEvent(DToServerEvent.LanguageChange)]
        public void OnPlayerLanguageChangeEvent(Player client, int language)
        {
            if (System.Enum.IsDefined(typeof(ELanguage), language))
                client.GetChar().LanguageEnum = (ELanguage)language;
        }

        [ServerEvent(Event.IncomingConnection)]
        public static async void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            while (_bansManager is null)
                await Task.Delay(1000).ConfigureAwait(false);

            var ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, null, ip, serial, socialClubName, socialClubId, true).ConfigureAwait(false);
            if (ban is { })
                cancel.Cancel = true;
        }

        //[DisableDefaultOnConnectSpawn] TODO on new Version 0.4.0.1
        [ServerEvent(Event.PlayerConnected)]
        public static async void OnPlayerConnected(Player client)
        {
            while (_bansManager is null)
                await Task.Delay(1000).ConfigureAwait(true);

            client.Position = new Vector3(0, 0, 1000).Around(10);
            Workaround.FreezePlayer(client, true);

            var ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, null, client.Address, client.Serial, client.SocialClubName, client.SocialClubId, false).ConfigureAwait(true);
            if (!HandlePlayerBan(client, ban))
                return;

            using var dbContext = new TDSDbContext();
            var playerIDName = await dbContext.Players.Where(p => p.Name == client.Name || p.SCName == client.SocialClubName).Select(p => new { p.Id, p.Name })
                .FirstOrDefaultAsync()
                .ConfigureAwait(true);
            if (playerIDName is null)
            {
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.StartRegisterLogin, client.SocialClubName, false);
                return;
            }

            ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, playerIDName.Id).ConfigureAwait(true);
            if (!HandlePlayerBan(client, ban))
                return;

            NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.StartRegisterLogin, playerIDName.Name, true);
        }

        private static bool HandlePlayerBan(Player client, PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr =  ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
            //todo Test line break and display
            client.Kick($"Banned!\nName: {ban.Player?.Name ?? client.Name}\nAdmin: {ban.Admin}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}");

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

            using var dbcontext = new TDSDbContext();
            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;
            dbcontext.Entry(target.PlayerStats).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync().ConfigureAwait(false);
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
                    target.Player!.DisableVoiceTo(player.Player);
                }
            }
            
        }

        public static async void ChangePlayerVoiceMuteTime(TDSPlayer admin, Players target, int minutes, string reason)
        {
            OutputVoiceMuteInfo(admin.DisplayName, target.Name, minutes, reason);

            using var dbcontext = new TDSDbContext();
            target.PlayerStats.VoiceMuteTime = minutes == -1 ? (int?)null : minutes;
            dbcontext.Entry(target.PlayerStats).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync().ConfigureAwait(false);
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
