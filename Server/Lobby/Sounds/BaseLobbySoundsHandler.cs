using TDS_Server.LobbySystem.Sync;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Sounds
{
    public class BaseLobbySoundsHandler
    {
        private readonly BaseLobbySync _sync;

        public BaseLobbySoundsHandler(BaseLobbySync sync)
            => _sync = sync;

        public void PlaySound(string soundName)
        {
            _sync.TriggerEvent(ToClientEvent.PlayCustomSound, soundName);
        }
    }
}
