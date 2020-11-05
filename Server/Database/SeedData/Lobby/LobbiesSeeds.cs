using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.SeedData.Lobby
{
    public static class LobbiesSeeds
    {
        public static ModelBuilder HasLobbies(this ModelBuilder modelBuilder)
        {
            var seedLobbies = new List<Lobbies> {
                new Lobbies { Id = -4, OwnerId = -1, Type = LobbyType.MainMenu, Name = "MainMenu", IsTemporary = false, IsOfficial = true },
                new Lobbies { Id = -1, OwnerId = -1, Type = LobbyType.Arena, Name = "Arena", IsTemporary = false, IsOfficial = true },

                new Lobbies
                {
                    Id = -2, OwnerId = -1, Type = LobbyType.GangLobby, Name = "GangLobby", IsTemporary = false, IsOfficial = true,
                    DefaultSpawnX =  -365.425f, DefaultSpawnY = -131.809f, DefaultSpawnZ = 37.873f, DefaultSpawnRotation = 0f
                },

                // only for map-creator ban & spawn
                new Lobbies
                {
                    Id = -3, OwnerId = -1, Type = LobbyType.MapCreateLobby, Name = "MapCreateLobby", IsTemporary = false, IsOfficial = true,
                    DefaultSpawnX = -365.425f, DefaultSpawnY = -131.809f, DefaultSpawnZ = 37.873f, DefaultSpawnRotation = 0f
                },

                // only for char-creator ban & spawn
                new Lobbies
                {
                    Id = -5, OwnerId = -1, Type = LobbyType.CharCreateLobby, Name = "CharCreateLobby", IsTemporary = false, IsOfficial = true,
                    DefaultSpawnX = -425.2233f, DefaultSpawnY = 1126.9731f, DefaultSpawnZ = 326.8f, DefaultSpawnRotation = 0f
                },

                // only for damage-test spawn
                new Lobbies
                {
                    Id = -6, OwnerId = -1, Type = LobbyType.DamageTestLobby, Name = "DamageTestLobby", IsTemporary = false, IsOfficial = true,
                    DefaultSpawnX = -365.425f, DefaultSpawnY = -131.809f, DefaultSpawnZ = 37.873f, DefaultSpawnRotation = 0f,
                }
            };
            modelBuilder.Entity<Lobbies>().HasData(seedLobbies);
            return modelBuilder;
        }
    }
}
