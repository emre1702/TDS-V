using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelHandler
    {

        private readonly UserpanelApplicationsAdminHandler _userpanelApplicationsAdminHandler;
        private readonly UserpanelApplicationUserHandler _userpanelApplicationUserHandler;

        private readonly UserpanelCommandsHandler _userpanelCommandsHandler;
        private readonly UserpanelRulesHandler _userpanelRulesHandler;
        private readonly UserpanelFAQsHandlers _userpanelFAQsHandlers;
        private readonly UserpanelPlayerStatsHandler _userpanelPlayerStatsHandler;

        private readonly UserpanelSupportUserHandler _userpanelSupportUserHandler;
        private readonly UserpanelSupportAdminHandler _userpanelSupportAdminHandler;
        private readonly UserpanelSettingsSpecialHandler _userpanelSettingsSpecialHandler;
        private readonly UserpanelOfflineMessagesHandler _userpanelOfflineMessagesHandler;

        public UserpanelHandler(IServiceProvider serviceProvider)
        {
            _userpanelPlayerStatsHandler = ActivatorUtilities.CreateInstance<UserpanelPlayerStatsHandler>(serviceProvider);
            _userpanelApplicationUserHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationUserHandler>(serviceProvider);
            _userpanelRulesHandler = ActivatorUtilities.CreateInstance<UserpanelRulesHandler>(serviceProvider);
            _userpanelFAQsHandlers = ActivatorUtilities.CreateInstance<UserpanelFAQsHandlers>(serviceProvider);
            _userpanelCommandsHandler = ActivatorUtilities.CreateInstance<UserpanelCommandsHandler>(serviceProvider);
            _userpanelOfflineMessagesHandler = ActivatorUtilities.CreateInstance<UserpanelOfflineMessagesHandler>(serviceProvider);
            _userpanelSettingsSpecialHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsSpecialHandler>(serviceProvider);

            var userpanelSupportRequestHandler = ActivatorUtilities.CreateInstance<UserpanelSupportRequestHandler>(serviceProvider);

            _userpanelApplicationsAdminHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationsAdminHandler>(serviceProvider, _userpanelPlayerStatsHandler);
            _userpanelSupportUserHandler = new UserpanelSupportUserHandler(userpanelSupportRequestHandler);
            _userpanelSupportAdminHandler = new UserpanelSupportAdminHandler(userpanelSupportRequestHandler);
        }

        public async void PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType dataType)
        {
            string? json = null;
            switch (dataType)
            {
                case UserpanelLoadDataType.Commands:
                    json = _userpanelCommandsHandler.GetData();
                    break;

                case UserpanelLoadDataType.Rules:
                    json = _userpanelRulesHandler.GetData();
                    player.AddToChallenge(ChallengeType.ReadTheRules);
                    break;

                case UserpanelLoadDataType.FAQs:
                    json = _userpanelFAQsHandlers.GetData(player);
                    player.AddToChallenge(ChallengeType.ReadTheFAQ);
                    break;

                case UserpanelLoadDataType.MyStats:
                    json = await _userpanelPlayerStatsHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.ApplicationUser:
                    json = await _userpanelApplicationUserHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.ApplicationsAdmin:
                    json = await _userpanelApplicationsAdminHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SettingsSpecial:
                    json = _userpanelSettingsSpecialHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SupportUser:
                    json = await _userpanelSupportUserHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SupportAdmin:
                    json = await _userpanelSupportAdminHandler.GetData(player);
                    break;
                case UserpanelLoadDataType.OfflineMessages:
                    json = await _userpanelOfflineMessagesHandler.GetData(player);
                    break;
            }

            if (json == null)
                return;

            player.SendEvent(ToClientEvent.LoadUserpanelData, (int)dataType, json);
        }
    }
}
