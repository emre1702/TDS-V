﻿using GTANetworkAPI;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(GTANetworkAPI.Player player)
        {
            (Program.BaseAPI.Player as PlayerAPI)?.PlayerConnected(player);

            var modPlayer = Program.GetModPlayer(player);
            if (modPlayer is null)
                return;

            Program.TDSCore.EventsHandler.OnPlayerConnected(modPlayer);
        }
    }
}