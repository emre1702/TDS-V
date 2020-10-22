using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IDamageTestLobby : IFightLobby
    {
        new IDamageTestLobbyDeathmatch Deathmatch { get; }
    }
}
