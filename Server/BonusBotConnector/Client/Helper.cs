using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Player;

namespace BonusBotConnector.Client
{
    public class Helper
    {
        public List<EmbedField> GetBanEmbedFields(PlayerBans ban)
        {
           var list = new List<EmbedField>
           {
               new EmbedField { Name = "Location:", Value = ban.Lobby.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu ? "Server" : ban.Lobby.Name },
               new EmbedField { Name = "Admin:", Value = $"{ban.Admin.Name} ({ban.Admin.SCName})" },
               new EmbedField { Name = "Reason:", Value = ban.Reason },
               new EmbedField { Name = "Started:", Value = ban.StartTimestamp.ToString() },
               new EmbedField { Name = "Ends:", Value = ban.EndTimestamp?.ToString() ?? "-" }
           };

            return list;
        }
    }
}
