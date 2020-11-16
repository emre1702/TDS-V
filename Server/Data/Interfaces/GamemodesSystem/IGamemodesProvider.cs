using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.Map;

namespace TDS.Server.Data.Interfaces.GamemodesSystem
{
    public interface IGamemodesProvider
    {
        IBaseGamemode Create(IRoundFightLobby lobby, MapDto map);

        TGamemode Create<TGamemode>(IRoundFightLobby lobby, MapDto map) where TGamemode : IBaseGamemode;
    }
}
