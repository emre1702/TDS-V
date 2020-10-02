using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Sounds;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Sounds
{
    public class BaseLobbySoundsHandler : IBaseLobbySoundsHandler
    {
        private readonly IBaseLobby _lobby;

        public BaseLobbySoundsHandler(IBaseLobby lobby)
            => _lobby = lobby;

        public void PlaySound(string soundName)
        {
            _lobby.Sync.TriggerEvent(ToClientEvent.PlayCustomSound, soundName);
        }
    }
}
