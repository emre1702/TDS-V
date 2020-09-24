using TDS_Server.Data.Interfaces.Entities.Gamemodes;

namespace TDS_Server.LobbySystem.GamemodesHandlers
{
    public class RoundFightLobbyGamemodesHandler
    {
        public IGamemode? CurrentGamemode { get; private set; }
    }
}
