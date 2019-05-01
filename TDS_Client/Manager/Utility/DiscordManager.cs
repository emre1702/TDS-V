using TDS_Client.Manager.Lobby;

namespace TDS_Client.Manager.Utility
{
    internal class DiscordManager
    {
        public static void Update()
        {
            RAGE.Discord.Update($"TDS-V - {Settings.LobbyName}", Team.CurrentTeamName);
        }
    }
}