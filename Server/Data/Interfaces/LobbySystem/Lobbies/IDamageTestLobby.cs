using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IDamageTestLobby : IFightLobby
    {
        new IDamageTestLobbyDeathmatch Deathmatch { get; }
    }
}
