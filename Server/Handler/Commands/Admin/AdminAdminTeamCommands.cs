using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminAdminTeamCommands
    {
        private readonly OfflineMessagesHandler _offlineMessagesHandler;
        private readonly LangHelper _langHelper;
        private readonly DatabasePlayerHelper _databasePlayerHelper;

        public AdminAdminTeamCommands(OfflineMessagesHandler offlineMessagesHandler, LangHelper langHelper, DatabasePlayerHelper databasePlayerHelper)
            => (_offlineMessagesHandler, _langHelper, _databasePlayerHelper) = (offlineMessagesHandler, langHelper, databasePlayerHelper);

        [TDSCommand(AdminCommand.SetAdmin, 1)]
        public async Task SetAdmin(ITDSPlayer player, ITDSPlayer target, [AdminLevelParameter] short adminLevel)
        {
            target.Entity!.AdminLeaderId = player.Entity!.Id;
            target.Entity.AdminLvl = adminLevel;
            await target.DatabaseHandler.SaveData().ConfigureAwait(false);

            NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.ADMIN_LEVEL_SET_SUCCESSFULLY, target.Name, adminLevel)));
            NAPI.Task.RunSafe(() => target.SendNotification(string.Format(target.Language.ADMIN_HAS_GAVE_YOU_ADMIN_LEVEL, player.DisplayName, adminLevel)));

            if (player.Admin.Level.Level - adminLevel > 1)
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.SHOULD_SET_ADMIN_LEADER_INFO));

            LoggingHandler.Instance.LogAdmin(LogType.AdminTeam, player, target, string.Empty);
        }

        [TDSCommand(AdminCommand.SetAdmin, 0)]
        public async Task SetAdmin(ITDSPlayer player, Players target, [AdminLevelParameter] short adminLevel)
        {
            target.AdminLeaderId = player.Entity!.Id;
            target.AdminLvl = adminLevel;
            await _databasePlayerHelper.Save(target);

            NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.ADMIN_LEVEL_SET_SUCCESSFULLY, target.Name, adminLevel)));
            _offlineMessagesHandler.Add(target, player.Entity, _langHelper.GetLang(Language.English).ADMIN_HAS_GAVE_YOU_ADMIN_LEVEL);

            if (player.Admin.Level.Level - adminLevel > 1)
                NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.SHOULD_SET_ADMIN_LEADER_INFO, target.Name)));

            LoggingHandler.Instance.LogAdmin(LogType.AdminTeam, player, string.Empty, targetid: target.Id);
        }
    }
}
