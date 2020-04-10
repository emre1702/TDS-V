using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler
{
    public class DiscordHandler
    {
        private readonly IModAPI _modAPI;

        public DiscordHandler(IModAPI modAPI)
            => _modAPI = modAPI;

        public void Update(string lobbyName, string teamName)
        {
            _modAPI.Discord.Update($"TDS-V - {lobbyName}", teamName);
        }
    }
}
