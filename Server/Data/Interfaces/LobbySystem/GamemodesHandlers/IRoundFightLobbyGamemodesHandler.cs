using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;

namespace TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers
{
    public interface IRoundFightLobbyGamemodesHandler
    {
        IBaseGamemode CurrentGamemode { get; }
    }
}
