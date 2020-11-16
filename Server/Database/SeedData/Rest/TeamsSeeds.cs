using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.SeedData.Rest
{
    public static class TeamsSeeds
    {
        public static ModelBuilder HasTeams(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teams>().HasData(
                new Teams { Id = -1, Index = 0, Name = "Spectator", Lobby = -4, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 0 /*1004114196*/ },
                new Teams { Id = -2, Index = 0, Name = "Spectator", Lobby = -1, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 0 },
                new Teams { Id = -3, Index = 1, Name = "SWAT", Lobby = -1, ColorR = 0, ColorG = 150, ColorB = 0, BlipColor = 52, SkinHash = 0 /*-1920001264*/ },
                new Teams { Id = -4, Index = 2, Name = "Terrorist", Lobby = -1, ColorR = 150, ColorG = 0, ColorB = 0, BlipColor = 1, SkinHash = 0 /*275618457*/ },
                new Teams { Id = -5, Index = 0, Name = "None", Lobby = -2, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 0 }
            );
            return modelBuilder;
        }
    }
}
