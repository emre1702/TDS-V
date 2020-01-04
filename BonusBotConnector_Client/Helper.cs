﻿using System;
using System.Collections.Generic;
using TDS_Server_DB.Entity.Player;

namespace BonusBotConnector_Client
{
    public class Helper
    {
        public static List<EmbedField> GetBanEmbedFields(PlayerBans ban)
        {
           var list = new List<EmbedField>
           {
               new EmbedField { Name = "Location:", Value = ban.Lobby.Type == TDS_Common.Enum.ELobbyType.MainMenu ? "Server" : ban.Lobby.Name },
               new EmbedField { Name = "Admin:", Value = $"{ban.Admin.Name} ({ban.Admin.SCName})" },
               new EmbedField { Name = "Reason:", Value = ban.Reason },
               new EmbedField { Name = "Started:", Value = ban.StartTimestamp.ToString() },
               new EmbedField { Name = "Ends:", Value = ban.EndTimestamp?.ToString() ?? "-" }
           };

            return list;
        }
    }
}