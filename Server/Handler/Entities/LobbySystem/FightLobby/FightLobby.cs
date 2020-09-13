using BonusBotConnector.Client;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class FightLobby : Lobby
    {
        public readonly Damagesys DmgSys;

        public FightLobby(Lobbies entity, bool isGangActionLobby, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler, WeaponDatasLoadingHandler weaponDatasLoadingHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : base(entity, isGangActionLobby, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper,
                  dataSyncHandler, eventsHandler, bonusBotConnectorClient, bansHandler)
        {
            AmountLifes = Entity.FightSettings?.AmountLifes ?? 0;
            DmgSys = new Damagesys(entity.LobbyWeapons, entity.LobbyKillingspreeRewards, loggingHandler, weaponDatasLoadingHandler);
        }
    }
}
