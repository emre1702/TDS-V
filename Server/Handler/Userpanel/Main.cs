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

        private readonly UserpanelApplicationsAdminHandler _applicationsAdminHandler;
        private readonly UserpanelApplicationUserHandler _applicationUserHandler;

        private readonly UserpanelCommandsHandler _commandsHandler;
        private readonly UserpanelRulesHandler _rulesHandler;
        private readonly UserpanelFAQsHandlers _fAQsHandlers;
        private readonly UserpanelPlayerStatsHandler _playerStatsHandler;

        private readonly UserpanelSupportUserHandler _supportUserHandler;
        private readonly UserpanelSupportAdminHandler _supportAdminHandler;
        public readonly UserpanelSettingsNormalHandler SettingsNormalHandler;
        private readonly UserpanelSettingsSpecialHandler _settingsSpecialHandler;
        private readonly UserpanelOfflineMessagesHandler _offlineMessagesHandler;

        public UserpanelHandler(IServiceProvider serviceProvider, BonusBotConnectorServer bonusBotConnectorServer)
        {
            bonusBotConnectorServer.CommandService.OnUsedCommand += CommandService_OnUsedCommand;

            _playerStatsHandler = ActivatorUtilities.CreateInstance<UserpanelPlayerStatsHandler>(serviceProvider);
            _applicationUserHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationUserHandler>(serviceProvider);
            _rulesHandler = ActivatorUtilities.CreateInstance<UserpanelRulesHandler>(serviceProvider);
            _fAQsHandlers = ActivatorUtilities.CreateInstance<UserpanelFAQsHandlers>(serviceProvider);
            _commandsHandler = ActivatorUtilities.CreateInstance<UserpanelCommandsHandler>(serviceProvider);
            _offlineMessagesHandler = ActivatorUtilities.CreateInstance<UserpanelOfflineMessagesHandler>(serviceProvider);
            SettingsNormalHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsNormalHandler>(serviceProvider);
            _settingsSpecialHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsSpecialHandler>(serviceProvider);

            var userpanelSupportRequestHandler = ActivatorUtilities.CreateInstance<UserpanelSupportRequestHandler>(serviceProvider);

            _applicationsAdminHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationsAdminHandler>(serviceProvider, _playerStatsHandler);
            _supportUserHandler = new UserpanelSupportUserHandler(userpanelSupportRequestHandler);
            _supportAdminHandler = new UserpanelSupportAdminHandler(userpanelSupportRequestHandler);
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
                    json = await _applicationUserHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.ApplicationsAdmin:
                    json = await _applicationsAdminHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SettingsSpecial:
                    json = _settingsSpecialHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SupportUser:
                    json = await _supportUserHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.SupportAdmin:
                    json = await _supportAdminHandler.GetData(player);
                    break;
                case UserpanelLoadDataType.OfflineMessages:
                    json = await _offlineMessagesHandler.GetData(player);
                    break;
            }

            if (json == null)
                return;

            player.SendEvent(ToClientEvent.LoadUserpanelData, (int)dataType, json);
        }
    }
}
