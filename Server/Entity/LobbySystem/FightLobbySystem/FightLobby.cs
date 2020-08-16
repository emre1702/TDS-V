using BonusBotConnector.Client;
using System;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Entity.Damagesys;
using TDS_Server.Entity.LobbySystem.BaseSystem;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;

namespace TDS_Server.Entity.LobbySystem.FightLobbySystem
{
    public partial class FightLobby : Lobby, IFightLobby
    {
        #region Public Fields

        public IDamageSystem DmgSys { get; }

        #endregion Public Fields

        #region Public Constructors

        public FightLobby(Lobbies entity, bool isGangActionLobby, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, EventsHandler eventsHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, IServiceProvider serviceProvider, IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            : base(entity, isGangActionLobby, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper,
                  eventsHandler, bonusBotConnectorClient, bansHandler, serviceProvider, entitiesByInterfaceCreator)
        {
            AmountLifes = Entity.FightSettings?.AmountLifes ?? 0;
            DmgSys = new DamageSystem(entity.LobbyWeapons, entity.LobbyKillingspreeRewards, loggingHandler, weaponDatasLoadingHandler);
        }

        #endregion Public Constructors
    }
}
