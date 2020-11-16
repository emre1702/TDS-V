using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Account;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using PlayerDb = TDS.Server.Database.Entity.Player.Players;

namespace TDS.Server.LobbySystem.BansHandlers
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
            NAPI.Task.RunSafe(() =>
                target.SendChatMessage(string.Format(target.Language.TIMEBAN_YOU_INFO, lengthHours, admin.DisplayName, ban.Reason)));
        }

        protected override void OutputPermBanInfoToTarget(ITDSPlayer target, PlayerBans ban, ITDSPlayer admin)
        {
            var lengthHours = (ban.EndTimestamp!.Value - ban.StartTimestamp).TotalMinutes / 60;
            NAPI.Task.RunSafe(() =>
                target.SendChatMessage(string.Format(target.Language.PERMABAN_YOU_INFO, admin.DisplayName, ban.Reason)));
        }

        public override async Task<PlayerBans?> Unban(ITDSPlayer admin, PlayerDb target, string reason)
        {
            var oldBan = await base.Unban(admin, target, reason).ConfigureAwait(false);
            if (oldBan is { })
                _globalBansHandler.RemoveServerBan(oldBan);
            return oldBan;
        }

        protected override void OutputUnbanMessage(ITDSPlayer admin, string targetName, string reason)
        {
            LangHelper.SendAllChatMessage(lang => string.Format(lang.UNBAN_INFO, targetName, admin.Admin.LevelName, reason));
        }
    }
}
