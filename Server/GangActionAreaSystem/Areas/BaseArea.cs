using BonusBotConnector.Client;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Action;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Database;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Events;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.GangsHandler;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.LobbyHandlers;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.MapHandlers;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Notifications;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.StartRequirements;
using TDS.Server.Data.Interfaces.LobbySystem;
using TDS.Server.Data.Models.Map;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.GangActionAreaSystem.Action;
using TDS.Server.GangActionAreaSystem.Database;
using TDS.Server.GangActionAreaSystem.DependenciesModels;
using TDS.Server.GangActionAreaSystem.Events;
using TDS.Server.GangActionAreaSystem.GangsHandlers;
using TDS.Server.GangActionAreaSystem.LobbyHandlers;
using TDS.Server.GangActionAreaSystem.MapHandlers;
using TDS.Server.GangActionAreaSystem.Notifications;
using TDS.Server.GangActionAreaSystem.StartRequirements;
using TDS.Server.Handler;
using TDS.Server.Handler.GangSystem;
using TDS.Server.Handler.Helper;

namespace TDS.Server.GangActionAreaSystem.Areas
{
    internal abstract class BaseArea : IBaseGangActionArea
    {
        public abstract GangActionType Type { get; }

#nullable disable
        public IBaseGangActionAreaAction Action { get; private set; }
        public IBaseGangActionAreaDatabase DatabaseHandler { get; private set; }
        public IBaseGangActionAreaEvents Events { get; private set; }
        public IBaseGangActionAreaGangsHandler GangsHandler { get; private set; }
        public IBaseGangActionAreaLobbyHandler LobbyHandler { get; private set; }
        public IBaseGangActionAreaMapHandler MapHandler { get; private set; }
        public IBaseGangActionAreaNotifications Notifications { get; private set; }
        public IBaseGangActionAreaStartRequirements StartRequirements { get; private set; }
#nullable restore

        protected IDatabaseHandler Database { get; }
        protected GangsHandler GlobalGangsHandler { get; }
        protected LobbiesHandler GlobalLobbiesHandler { get; }
        protected LangHelper LangHelper { get; }
        protected ILobbiesProvider LobbiesProvider { get; }
        protected ISettingsHandler SettingsHandler { get; }
        protected BonusBotConnectorClient BonusBotConnectorClient { get; }

        protected BaseArea(IDatabaseHandler database, GangsHandler gangsHandler, LobbiesHandler lobbiesHandler, LangHelper langHelper,
            ILobbiesProvider lobbiesProvider, ISettingsHandler settingsHandler, BonusBotConnectorClient bonusBotConnectorClient)
        {
            Database = database;
            GlobalGangsHandler = gangsHandler;
            GlobalLobbiesHandler = lobbiesHandler;
            LangHelper = langHelper;
            LobbiesProvider = lobbiesProvider;
            SettingsHandler = settingsHandler;
            BonusBotConnectorClient = bonusBotConnectorClient;

            InitDependencies(null);
        }

        public void Init(MapDto map, GangActionAreas? entity)
        {
            if (entity is { })
            {
                DatabaseHandler.Init(entity);
                GangsHandler.Init(entity);
            }

            Action.Init(this);                
            LobbyHandler.Init(this);
            MapHandler.Init(this, map);
            Notifications.Init(this);
            StartRequirements.Init(this);
        }

        protected virtual void InitDependencies(BaseAreaDependencies? d)
        {
            d ??= new BaseAreaDependencies();

            d.Action ??= new BaseAreaAction();
            d.DatabaseHandler ??= new BaseAreaDatabase(Database);
            d.Events ??= new BaseAreaEvents();
            d.GangsHandler ??= new BaseAreaGangsHandler(GlobalGangsHandler);
            d.LobbyHandler ??= new BaseAreaLobbyHandler(GlobalLobbiesHandler, SettingsHandler, LobbiesProvider);
            d.MapHandler ??= new BaseAreaMapHandler(GlobalLobbiesHandler);
            d.Notifications ??= new BaseAreaNotifications(BonusBotConnectorClient, LangHelper);
            d.StartRequirements ??= new BaseAreaStartRequirements(SettingsHandler);

            DatabaseHandler = d.DatabaseHandler;
            LobbyHandler = d.LobbyHandler;
        }

        public override string ToString()
        {
            var mapName = DatabaseHandler?.Entity?.Map.Name ?? "Unknown";
            return $"'{mapName}' ({Type})";
        }
    }
}
