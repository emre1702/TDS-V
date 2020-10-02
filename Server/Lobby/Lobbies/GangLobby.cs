using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class GangLobby : FreeroamLobby, IGangLobby
    {
        private readonly List<IGangActionLobby> _gangActionLobbies = new List<IGangActionLobby>();

        public GangLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler)
        {
        }

        public void AddGangActionLobby(IGangActionLobby lobby)
        {
            lobby.Events.RemoveAfter += GangActionLobbyRemoved;
            lock (_gangActionLobbies)
            {
                _gangActionLobbies.Add(lobby);
            }
        }

        private void GangActionLobbyRemoved(IBaseLobby lobby)
        {
            if (!(lobby is IGangActionLobby gangActionLobby))
                return;
            lock (_gangActionLobbies)
            {
                _gangActionLobbies.Remove(gangActionLobby);
            }
        }

        public void DoForGangActionLobbies(Action<IGangActionLobby> action)
        {
            lock (_gangActionLobbies)
            {
                foreach (var lobby in _gangActionLobbies)
                    action(lobby);
            }
        }
    }
}
