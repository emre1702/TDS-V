using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers;

namespace TDS_Server.LobbySystem.GamemodesHandlers
{
    public class RoundFightLobbyGamemodesHandler : IRoundFightLobbyGamemodesHandler
    {
        public IBaseGamemode? CurrentGamemode { get; private set; }
    }
}
