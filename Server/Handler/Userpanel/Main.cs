﻿using BonusBotConnector_Server;
using GTANetworkAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelHandler : IUserpanelHandler
    {

        private readonly UserpanelCommandsHandler _commandsHandler;
        private readonly UserpanelFAQsHandlers _fAQsHandlers;
        private readonly UserpanelPlayerGeneralStatsHandler _playerStatsHandler;
        private readonly UserpanelRulesHandler _rulesHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public UserpanelHandler(IServiceProvider serviceProvider, BonusBotConnectorServer bonusBotConnectorServer,
            UserpanelCommandsHandler userpanelCommandsHandler, ITDSPlayerHandler tdsPlayerHandler)
        {
            _tdsPlayerHandler = tdsPlayerHandler;

            bonusBotConnectorServer.CommandService.OnUsedCommand += CommandService_OnUsedCommand;

            _playerStatsHandler = ActivatorUtilities.CreateInstance<UserpanelPlayerGeneralStatsHandler>(serviceProvider);
            ApplicationUserHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationUserHandler>(serviceProvider);
            _rulesHandler = ActivatorUtilities.CreateInstance<UserpanelRulesHandler>(serviceProvider);
            _fAQsHandlers = ActivatorUtilities.CreateInstance<UserpanelFAQsHandlers>(serviceProvider);
            _commandsHandler = userpanelCommandsHandler;
            SettingsCommandsHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsCommandsHandler>(serviceProvider);
            OfflineMessagesHandler = ActivatorUtilities.CreateInstance<UserpanelOfflineMessagesHandler>(serviceProvider);
            SettingsNormalHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsNormalHandler>(serviceProvider);
            SettingsSpecialHandler = ActivatorUtilities.CreateInstance<UserpanelSettingsSpecialHandler>(serviceProvider);

            SupportRequestHandler = ActivatorUtilities.CreateInstance<UserpanelSupportRequestHandler>(serviceProvider);

            ApplicationsAdminHandler = ActivatorUtilities.CreateInstance<UserpanelApplicationsAdminHandler>(serviceProvider, _playerStatsHandler, ApplicationUserHandler);
            PlayerWeaponStatsHandler = ActivatorUtilities.CreateInstance<UserpanelPlayerWeaponStatsHandler>(serviceProvider);

            SupportUserHandler = new UserpanelSupportUserHandler(SupportRequestHandler);
            SupportAdminHandler = new UserpanelSupportAdminHandler(SupportRequestHandler);

            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.LoadUserpanelData, this, OnLoadUserpanelData);
        }

        public IUserpanelApplicationsAdminHandler ApplicationsAdminHandler { get; }
        public IUserpanelApplicationUserHandler ApplicationUserHandler { get; }
        public IUserpanelOfflineMessagesHandler OfflineMessagesHandler { get; }
        public IUserpanelPlayerWeaponStatsHandler PlayerWeaponStatsHandler { get; }
        public IUserpanelPlayerCommandsHandler SettingsCommandsHandler { get; }
        public IUserpanelSettingsNormalHandler SettingsNormalHandler { get; }
        public IUserpanelSettingsSpecialHandler SettingsSpecialHandler { get; }
        public IUserpanelSupportAdminHandler SupportAdminHandler { get; }
        public IUserpanelSupportRequestHandler SupportRequestHandler { get; }
        public IUserpanelSupportUserHandler SupportUserHandler { get; }

        public void OnLoadUserpanelData(ITDSPlayer player, int dataType)
        {
            if (!player.LoggedIn)
                return;
            var type = (UserpanelLoadDataType)dataType;
            PlayerLoadData(player, type);
        }

        public async void PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType dataType)
        {
            try
            {
                string? json = null;
                switch (dataType)
                {
                    // Doing that at login now because it's also used for chat
                    /*case UserpanelLoadDataType.Commands:
                        json = _commandsHandler.GetData();
                        break;*/

                    case UserpanelLoadDataType.Rules:
                        json = _rulesHandler.GetData();
                        player.Challenges.AddToChallenge(ChallengeType.ReadTheRules);
                        break;

                    case UserpanelLoadDataType.FAQs:
                        json = _fAQsHandlers.GetData(player);
                        player.Challenges.AddToChallenge(ChallengeType.ReadTheFAQ);
                        break;

                    case UserpanelLoadDataType.MyStatsGeneral:
                        json = await _playerStatsHandler.GetData(player).ConfigureAwait(false);
                        break;

                    case UserpanelLoadDataType.MyStatsWeapon:
                        json = PlayerWeaponStatsHandler.GetData(player);
                        break;

                    case UserpanelLoadDataType.ApplicationUser:
                        json = await ApplicationUserHandler.GetData(player).ConfigureAwait(false);
                        break;

                    case UserpanelLoadDataType.ApplicationsAdmin:
                        json = await ApplicationsAdminHandler.GetData(player).ConfigureAwait(false);
                        break;

                    case UserpanelLoadDataType.SettingsSpecial:
                        json = SettingsSpecialHandler.GetData(player);
                        break;

                    // Doing that at clientside
                    /*case UserpanelLoadDataType.SettingsCommands:
                        json = await SettingsCommandsHandler.GetData(player);
                        break;*/

                    case UserpanelLoadDataType.SupportUser:
                        json = await SupportUserHandler.GetData(player).ConfigureAwait(false);
                        break;

                    case UserpanelLoadDataType.SupportAdmin:
                        json = await SupportAdminHandler.GetData(player).ConfigureAwait(false);
                        break;

                    case UserpanelLoadDataType.OfflineMessages:
                        json = await OfflineMessagesHandler.GetData(player).ConfigureAwait(false);
                        break;
                }

                if (json == null)
                    return;

                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadUserpanelData, (int)dataType, json));
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private async ValueTask CommandService_OnUsedCommand((ulong userId, string command, IList<string> args, BBUsedCommandReply reply) data)
        {
            switch (data.command)
            {
                case "ConfirmTDS":
                    var task = SettingsNormalHandler?.ConfirmDiscordUserId(data.userId);
                    if (task is { })
                        data.reply.Message = await task.ConfigureAwait(false);

                    data.reply.Message ??= "BonusBot-Connector is not started at server.";
                    break;
            }
        }

    }
}
