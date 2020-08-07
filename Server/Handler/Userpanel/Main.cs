using BonusBotConnector_Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Handler.Player;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Userpanel
{
    public class UserpanelHandler : IUserpanelHandler
    {
        #region Private Fields

        private readonly UserpanelCommandsHandler _commandsHandler;
        private readonly UserpanelFAQsHandlers _fAQsHandlers;
        private readonly IModAPI _modAPI;
        private readonly UserpanelPlayerGeneralStatsHandler _playerStatsHandler;
        private readonly UserpanelRulesHandler _rulesHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelHandler(IServiceProvider serviceProvider, BonusBotConnectorServer bonusBotConnectorServer,
            UserpanelCommandsHandler userpanelCommandsHandler, IModAPI modAPI, ITDSPlayerHandler tdsPlayerHandler)
        {
            _modAPI = modAPI;
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

            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.LoadUserpanelData, this, OnLoadUserpanelData);
        }

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        public void OnLoadUserpanelData(IPlayer modPlayer, int dataType)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;

            var type = (UserpanelLoadDataType)dataType;
            PlayerLoadData(player, type);
        }

        public async void PlayerLoadData(ITDSPlayer player, UserpanelLoadDataType dataType)
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
                    player.AddToChallenge(ChallengeType.ReadTheRules);
                    break;

                case UserpanelLoadDataType.FAQs:
                    json = _fAQsHandlers.GetData(player);
                    player.AddToChallenge(ChallengeType.ReadTheFAQ);
                    break;

                case UserpanelLoadDataType.MyStatsGeneral:
                    json = await _playerStatsHandler.GetData(player);
                    break;

                case UserpanelLoadDataType.MyStatsWeapon:
                    json = PlayerWeaponStatsHandler.GetData(player);
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

                // Doing that at clientside
                /*case UserpanelLoadDataType.SettingsCommands:
                    json = await SettingsCommandsHandler.GetData(player);
                    break;*/

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

            _modAPI.Thread.QueueIntoMainThread(() => player.SendEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadUserpanelData, (int)dataType, json));
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}
