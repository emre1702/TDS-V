using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Player;
using Task = System.Threading.Tasks.Task;

namespace TDS_Server.Core.Manager.PlayerManager
{
    class Account
    {
        private static BansHandler? _bansManager;

        public static void Init()
        {
            _bansManager = BansManager.Get();
        }

        [RemoteEvent(DToServerEvent.TryRegister)]
        public async Task OnPlayerTryRegisterEvent(Player client, string username, string password, string email)
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

        

        [RemoteEvent(DToServerEvent.LanguageChange)]
        public void OnPlayerLanguageChangeEvent(Player client, int language)
        {
            if (System.Enum.IsDefined(typeof(Language), language))
                client.GetChar().LanguageEnum = (Language)language;
        }

        [ServerEvent(Event.IncomingConnection)]
        public async Task OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            while (_bansManager is null)
                await Task.Delay(1000).ConfigureAwait(false);

            var ban = await _bansManager.GetBan(LobbyManager.MainMenu.Id, null, ip, serial, socialClubName, socialClubId, true).ConfigureAwait(false);
            if (ban is { })
                cancel.Cancel = true;
        }

        private bool HandlePlayerBan(TDSPlayer player, PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
            //todo Test line break and display
            player.ModPlayer?.Kick($"Banned!\nName: {ban.Player?.Name ?? player.DisplayName}\nAdmin: {ban.Admin}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}");

            return false;
        }

        public void ChangePlayerMuteTime(TDSPlayer admin, TDSPlayer target, int minutes, string reason)
        {
            if (target.Entity is null)
                return;
            OutputMuteInfo(admin.DisplayName, target.Entity.Name, minutes, reason);
            target.MuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);
        }

        public async Task ChangePlayerMuteTime(TDSPlayer admin, Players target, int minutes, string reason)
        {
            OutputMuteInfo(admin.DisplayName, target.Name, minutes, reason);

            using var dbcontext = new TDSDbContext();
            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;
            dbcontext.Entry(target.PlayerStats).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync().ConfigureAwait(false);
        }

        public void ChangePlayerVoiceMuteTime(TDSPlayer admin, TDSPlayer target, int minutes, string reason)
        {
            if (target.Entity is null)
                return;
            OutputVoiceMuteInfo(admin.DisplayName, target.Entity.Name, minutes, reason);
            target.VoiceMuteTime = minutes == -1 ? 0 : (minutes == 0 ? (int?)null : minutes);

            if (target.VoiceMuteTime is { } && target.Team is { })
            {
                foreach (var player in target.Team.Players)
                {
                    target.SetVoiceTo(player, false);
                }
            }

        }

        public async Task ChangePlayerVoiceMuteTime(TDSPlayer admin, Players target, int minutes, string reason)
        {
            OutputVoiceMuteInfo(admin.DisplayName, target.Name, minutes, reason);

            using var dbcontext = new TDSDbContext();
            target.PlayerStats.VoiceMuteTime = minutes == -1 ? (int?)null : minutes;
            dbcontext.Entry(target.PlayerStats).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync().ConfigureAwait(false);
        }

        private void OutputMuteInfo(string adminName, string targetName, float minutes, string reason)
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

        private void OutputVoiceMuteInfo(string adminName, string targetName, float minutes, string reason)
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
