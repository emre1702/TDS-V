﻿using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Chat;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Chat
{
    class ChatAPI : IChatAPI
    {
        public ChatAPI()
        {

        }

        public void SendMessage(string message, ICollection<int>? ignorePlayersWithId = null)
        {
            foreach (var player in Program.TDSCore.LoggedInPlayers)
            {
                if ((ignorePlayersWithId is null || !ignorePlayersWithId.Contains(player.Id)) && player.ModPlayer is Player.Player modPlayer)
                    modPlayer._instance.SendChatMessage(message);
            }
        }

        public void SendMessage(ILobby lobby, string message, ICollection<int>? ignorePlayersWithId = null)
        {
            foreach (var player in lobby.Players)
            {
                if ((ignorePlayersWithId is null || !ignorePlayersWithId.Contains(player.Id)) && player.ModPlayer is Player.Player modPlayer)
                    modPlayer._instance.SendChatMessage(message);
            }
        }
    }
}