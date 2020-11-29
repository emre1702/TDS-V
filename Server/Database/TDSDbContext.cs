using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Reflection;
using TDS.Server.Database.Entity.Admin;
using TDS.Server.Database.Entity.Bonusbot;
using TDS.Server.Database.Entity.Challenge;
using TDS.Server.Database.Entity.Command;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Entity.Log;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Player.Char;
using TDS.Server.Database.Entity.Player.Settings;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Database.Entity.Server;
using TDS.Server.Database.Entity.Userpanel;
using TDS.Server.Database.EntityConfigurations;
using TDS.Server.Database.EntityConfigurations.Admin;
using TDS.Server.Database.ModelBuilding.BonusBot;
using TDS.Server.Database.ModelBuilding.Challenge;
using TDS.Server.Database.ModelBuilding.Command;
using TDS.Server.Database.SeedData;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Challenge;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Database.Entity
{
    public partial class TDSDbContext : DbContext
    {
        static TDSDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PlayerRelation>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WeaponHash>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WeaponType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<LogType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<LobbyType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Language>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<VehicleHash>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FreeroamVehicleType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<MapLimitType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RuleCategory>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RuleTarget>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<UserpanelAdminQuestionAnswerType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<SupportType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ChallengeType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ChallengeFrequency>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ScoreboardPlayerSorting>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TimeSpanUnitsOfTime>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PedBodyPart>();
        }

        public TDSDbContext(DbContextOptions<TDSDbContext> options)
                    : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual DbSet<AdminLevels> AdminLevels { get; set; }
        public virtual DbSet<Announcements> Announcements { get; set; }
        public virtual DbSet<ApplicationAnswers> ApplicationAnswers { get; set; }
        public virtual DbSet<ApplicationInvitations> ApplicationInvitations { get; set; }
        public virtual DbSet<ApplicationQuestions> ApplicationQuestions { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<BonusbotSettings> BonusbotSettings { get; set; }
        public virtual DbSet<ChallengeSettings> ChallengeSettings { get; set; }
        public virtual DbSet<ChatInfos> ChatInfos { get; set; }
        public virtual DbSet<CommandAlias> CommandAlias { get; set; }
        public virtual DbSet<CommandInfos> CommandInfos { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<FAQs> FAQs { get; set; }
        public virtual DbSet<FreeroamDefaultVehicle> FreeroamDefaultVehicle { get; set; }
        public virtual DbSet<GangHouses> GangHouses { get; set; }
        public virtual DbSet<GangLevelSettings> GangLevelSettings { get; set; }
        public virtual DbSet<GangMembers> GangMembers { get; set; }
        public virtual DbSet<GangRankPermissions> GangRankPermissions { get; set; }
        public virtual DbSet<GangRanks> GangRanks { get; set; }
        public virtual DbSet<Gangs> Gangs { get; set; }
        public virtual DbSet<GangVehicles> GangVehicles { get; set; }
        public virtual DbSet<GangActionAreas> GangActionAreas { get; set; }
        public virtual DbSet<LobbyKillingspreeRewards> KillingspreeRewards { get; set; }
        public virtual DbSet<Lobbies> Lobbies { get; set; }
        public virtual DbSet<LobbyFightSettings> LobbyFightSettings { get; set; }
        public virtual DbSet<LobbyMaps> LobbyMaps { get; set; }
        public virtual DbSet<LobbyRewards> LobbyRewards { get; set; }
        public virtual DbSet<LobbyRoundSettings> LobbyRoundSettings { get; set; }
        public virtual DbSet<LobbyWeapons> LobbyWeapons { get; set; }
        public virtual DbSet<LogAdmins> LogAdmins { get; set; }
        public virtual DbSet<LogChats> LogChats { get; set; }
        public virtual DbSet<LogErrors> LogErrors { get; set; }
        public virtual DbSet<LogKills> LogKills { get; set; }
        public virtual DbSet<LogRests> LogRests { get; set; }
        public virtual DbSet<Maps> Maps { get; set; }
        public virtual DbSet<Offlinemessages> Offlinemessages { get; set; }
        public virtual DbSet<PlayerBans> PlayerBans { get; set; }
        public virtual DbSet<PlayerChallenges> PlayerChallenges { get; set; }
        public virtual DbSet<PlayerCharAppearanceDatas> PlayerCharAppearanceDatas { get; set; }
        public virtual DbSet<PlayerCharDatas> PlayerCharDatas { get; set; }
        public virtual DbSet<PlayerCharFeaturesDatas> PlayerCharFeaturesDatas { get; set; }
        public virtual DbSet<PlayerCharGeneralDatas> PlayerCharGeneralDatas { get; set; }
        public virtual DbSet<PlayerCharHairAndColorsDatas> PlayerCharHairAndColorsDatas { get; set; }
        public virtual DbSet<PlayerCharHeritageDatas> PlayerCharHeritageDatas { get; set; }
        public virtual DbSet<PlayerClothes> PlayerClothes { get; set; }
        public virtual DbSet<PlayerCommands> PlayerCommands { get; set; }
        public virtual DbSet<PlayerKillInfoSettings> PlayerKillInfoSettings { get; set; }
        public virtual DbSet<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual DbSet<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual DbSet<PlayerMapRatings> PlayerMapRatings { get; set; }
        public virtual DbSet<PlayerRelations> PlayerRelations { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<PlayerSettings> PlayerSettings { get; set; }
        public virtual DbSet<PlayerStats> PlayerStats { get; set; }
        public virtual DbSet<PlayerThemeSettings> PlayerThemeSettings { get; set; }
        public virtual DbSet<PlayerWeaponBodypartStats> PlayerWeaponBodypartStats { get; set; }
        public virtual DbSet<PlayerWeaponStats> PlayerWeaponStats { get; set; }
        public virtual DbSet<Rules> Rules { get; set; }
        public virtual DbSet<ServerDailyStats> ServerDailyStats { get; set; }
        public virtual DbSet<ServerSettings> ServerSettings { get; set; }
        public virtual DbSet<ServerTotalStats> ServerTotalStats { get; set; }
        public virtual DbSet<SupportRequestMessages> SupportRequestMessages { get; set; }
        public virtual DbSet<SupportRequests> SupportRequests { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<Weapons> Weapons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasPostgresExtension("tsm_system_rows")

                .HasEnums()

                .ApplyConfigurationsFromAssembly(GetType().Assembly)
                
                .HasSeeds();
        }
    }
}
