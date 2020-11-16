using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.SeedData.Admin;
using TDS.Server.Database.SeedData.BonusBot;
using TDS.Server.Database.SeedData.Challenge;
using TDS.Server.Database.SeedData.Command;
using TDS.Server.Database.SeedData.Gang;
using TDS.Server.Database.SeedData.Lobby;
using TDS.Server.Database.SeedData.Player;
using TDS.Server.Database.SeedData.Rest;
using TDS.Server.Database.SeedData.Server;
using TDS.Server.Database.SeedData.Userpanel;

namespace TDS.Server.Database.SeedData
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
