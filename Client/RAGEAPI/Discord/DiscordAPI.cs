using TDS_Client.Data.Interfaces.ModAPI.Discord;

namespace TDS_Client.RAGEAPI.Discord
{
    class DiscordAPI : IDiscordAPI
    {
        public void Update(string details, string state)
        {
            RAGE.Discord.Update(details, state);
        }
    }
}
