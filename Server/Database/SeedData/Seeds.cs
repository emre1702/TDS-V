using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.SeedData.Admin;
using TDS_Server.Database.SeedData.BonusBot;
using TDS_Server.Database.SeedData.Challenge;
using TDS_Server.Database.SeedData.Command;
using TDS_Server.Database.SeedData.Gang;
using TDS_Server.Database.SeedData.Lobby;
using TDS_Server.Database.SeedData.Player;
using TDS_Server.Database.SeedData.Rest;
using TDS_Server.Database.SeedData.Server;
using TDS_Server.Database.SeedData.Userpanel;

namespace TDS_Server.Database.SeedData
{
    public static class Seeds
    {
        public static ModelBuilder HasSeeds(this ModelBuilder modelBuilder)
        {
            return modelBuilder
                .HasAdminLevels()
                .HasAdminLevelNames()
                
                .HasBonusBotSettings()

                .HasChallengeSettings()

                .HasCommands()
                .HasCommandInfos()
                .HasCommandAliases()

                .HasGangs()
                .HasGangRanks()
                .HasGangRankPermissions()

                .HasLobbies()
                .HasLobbyArmsRaceWeapons()
                .HasLobbyFightSettings()
                .HasLobbyKillingspreeRewards()
                .HasLobbyMapSettings()
                .HasLobbyMaps()
                .HasLobbyRewards()
                .HasLobbyRoundSettings()
                .HasLobbyWeapons()

                .HasPlayers()

                .HasChatInfos()
                .HasFreeroamDefaultVehicles()
                .HasMaps()
                .HasTeams()
                .HasWeapons()

                .HasServerSettings()
                .HasServerTotalStats()
                
                .HasFAQs()
                .HasRules()
                .HasRuleTexts();
        }
    }
}
