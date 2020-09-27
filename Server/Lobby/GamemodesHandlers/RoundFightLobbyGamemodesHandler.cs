using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers;

namespace TDS_Server.LobbySystem.GamemodesHandlers
{
    public class RoundFightLobbyGamemodesHandler : IRoundFightLobbyGamemodesHandler
    {
        public IGamemode? CurrentGamemode { get; private set; }
    }
}
