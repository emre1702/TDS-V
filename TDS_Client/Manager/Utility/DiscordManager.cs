using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Manager.Lobby;

namespace TDS_Client.Manager.Utility
{
    class DiscordManager
    {
        public static void Update()
        {
            RAGE.Discord.Update($"TDS-V - {Settings.LobbyName}", $"{Team.CurrentTeamName}");
        }
    }
}
