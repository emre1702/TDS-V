using TDS_Server.Data.Interfaces.Entities.Gamemodes;

namespace TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers
{
    public interface IRoundFightLobbyGamemodesHandler
    {
        IGamemode CurrentGamemode { get; }
    }
}
