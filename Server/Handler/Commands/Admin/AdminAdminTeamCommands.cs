using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Events;
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
        private readonly EventsHandler _eventsHandler;

        public AdminAdminTeamCommands(OfflineMessagesHandler offlineMessagesHandler, LangHelper langHelper, DatabasePlayerHelper databasePlayerHelper,
            EventsHandler eventsHandler)
            => (_offlineMessagesHandler, _langHelper, _databasePlayerHelper, _eventsHandler) = (offlineMessagesHandler, langHelper, databasePlayerHelper, eventsHandler);

        [TDSCommand(AdminCommand.SetAdmin, 1)]
        public async Task SetAdmin(ITDSPlayer player, ITDSPlayer target, [AdminLevelParameter] short adminLevel)
        {
            var oldAdminLevel = target.Entity!.AdminLvl;
            target.Entity.AdminLeaderId = player.Entity!.Id;
            target.Entity.AdminLvl = adminLevel;
            await target.DatabaseHandler.SaveData().ConfigureAwait(false);

            _eventsHandler.OnPlayerAdminLevelChange(target, oldAdminLevel, adminLevel);

            NAPI.Task.RunSafe(() =>
            {
                player.SendNotification(string.Format(player.Language.ADMIN_LEVEL_SET_SUCCESSFULLY, target.Name, adminLevel));
                target.SendNotification(string.Format(target.Language.ADMIN_HAS_SET_YOUR_ADMIN_LEVEL, player.Name, adminLevel));
            });

            if (player.Admin.Level.Level - adminLevel > 1)
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.SHOULD_SET_ADMIN_LEADER_INFO));

            LoggingHandler.Instance.LogAdmin(LogType.SetAdmin, player, target, string.Empty);
        }

        [TDSCommand(AdminCommand.SetAdmin, 0)]
        public async Task SetAdmin(ITDSPlayer player, Players target, [AdminLevelParameter] short adminLevel)
        {
            target.AdminLeaderId = player.Entity!.Id;
            target.AdminLvl = adminLevel;
            await _databasePlayerHelper.Save(target).ConfigureAwait(false);

            NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.ADMIN_LEVEL_SET_SUCCESSFULLY, target.Name, adminLevel)));
            _offlineMessagesHandler.Add(target, player.Entity, string.Format(_langHelper.GetLang(Language.English).ADMIN_HAS_SET_YOUR_ADMIN_LEVEL, player.Name, adminLevel));

            if (player.Admin.Level.Level - adminLevel > 1)
                NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.SHOULD_SET_ADMIN_LEADER_INFO, target.Name)));

            LoggingHandler.Instance.LogAdmin(LogType.SetAdmin, player, string.Empty, targetid: target.Id);
        }

        [TDSCommand(AdminCommand.SetAdminLeader, 3)]
        public async Task SetAdminLeader(ITDSPlayer player, ITDSPlayer target, ITDSPlayer adminLeader)
        {
            target.Entity!.AdminLeaderId = adminLeader.Entity!.Id;
            await target.DatabaseHandler.SaveData().ConfigureAwait(false);

            NAPI.Task.RunSafe(() => 
            {
                player.SendNotification(string.Format(player.Language.ADMIN_LEADER_SET_SUCCESSFULLY, target.Name, adminLeader.Name));
                target.SendNotification(string.Format(target.Language.ADMIN_HAS_SET_YOUR_ADMIN_LEADER, player.Name, adminLeader.Name));
                adminLeader.SendNotification(string.Format(adminLeader.Language.YOUVE_BECOME_ADMIN_LEADER, player.Name, target.Name));
            });

            LoggingHandler.Instance.LogAdmin(LogType.SetAdminLeader, player, target, string.Empty);
        }

        [TDSCommand(AdminCommand.SetAdminLeader, 2)]
        public async Task SetAdminLeader(ITDSPlayer player, Players target, ITDSPlayer adminLeader)
        {
            target.AdminLeaderId = adminLeader.Entity!.Id;
            await _databasePlayerHelper.Save(target).ConfigureAwait(false);

            NAPI.Task.RunSafe(() => 
            {
                player.SendNotification(string.Format(player.Language.ADMIN_LEADER_SET_SUCCESSFULLY, target.Name, adminLeader.Name));
                adminLeader.SendNotification(string.Format(adminLeader.Language.YOUVE_BECOME_ADMIN_LEADER, player.Name, target.Name));
            });
            _offlineMessagesHandler.Add(target, player.Entity!, string.Format(_langHelper.GetLang(Language.English).ADMIN_HAS_SET_YOUR_ADMIN_LEADER, player.Name, adminLeader.Name));

            LoggingHandler.Instance.LogAdmin(LogType.SetAdminLeader, player, string.Empty, targetid: target.Id);
        }

        [TDSCommand(AdminCommand.SetAdminLeader, 1)]
        public async Task SetAdminLeader(ITDSPlayer player, ITDSPlayer target, Players adminLeader)
        {
            target.Entity!.AdminLeaderId = adminLeader.Id;
            await target.DatabaseHandler.SaveData().ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                player.SendNotification(string.Format(player.Language.ADMIN_LEADER_SET_SUCCESSFULLY, target.Name, adminLeader.Name));
                target.SendNotification(string.Format(target.Language.ADMIN_HAS_SET_YOUR_ADMIN_LEADER, player.Name, adminLeader.Name));
            });
            _offlineMessagesHandler.Add(adminLeader, player.Entity!, string.Format(_langHelper.GetLang(Language.English).YOUVE_BECOME_ADMIN_LEADER, player.Name, target.Name));

            LoggingHandler.Instance.LogAdmin(LogType.SetAdminLeader, player, target, string.Empty);
        }

        [TDSCommand(AdminCommand.SetAdminLeader, 0)]
        public async Task SetAdminLeader(ITDSPlayer player, Players target, Players adminLeader)
        {
            target.AdminLeaderId = adminLeader.Id;
            await _databasePlayerHelper.Save(target).ConfigureAwait(false);

            NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.ADMIN_LEADER_SET_SUCCESSFULLY, target.Name, adminLeader.Name)));
            _offlineMessagesHandler.Add(target, player.Entity!, string.Format(_langHelper.GetLang(Language.English).ADMIN_HAS_SET_YOUR_ADMIN_LEADER, player.Name, adminLeader.Name));
            _offlineMessagesHandler.Add(adminLeader, player.Entity!, string.Format(_langHelper.GetLang(Language.English).YOUVE_BECOME_ADMIN_LEADER, player.Name, target.Name));

            LoggingHandler.Instance.LogAdmin(LogType.SetAdminLeader, player, string.Empty, targetid: target.Id);
        }

        [TDSCommand(AdminCommand.SetVip, 1)]
        public async Task SetVip(ITDSPlayer player, ITDSPlayer target, bool isVip)
        {
            target.Entity!.IsVip = isVip;
            await target.DatabaseHandler.SaveData().ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                player.SendNotification(string.Format(player.Language.VIP_SET_SUCCESSFULLY, target.Name, isVip));
                if (isVip)
                    target.SendNotification(string.Format(target.Language.ADMIN_HAS_SET_YOU_VIP, player.Name));
                else
                    target.SendNotification(string.Format(target.Language.ADMIN_HAS_SET_YOU_NOT_VIP, player.Name));
            });

            LoggingHandler.Instance.LogAdmin(LogType.SetVip, player, target, string.Empty);
        }

        [TDSCommand(AdminCommand.SetVip, 0)]
        public async Task SetVip(ITDSPlayer player, Players target, bool isVip)
        {
            target.IsVip = isVip;
            await _databasePlayerHelper.Save(target).ConfigureAwait(false);

            NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.VIP_SET_SUCCESSFULLY, target.Name, isVip)));
            if (isVip)
                _offlineMessagesHandler.Add(target, player.Entity!, string.Format(_langHelper.GetLang(Language.English).ADMIN_HAS_SET_YOU_VIP, player.Name));
            else
                _offlineMessagesHandler.Add(target, player.Entity!, string.Format(_langHelper.GetLang(Language.English).ADMIN_HAS_SET_YOU_NOT_VIP, player.Name));

            LoggingHandler.Instance.LogAdmin(LogType.SetVip, player, string.Empty, targetid: target.Id);
        }
    }
}
