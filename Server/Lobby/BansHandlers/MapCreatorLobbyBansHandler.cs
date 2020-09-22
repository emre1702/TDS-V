﻿using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.BansHandlers
{
    public class MapCreatorLobbyBansHandler : BaseLobbyBansHandler
    {
        public MapCreatorLobbyBansHandler(BaseLobbyDatabase database, IBaseLobbyEventsHandler events, LangHelper langHelper, BaseLobbyChat chat, LobbyDb entity)
            : base(database, events, langHelper, chat, entity)
        {
        }

        // We got a dummy lobby official lobby for this check.
        public override ValueTask<bool> CheckIsBanned(ITDSPlayer player)
            => Entity.IsOfficial ? base.CheckIsBanned(player) : new ValueTask<bool>(false);
    }
}
