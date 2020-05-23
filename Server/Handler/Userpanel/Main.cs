using BonusBotConnector_Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelHandler : IUserpanelHandler
    {
        public IUserpanelApplicationsAdminHandler ApplicationsAdminHandler { get; }
        public IUserpanelApplicationUserHandler ApplicationUserHandler { get; }

        private readonly UserpanelCommandsHandler _commandsHandler;
        private readonly UserpanelRulesHandler _rulesHandler;
        private readonly UserpanelFAQsHandlers _fAQsHandlers;
        private readonly UserpanelPlayerStatsHandler _playerStatsHandler;

        public IUserpanelSupportUserHandler SupportUserHandler { get; }
        public IUserpanelSupportAdminHandler SupportAdminHandler { get; }
        public IUserpanelSupportRequestHandler SupportRequestHandler { get; }
        public IUserpanelSettingsNormalHandler SettingsNormalHandler { get; }
        public IUserpanelSettingsSpecialHandler SettingsSpecialHandler { get; }
        public IUserpanelOfflineMessagesHandler OfflineMessagesHandler { get; }

        private readonly IModAPI _modAPI;

        public UserpanelHandler(IServiceProvider serviceProvider, BonusBotConnectorServer bonusBotConnectorServer, 
            UserpanelCommandsHandler userpanelCommandsHandler, IModAPI modAPI)
        {
            _modAPI = modAPI;

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

        private async ValueTask CommandService_OnUsedCommand((ulong userId, string command, IList<string> args, BBUsedCommandReply reply) data)
        {
            switch (data.command)
            {
                case "ConfirmTDS":
                    var task = SettingsNormalHandler?.ConfirmDiscordUserId(data.userId);
                    if (task is { })
                        data.reply.Message = await task;

                    data.reply.Message ??= "BonusBot-Connector is not started at server.";
                    break;
            }
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

            _modAPI.Thread.RunInMainThread(() => player.SendEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadUserpanelData, (int)dataType, json));
        }
    }
}
