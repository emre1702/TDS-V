using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Helper;
using PlayerDb = TDS_Server.Database.Entity.Player.Players;

namespace TDS_Server.LobbySystem.BansHandlers
{
    public class MainMenuBansHandler : BaseLobbyBansHandler
    {
        private readonly BansHandler _globalBansHandler;

        public MainMenuBansHandler(IMainMenu lobby, LangHelper langHelper, BansHandler globalBansHandler)
            : base(lobby, langHelper)
        {
            _globalBansHandler = globalBansHandler;
        }

        public override ValueTask<bool> CheckIsBanned(ITDSPlayer player)
            => new ValueTask<bool>(false);

        protected override void OutputNewBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            base.OutputNewBanInfo(ban, admin, targetName);

            _globalBansHandler.AddServerBan(ban);
        }

        protected override void OutputNewTempBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            var lengthMinutes = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            string msgProvider(ILanguage lang) => string.Format(lang.TIMEBAN_INFO, targetName, lengthMinutes, admin.DisplayName, ban.Reason);
            LangHelper.SendAllChatMessage(msgProvider);
        }

        protected override void OutputNewPermBanInfo(PlayerBans ban, ITDSPlayer admin, string targetName)
        {
            string msgProvider(ILanguage lang) => string.Format(lang.PERMABAN_INFO, targetName, admin.DisplayName, ban.Reason);
            LangHelper.SendAllChatMessage(msgProvider);
        }

        protected override void OutputTempBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.Run(() =>
                target.SendChatMessage(string.Format(target.Language.TIMEBAN_YOU_INFO, lengthHours, admin.DisplayName, ban.Reason)));
        }

        protected override void OutputPermBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.Run(() =>
                target.SendChatMessage(string.Format(target.Language.PERMABAN_YOU_INFO, admin.DisplayName, ban.Reason)));
        }

        public override async Task<PlayerBans?> Unban(ITDSPlayer admin, PlayerDb target, string reason)
        {
            var oldBan = await base.Unban(admin, target, reason);
            if (oldBan is { })
                _globalBansHandler.RemoveServerBan(oldBan);
            return oldBan;
        }

        protected override void OutputUnbanMessage(ITDSPlayer admin, string targetName, string reason)
        {
            LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_INFO, targetName, admin.AdminLevelName, reason));
        }
    }
}
