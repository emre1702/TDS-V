using TDS_Client.Data.Interfaces.RAGE.Game.Discord;

namespace TDS_Client.RAGEAPI.Discord
{
    internal class DiscordAPI : IDiscordAPI
    {
        #region Public Methods

        public void Update(string details, string state)
        {
            RAGE.Discord.Update(details, state);
        }

        #endregion Public Methods
    }
}