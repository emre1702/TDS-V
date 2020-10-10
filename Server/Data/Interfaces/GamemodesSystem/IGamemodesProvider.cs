using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.Data.Interfaces.GamemodesSystem
{
    public interface IGamemodesProvider
    {
        IBaseGamemode Create(IRoundFightLobby lobby, MapDto map);

        TGamemode Create<TGamemode>(IRoundFightLobby lobby, MapDto map) where TGamemode : IBaseGamemode;
    }
}
