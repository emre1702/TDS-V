using BonusBotConnector.Client;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class Lobby : DatabaseEntityWrapper, ILobby
    {
        protected readonly BansHandler BansHandler;
        protected readonly BonusBotConnectorClient BonusBotConnectorClient;
        protected readonly DataSyncHandler DataSyncHandler;
        protected readonly EventsHandler EventsHandler;
        protected readonly LangHelper LangHelper;
        protected readonly LobbiesHandler LobbiesHandler;
        protected readonly Serializer Serializer;
        protected readonly ISettingsHandler SettingsHandler;
        protected SyncedLobbySettings SyncedLobbySettings;

        public Lobby(
            Lobbies entity,
            bool isGangActionLobby,

            TDSDbContext dbContext,
            ILoggingHandler loggingHandler,
            Serializer serializer,
            LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler,
            LangHelper langHelper,
            DataSyncHandler dataSyncHandler,
            EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient,
            BansHandler bansHandler) : base(dbContext, loggingHandler)
        {
            Serializer = serializer;
            LobbiesHandler = lobbiesHandler;
            SettingsHandler = settingsHandler;
            LangHelper = langHelper;
            DataSyncHandler = dataSyncHandler;
            EventsHandler = eventsHandler;
            BonusBotConnectorClient = bonusBotConnectorClient;
            BansHandler = bansHandler;

            eventsHandler.PlayerLoggedOutBefore += OnPlayerLoggedOut;
        }

        public string CreatorName => Entity.Owner?.Name ?? "?";

        public bool IsGangActionLobby { get; set; }
        public string OwnerName => CreatorName;
        public int StartTotalHP => (Entity.FightSettings?.StartArmor ?? 100) + (Entity.FightSettings?.StartHealth ?? 100);
        public int Id => Entity.Id;
        public bool IsOfficial => Entity.IsOfficial;
        public string Name => Entity.Name;
        public LobbyType Type => Entity.Type;

        public virtual void Start()
        {
        }

        protected bool IsEmpty()
        {
            return Players.Count == 0;
        }

        protected async virtual Task Remove()
        {
            LobbiesHandler.RemoveLobby(this);

            foreach (var player in Players.Values.ToList())
            {
                await RemovePlayer(player);
            }

            await ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Remove(Entity);
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
