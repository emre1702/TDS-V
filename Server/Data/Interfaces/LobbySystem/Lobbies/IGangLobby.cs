using System;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IGangLobby : IBaseLobby
    {
        void AddGangActionLobby(IGangActionLobby lobby);

        void DoForGangActionLobbies(Action<IGangActionLobby> action);
    }
}
