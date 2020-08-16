using AltV.Net.Async;
using BonusBotConnector.Client;
using System;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Entity.LobbySystem.FightLobbySystem;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Server;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;

namespace TDS_Server.Entity.LobbySystem.ArenaSystem
{
    public partial class Arena : FightLobby, IArena
    {
        #region Private Fields

        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly ServerStatsHandler _serverStatsHandler;
        private readonly TDSBlipHandler _tdsBlipHandler;

        private bool _dontRemove;

        #endregion Private Fields

        #region Public Constructors

        public Arena(Lobbies entity, bool isGangActionLobby, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, MapsLoadingHandler mapsLoadingHandler, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ServerStatsHandler serverStatsHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, TDSBlipHandler tdsBlipHandler, IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            : base(entity, isGangActionLobby, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper, eventsHandler,
                  weaponDatasLoadingHandler, bonusBotConnectorClient, bansHandler, serviceProvider, entitiesByInterfaceCreator)
        {
            _mapsLoadingHandler = mapsLoadingHandler;
            _serverStatsHandler = serverStatsHandler;
            _tdsBlipHandler = tdsBlipHandler;

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

        public Arena(Lobbies entity, IGangwarArea gangwarArea, bool removeAfterOneRound, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, LangHelper langHelper, MapsLoadingHandler mapsLoadingHandler,
            EventsHandler eventsHandler, IServiceProvider serviceProvider, ServerStatsHandler serverStatsHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, TDSBlipHandler tdsBlipHandler, IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            : this(entity, true, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper, mapsLoadingHandler, eventsHandler,
                  serviceProvider, serverStatsHandler, weaponDatasLoadingHandler, bonusBotConnectorClient, bansHandler, tdsBlipHandler, entitiesByInterfaceCreator)
        {
            IsGangActionLobby = true;
            RemoveAfterOneRound = removeAfterOneRound;

            GangwarArea = gangwarArea;
            gangwarArea.InLobby = this;
        }

        #endregion Public Constructors

        #region Public Properties

        public IGangwarArea? GangwarArea { get; set; }

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
            await AltAsync.Do(StartMapClear);
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
