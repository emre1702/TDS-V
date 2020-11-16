using System;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IGangLobby : IBaseLobby
    {
        void AddGangActionLobby(IGangActionLobby lobby);

        void DoForGangActionLobbies(Action<IGangActionLobby> action);
    }
}
