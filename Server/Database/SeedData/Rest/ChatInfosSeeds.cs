using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.SeedData.Rest
{
    public static class ChatInfosSeeds
    {
        public static ModelBuilder HasChatInfos(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatInfos>().HasData(
                new ChatInfos
                {
                    Id = 1,
                    Language = Language.German,
                    Message = "Du kannst die Lobby mit \"/leave\" verlassen."
                },
                new ChatInfos
                {
                    Id = 2,
                    Language = Language.English,
                    Message = "You can leave the lobby with \"/leave.\""
                },
                new ChatInfos
                {
                    Id = 3,
                    Language = Language.German,
                    Message = "VIPs sind keine Spender"
                },
                new ChatInfos
                {
                    Id = 4,
                    Language = Language.English,
                    Message = "VIPs are not donators"
                },
                new ChatInfos
                {
                    Id = 5,
                    Language = Language.German,
                    Message = "Es gibt 3 Admin-Ränge: Supporter, Administrator, Projektleiter"
                },
                new ChatInfos
                {
                    Id = 6,
                    Language = Language.English,
                    Message = "There are 3 admin ranks: Supporter, Administrator, Project Leader"
                },
                new ChatInfos
                {
                    Id = 7,
                    Language = Language.German,
                    Message = "Der Projektleiter ernennt Administratoren. Die Administratoren ernennen Supporter."
                },
                new ChatInfos
                {
                    Id = 8,
                    Language = Language.English,
                    Message = "The project leader appoints administrators. The administrators appoint supporters."
                }
            );
            return modelBuilder;
        }

    }
}
