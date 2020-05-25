using BonusBotConnector.Client;
using System;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Entities.Utility;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Server;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class Arena : FightLobby
    {
        #region Private Fields

        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly ServerStatsHandler _serverStatsHandler;
        private readonly IServiceProvider _serviceProvider;
        private bool _dontRemove;

        #endregion Private Fields

        #region Public Constructors

        public Arena(Lobbies entity, bool isGangActionLobby, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, MapsLoadingHandler mapsLoadingHandler, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ServerStatsHandler serverStatsHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : base(entity, isGangActionLobby, dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, eventsHandler,
                  weaponDatasLoadingHandler, bonusBotConnectorClient, bansHandler)
        {
            _serviceProvider = serviceProvider;
            _mapsLoadingHandler = mapsLoadingHandler;
            _serverStatsHandler = serverStatsHandler;

            _roundStatusMethod[RoundStatus.MapClear] = StartMapClear;
            _roundStatusMethod[RoundStatus.NewMapChoose] = StartNewMapChoose;
            _roundStatusMethod[RoundStatus.Countdown] = StartRoundCountdown;
            _roundStatusMethod[RoundStatus.Round] = StartRound;
            _roundStatusMethod[RoundStatus.RoundEnd] = EndRound;
            _roundStatusMethod[RoundStatus.RoundEndRanking] = ShowRoundRanking;

            DurationsDict[RoundStatus.Round] = (uint)entity.LobbyRoundSettings.RoundTime * 1000;
            DurationsDict[RoundStatus.Countdown] = (uint)entity.LobbyRoundSettings.CountdownTime * 1000;

            if (!entity.LobbyRoundSettings.ShowRanking)
            {
                _nextRoundStatsDict[RoundStatus.RoundEnd] = RoundStatus.MapClear;
            }
        }

        public Arena(Lobbies entity, GangwarArea gangwarArea, bool removeAfterOneRound, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI,
            LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, MapsLoadingHandler mapsLoadingHandler,
            EventsHandler eventsHandler, IServiceProvider serviceProvider, ServerStatsHandler serverStatsHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : this(entity, true, dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, mapsLoadingHandler, eventsHandler,
                  serviceProvider, serverStatsHandler, weaponDatasLoadingHandler, bonusBotConnectorClient, bansHandler)
        {
            IsGangActionLobby = true;
            RemoveAfterOneRound = removeAfterOneRound;

            GangwarArea = gangwarArea;
            gangwarArea.InLobby = this;
        }

        #endregion Public Constructors

        #region Public Properties

        public GangwarArea? GangwarArea { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Start()
        {
            base.Start();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override async Task Remove()
        {
            _nextRoundStatusTimer?.Kill();
            _nextRoundStatusTimer = null;

            _dontRemove = true;
            EndRound();
            ModAPI.Thread.RunInMainThread(() => StartMapClear());
            RoundEndReasonText = null;

            if (GangwarArea is { })
            {
                GangwarArea.InLobby = null;
                GangwarArea = null;
            }

            await base.Remove();
        }

        #endregion Protected Methods
    }
}
