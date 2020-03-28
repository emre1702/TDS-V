using BonusBotConnector_Server;
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

        public readonly UserpanelApplicationsAdminHandler ApplicationsAdminHandler;
        public readonly UserpanelApplicationUserHandler ApplicationUserHandler;

        private readonly UserpanelCommandsHandler _commandsHandler;
        private readonly UserpanelRulesHandler _rulesHandler;
        private readonly UserpanelFAQsHandlers _fAQsHandlers;
        private readonly UserpanelPlayerStatsHandler _playerStatsHandler;

        public readonly UserpanelSupportUserHandler SupportUserHandler;
        public readonly UserpanelSupportAdminHandler SupportAdminHandler;
        public readonly UserpanelSupportRequestHandler SupportRequestHandler;
        public readonly UserpanelSettingsNormalHandler SettingsNormalHandler;
        public readonly UserpanelSettingsSpecialHandler SettingsSpecialHandler;
        public readonly UserpanelOfflineMessagesHandler OfflineMessagesHandler;

        public UserpanelHandler(IServiceProvider serviceProvider, BonusBotConnectorServer bonusBotConnectorServer, UserpanelCommandsHandler userpanelCommandsHandler)
        {
            bonusBotConnectorServer.CommandService.OnUsedCommand += CommandService_OnUsedCommand;

            _playerStatsHandler = ActivatorUtilities.CreateInstance<UserpanelPlayerStatsHandler>(serviceProvider);
            ApplicationUserHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationUserHandler>(serviceProvider);
            _rulesHandler = ActivatorUtilities.CreateInstance<UserpanelRulesHandler>(serviceProvider);
            _fAQsHandlers = ActivatorUtilities.CreateInstance<UserpanelFAQsHandlers>(serviceProvider);
            _commandsHandler = userpanelCommandsHandler;
            OfflineMessagesHandler = ActivatorUtilities.CreateInstance<UserpanelOfflineMessagesHandler>(serviceProvider);
            SettingsNormalHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsNormalHandler>(serviceProvider);
            SettingsSpecialHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsSpecialHandler>(serviceProvider);

            SupportRequestHandler = ActivatorUtilities.CreateInstance<UserpanelSupportRequestHandler>(serviceProvider);

            ApplicationsAdminHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationsAdminHandler>(serviceProvider, _playerStatsHandler, ApplicationUserHandler);
            SupportUserHandler = new UserpanelSupportUserHandler(SupportRequestHandler);
            SupportAdminHandler = new UserpanelSupportAdminHandler(SupportRequestHandler);
        }

        private string? CommandService_OnUsedCommand(ulong userId, string command)
        {
            return command switch
            {
                "confirmtds" => SettingsNormalHandler?.ConfirmDiscordUserId(userId) ?? "BonusBot-Connector is not started at server.",
                _ => null,
            };
        }

        public async void PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType dataType)
        {
            string? json = null;
            switch (dataType)
            {
                case UserpanelLoadDataType.Commands:
                    json = _commandsHandler.GetData();
                    break;

                case UserpanelLoadDataType.Rules:
                    json = _rulesHandler.GetData();
                    player.AddToChallenge(ChallengeType.ReadTheRules);
                    break;

                case UserpanelLoadDataType.FAQs:
                    json = _fAQsHandlers.GetData(player);
                    player.AddToChallenge(ChallengeType.ReadTheFAQ);
                    break;

                case UserpanelLoadDataType.MyStats:
                    json = await _playerStatsHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.ApplicationUser:
                    json = await ApplicationUserHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.ApplicationsAdmin:
                    json = await ApplicationsAdminHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SettingsSpecial:
                    json = SettingsSpecialHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SupportUser:
                    json = await SupportUserHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SupportAdmin:
                    json = await SupportAdminHandler.GetData(player);
                    break;
                case UserpanelLoadDataType.OfflineMessages:
                    json = await OfflineMessagesHandler.GetData(player);
                    break;
            }

            if (json == null)
                return;

            player.SendEvent(ToClientEvent.LoadUserpanelData, (int)dataType, json);
        }
    }
}
