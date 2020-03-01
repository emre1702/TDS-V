using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;
using TDS_Common.Enum.Userpanel;
using TDS_Server_DB.Entity.Admin;
using TDS_Server_DB.Entity.Bonusbot;
using TDS_Server_DB.Entity.Challenge;
using TDS_Server_DB.Entity.Command;
using TDS_Server_DB.Entity.GangEntities;
using TDS_Server_DB.Entity.LobbyEntities;
using TDS_Server_DB.Entity.Log;
using TDS_Server_DB.Entity.Player;
using TDS_Server_DB.Entity.Rest;
using TDS_Server_DB.Entity.Server;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server_DB.Entity
{
    public partial class TDSDbContext : DbContext
    {
        public static bool IsConfigured { get; private set; }

        private static string _connectionString;

        public TDSDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TDSDbContext()
        {
        }

        public TDSDbContext(DbContextOptions<TDSDbContext> options)
            : base(options)
        {
        }

        static TDSDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EPlayerRelation>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WeaponHash>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EWeaponType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ELogType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ELobbyType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ELanguage>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<VehicleHash>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EFreeroamVehicleType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EMapLimitType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ERuleCategory>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ERuleTarget>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EUserpanelAdminQuestionAnswerType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ESupportType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EChallengeType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EChallengeFrequency>();
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
        public virtual DbSet<CommandAlias> CommandAlias { get; set; }
        public virtual DbSet<CommandInfos> CommandInfos { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<FAQs> FAQs { get; set; }
        public virtual DbSet<FreeroamDefaultVehicle> FreeroamDefaultVehicle { get; set; }
        public virtual DbSet<GangMembers> GangMembers { get; set; }
        public virtual DbSet<GangRankPermissions> GangRankPermissions { get; set; }
        public virtual DbSet<GangRanks> GangRanks { get; set; }
        public virtual DbSet<Gangs> Gangs { get; set; }
        public virtual DbSet<GangwarAreas> GangwarAreas { get; set; }
        public virtual DbSet<Lobbies> Lobbies { get; set; }
        public virtual DbSet<LobbyKillingspreeRewards> KillingspreeRewards { get; set; }
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
        public virtual DbSet<PlayerClothes> PlayerClothes { get; set; }
        public virtual DbSet<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual DbSet<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual DbSet<PlayerMapRatings> PlayerMapRatings { get; set; }
        public virtual DbSet<PlayerRelations> PlayerRelations { get; set; }
        public virtual DbSet<PlayerSettings> PlayerSettings { get; set; }
        public virtual DbSet<PlayerStats> PlayerStats { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Rules> Rules { get; set; }
        public virtual DbSet<ServerDailyStats> ServerDailyStats { get; set; }
        public virtual DbSet<ServerSettings> ServerSettings { get; set; }
        public virtual DbSet<ServerTotalStats> ServerTotalStats { get; set; }
        public virtual DbSet<SupportRequests> SupportRequests { get; set; }
        public virtual DbSet<SupportRequestMessages> SupportRequestMessages { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<Weapons> Weapons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                        .AddProvider(new CustomDBLogger())
                );

                string connStr = _connectionString ?? "Server=localhost;Database=TDSV;User ID=tdsv;Password=ajagrebo;";
                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging()
                    .UseNpgsql(connStr, options => 
                        options.EnableRetryOnFailure())
                    .UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseIdentityByDefaultColumns();
            modelBuilder.HasPostgresExtension("tsm_system_rows");

            #region Enum
            modelBuilder.HasPostgresEnum<EPlayerRelation>();
            modelBuilder.HasPostgresEnum<WeaponHash>();
            modelBuilder.HasPostgresEnum<EWeaponType>();
            modelBuilder.HasPostgresEnum<ELogType>();
            modelBuilder.HasPostgresEnum<ELobbyType>();
            modelBuilder.HasPostgresEnum<ELanguage>();
            modelBuilder.HasPostgresEnum<VehicleHash>();
            modelBuilder.HasPostgresEnum<EFreeroamVehicleType>();
            modelBuilder.HasPostgresEnum<EMapLimitType>();
            modelBuilder.HasPostgresEnum<ERuleCategory>();
            modelBuilder.HasPostgresEnum<ERuleTarget>();
            modelBuilder.HasPostgresEnum<EUserpanelAdminQuestionAnswerType>();
            modelBuilder.HasPostgresEnum<ESupportType>();
            modelBuilder.HasPostgresEnum<EChallengeType>();
            modelBuilder.HasPostgresEnum<EChallengeFrequency>();
            #endregion

            #region Tables
            modelBuilder.Entity<AdminLevels>(entity =>
            {
                entity.HasKey(e => e.Level);

                entity.Property(e => e.Level).ValueGeneratedNever();
            });

            modelBuilder.Entity<AdminLevelNames>(entity =>
            {
                entity.HasKey(e => new { e.Level, e.Language });

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.LevelNavigation)
                    .WithMany(p => p.AdminLevelNames)
                    .HasForeignKey(d => d.Level)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Announcements>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Created)
                    .IsRequired()
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
                
                entity.Property(e => e.Text)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationAnswers>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.QuestionId });

                entity.Property(e => e.ApplicationId);
                entity.Property(e => e.QuestionId);

                entity.HasOne(answer => answer.Application)
                    .WithMany(app => app.Answers)
                    .HasForeignKey(answer => answer.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(answer => answer.Question)
                    .WithMany(app => app.Answers)
                    .HasForeignKey(answer => answer.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApplicationInvitations>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(invitation => invitation.Application)
                    .WithMany(app => app.Invitations)
                    .HasForeignKey(answer => answer.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(invitation => invitation.Admin)
                   .WithMany(app => app.ApplicationInvitations)
                   .HasForeignKey(answer => answer.AdminId)
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApplicationQuestions>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(question => question.Admin)
                    .WithMany(admin => admin.ApplicationQuestions)
                    .HasForeignKey(answer => answer.AdminId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreateTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(app => app.Player)
                    .WithOne(player => player.Application)
                    .HasForeignKey<Applications>(app => app.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BonusbotSettings>(entity =>
            {
                entity.Property(e => e.GuildId).IsRequired(false);

                entity.Property(e => e.AdminApplicationsChannelId).IsRequired(false);
                entity.Property(e => e.SupportRequestsChannelId).IsRequired(false);
                entity.Property(e => e.ServerInfosChannelId).IsRequired(false);
                entity.Property(e => e.ActionsInfoChannelId).IsRequired(false);
                entity.Property(e => e.BansInfoChannelId).IsRequired(false);

                entity.Property(e => e.ErrorLogsChannelId).IsRequired(false);

                entity.Property(e => e.RefreshServerStatsFrequencySec).IsRequired().HasDefaultValue(60);
            });

            modelBuilder.Entity<ChallengeSettings>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Frequency });

                entity.Property(e => e.Type)
                    .ValueGeneratedNever();

                entity.Property(e => e.MinNumber)
                    .IsRequired()
                    .HasDefaultValue(1);

                entity.Property(e => e.MaxNumber)
                    .IsRequired()
                    .HasDefaultValue(1);

                entity.Property(e => e.Frequency)
                    .IsRequired();
            });

            modelBuilder.Entity<CommandAlias>(entity =>
            {
                entity.HasKey(e => new { e.Alias, e.Command });

                entity.Property(e => e.Alias).HasMaxLength(100);

                entity.HasOne(d => d.CommandNavigation)
                    .WithMany(p => p.CommandAlias)
                    .HasForeignKey(d => d.Command)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CommandInfos>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Language });

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.CommandInfos)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Commands>(entity =>
            {
                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.NeededAdminLevelNavigation)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.NeededAdminLevel)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FAQs>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Language });
            });

            modelBuilder.Entity<FreeroamDefaultVehicle>(entity =>
            {
                entity.HasKey(e => e.VehicleType);

                entity.Property(e => e.VehicleType);

                entity.Property(e => e.Note).HasColumnType("character varying");
            });

            modelBuilder.Entity<GangMembers>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.Rank).HasDefaultValue(0);

                entity.Property(e => e.JoinTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(e => e.Gang)
                    .WithMany(g => g.Members)
                    .HasForeignKey(e => e.GangId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Player)
                    .WithOne(g => g.GangMemberNavigation)
                    .HasForeignKey<GangMembers>(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GangRankPermissions>(entity =>
            {
                entity.HasKey(e => e.GangId);

                entity.Property(e => e.GangId);

                entity.HasOne(e => e.Gang)
                    .WithOne(g => g.RankPermissions)
                    .HasForeignKey<GangRankPermissions>(e => e.GangId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GangRanks>(entity =>
            {
                entity.HasKey(e => new { e.GangId, e.Rank });

                entity.Property(e => e.GangId);

                entity.HasOne(e => e.Gang)
                    .WithMany(g => g.Ranks)
                    .HasForeignKey(e => e.GangId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Gangs>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Short)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.OwnerId)
                    .IsRequired(false);

                entity.Property(e => e.CreateTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Gangs)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Owner)
                    .WithOne(o => o.OwnedGang)
                    .HasForeignKey<Gangs>(o => o.OwnerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<GangwarAreas>(entity =>
            {
                entity.HasKey(e => e.MapId);

                entity.Property(e => e.LastAttacked)
                    .HasDefaultValueSql("'2019-1-1'::timestamp");

                entity.Property(e => e.AttackCount)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(e => e.DefendCount)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasOne(g => g.Map)
                    .WithOne(m => m.GangwarArea)
                    .HasForeignKey<GangwarAreas>(g => g.MapId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(g => g.OwnerGang)
                    .WithMany(m => m.GangwarAreas)
                    .HasForeignKey(g => g.OwnerGangId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<LobbyKillingspreeRewards>(entity =>
            {
                entity.HasKey(e => new { e.LobbyId, e.KillsAmount });

                entity.Property(e => e.KillsAmount).ValueGeneratedNever();

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.LobbyKillingspreeRewards)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Lobbies>(entity =>
            {
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.AroundSpawnPoint).HasDefaultValueSql("3");

                entity.Property(e => e.CreateTimestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.Property(e => e.DefaultSpawnX).HasDefaultValueSql("0");
                entity.Property(e => e.DefaultSpawnY).HasDefaultValueSql("0");
                entity.Property(e => e.DefaultSpawnZ).HasDefaultValueSql("9000");
                entity.Property(e => e.DefaultSpawnRotation).HasDefaultValueSql("0");
                entity.Property(e => e.IsTemporary);
                entity.Property(e => e.IsOfficial);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.SpawnAgainAfterDeathMs).HasDefaultValueSql("400");

                entity.Property(e => e.StartArmor).HasDefaultValueSql("100");

                entity.Property(e => e.StartHealth).HasDefaultValueSql("100");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Lobbies)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LobbyMaps>(entity =>
            {
                entity.HasKey(e => new { e.LobbyId, e.MapId });

                entity.HasIndex(e => e.MapId);

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LobbyRewards>(entity =>
            {
                entity.HasKey(e => e.LobbyId);

                entity.Property(e => e.LobbyId)
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Lobby)
                    .WithOne(p => p.LobbyRewards)
                    .HasForeignKey<LobbyRewards>(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LobbyRoundSettings>(entity =>
            {
                entity.HasKey(e => e.LobbyId);

                entity.Property(e => e.LobbyId)
                    .ValueGeneratedNever();

                entity.Property(e => e.BombDefuseTimeMs).HasDefaultValueSql("8000");

                entity.Property(e => e.BombDetonateTimeMs).HasDefaultValueSql("45000");

                entity.Property(e => e.BombPlantTimeMs).HasDefaultValueSql("3000");

                entity.Property(e => e.CountdownTime).HasDefaultValueSql("5");

                entity.Property(e => e.RoundTime).HasDefaultValueSql("240");

                entity.HasOne(d => d.Lobby)
                    .WithOne(p => p.LobbyRoundSettings)
                    .HasForeignKey<LobbyRoundSettings>(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LobbyMapSettings>(entity =>
            {
                entity.HasKey(e => e.LobbyId);

                entity.Property(e => e.LobbyId)
                   .ValueGeneratedNever();

                entity.Property(e => e.MapLimitTime).HasDefaultValueSql("10");

                entity.HasOne(d => d.Lobby)
                    .WithOne(p => p.LobbyMapSettings)
                    .HasForeignKey<LobbyMapSettings>(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LobbyWeapons>(entity =>
            {
                entity.HasKey(e => new { e.Hash, e.Lobby });

                entity.Property(e => e.Hash).ValueGeneratedNever();

                entity.HasOne(d => d.HashNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Hash)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Lobby)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LogAdmins>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseHiLo();

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
                entity.Property(e => e.LengthOrEndTime).IsRequired(false);
            });

            modelBuilder.Entity<LogChats>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseHiLo();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<LogErrors>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseHiLo();

                entity.Property(e => e.Info).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<LogKills>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseHiLo();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<LogRests>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).UseHiLo();

                entity.Property(e => e.Serial).HasMaxLength(200);

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<Maps>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasMethod("hash");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreateTimestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Maps)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Offlinemessages>(entity =>
            {
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.OfflinemessagesSource)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.OfflinemessagesTarget)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerBans>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.LobbyId });

                entity.HasIndex(e => e.IP);
                entity.HasIndex(e => e.SCName);
                entity.HasIndex(e => e.SCId);
                entity.HasIndex(e => e.Serial);

                entity.Property(e => e.EndTimestamp)
                    .HasConversion(v => v, v => v == null ? (DateTime?)null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

                entity.Property(e => e.IP).IsRequired(false);

                entity.Property(e => e.Serial).IsRequired(false);

                entity.Property(e => e.SCName).IsRequired(false);

                entity.Property(e => e.SCId).IsRequired(false);

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.StartTimestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.PlayerBansAdmin)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.PlayerBans)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerBansPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<PlayerChallenges>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.Challenge, e.Frequency });

                entity.Property(e => e.Amount).HasDefaultValue(1);

                entity.Property(e => e.CurrentAmount).HasDefaultValue(0);

                entity.HasOne(c => c.Player)
                    .WithMany(p => p.Challenges)
                    .HasForeignKey(c => c.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerClothes>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.HasOne(c => c.Player)
                    .WithOne(p => p.PlayerClothes)
                    .HasForeignKey<PlayerClothes>(c => c.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerLobbyStats>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.LobbyId });

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.PlayerLobbyStats)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerLobbyStats)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerMapFavourites>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.MapId });

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.PlayerMapFavourites)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMapFavourites)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerMapRatings>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.MapId });

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.PlayerMapRatings)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMapRatings)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerRelations>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.TargetId });

                entity.Property(e => e.Relation).IsRequired();

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerRelationsPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.PlayerRelationsTarget)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerSettings>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.PlayerId)
                    .ValueGeneratedNever();

                entity.Property(e => e.AllowDataTransfer);
                entity.Property(e => e.Bloodscreen);
                entity.Property(e => e.FloatingDamageInfo);
                entity.Property(e => e.Hitsound);
                entity.Property(e => e.Language).HasDefaultValue(ELanguage.English);
                entity.Property(e => e.Voice3D).HasDefaultValue(false);
                entity.Property(e => e.VoiceAutoVolume).HasDefaultValue(false);
                entity.Property(e => e.VoiceVolume).HasDefaultValue(6.0);
                entity.Property(e => e.MapBorderColor).HasDefaultValue("rgba(150,0,0,0.35)");
                entity.Property(e => e.ShowConfettiAtRanking);
                entity.Property(e => e.DiscordUserId).HasDefaultValue(0);
                entity.Property(e => e.Timezone)
                    .HasDefaultValue("UTC");
                entity.Property(e => e.DateTimeFormat)
                    .HasDefaultValue("yyyy'-'MM'-'dd HH':'mm':'ss");
                entity.Property(e => e.BloodscreenCooldownMs)
                    .HasDefaultValue(150);
                entity.Property(e => e.HudAmmoUpdateCooldownMs)
                    .HasDefaultValue(100);
                entity.Property(e => e.HudHealthUpdateCooldownMs)
                    .HasDefaultValue(100);
                entity.Property(e => e.AFKKickAfterSeconds)
                    .HasDefaultValue(25);
                entity.Property(e => e.AFKKickShowWarningLastSeconds)
                    .HasDefaultValue(10);
                entity.Property(e => e.ShowFloatingDamageInfoDurationMs)
                    .HasDefaultValue(1000);
                entity.Property(e => e.NametagDeadColor)
                    .HasDefaultValue("rgba(0, 0, 0, 1)")
                    .IsRequired(false);
                entity.Property(e => e.NametagHealthEmptyColor)
                    .HasDefaultValue("rgba(50, 0, 0, 1)");
                entity.Property(e => e.NametagHealthFullColor)
                    .HasDefaultValue("rgba(0, 255, 0, 1)");
                entity.Property(e => e.NametagArmorEmptyColor)
                    .IsRequired(false);
                entity.Property(e => e.NametagArmorFullColor)
                    .HasDefaultValue("rgba(255, 255, 255, 1)");
                entity.Property(e => e.CheckAFK)
                    .HasDefaultValue(true);

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerSettings)
                    .HasForeignKey<PlayerSettings>(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerStats>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.PlayerId)
                    .ValueGeneratedNever();

                entity.Property(e => e.MapsBoughtCounter)
                    .IsRequired()
                    .HasDefaultValue(1);

                entity.Property(e => e.LastLoginTimestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.Property(e => e.LastMapsBoughtCounterReduce)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.Property(e => e.LastFreeUsernameChange)
                    .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null)
                    .IsRequired(false);

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerStats)
                    .HasForeignKey<PlayerStats>(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerTotalStats>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.PlayerId)
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Player)
                   .WithOne(p => p.PlayerTotalStats)
                   .HasForeignKey<PlayerTotalStats>(d => d.PlayerId)
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Donation).HasDefaultValue(0);

                entity.Property(e => e.AdminLvl).HasDefaultValue(0);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AdminLeaderId)
                    .IsRequired(false);

                entity.Property(e => e.RegisterTimestamp)
                    .HasDefaultValueSql("timezone('utc', now())")
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

                entity.Property(e => e.SCName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SCId)
                    .IsRequired();

                entity.HasOne(d => d.AdminLvlNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.AdminLvl)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.AdminLeader)
                    .WithMany(p => p.AdminMembers)
                    .HasForeignKey(p => p.AdminLeaderId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Rules>(entity =>
            {

            });

            modelBuilder.Entity<RuleTexts>(entity =>
            {
                entity.HasKey(e => new { e.RuleId, e.Language });

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.RuleTexts)
                    .HasForeignKey(d => d.RuleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ServerDailyStats>(entity =>
            {
                

                entity.HasKey(e => e.Date);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("date")
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', CURRENT_DATE)");
                entity.Property(e => e.PlayerPeak).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.ArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.CustomArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.AmountLogins).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.AmountRegistrations).IsRequired().HasDefaultValue(0);
            });

            modelBuilder.Entity<ServerSettings>(entity =>
            {
                entity.Property(e => e.GamemodeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.KillingSpreeMaxSecondsUntilNextKill)
                    .IsRequired()
                    .HasDefaultValue(18);

                entity.Property(e => e.MapRatingAmountForCheck)
                    .IsRequired()
                    .HasDefaultValue(10);

                entity.Property(e => e.MinMapRatingForNewMaps)
                    .IsRequired()
                    .HasDefaultValue(3f);

                entity.Property(e => e.GiveMoneyFee)
                    .IsRequired()
                    .HasDefaultValue(0.05);

                entity.Property(e => e.GiveMoneyMinAmount)
                    .IsRequired()
                    .HasDefaultValue(100);

                entity.Property(e => e.NametagMaxDistance)
                    .IsRequired()
                    .HasDefaultValue(25*25);

                entity.Property(e => e.MultiplierRankingKills)
                    .IsRequired()
                    .HasDefaultValue(75f);

                entity.Property(e => e.MultiplierRankingAssists)
                    .IsRequired()
                    .HasDefaultValue(25f);

                entity.Property(e => e.MultiplierRankingDamage)
                    .IsRequired()
                    .HasDefaultValue(1f);

                entity.Property(e => e.ShowNametagOnlyOnAiming)
                    .IsRequired();

                entity.Property(e => e.CloseApplicationAfterDays)
                    .IsRequired()
                    .HasDefaultValue(7);

                entity.Property(e => e.DeleteApplicationAfterDays)
                    .IsRequired()
                    .HasDefaultValue(14);

                entity.Property(e => e.GangwarPreparationTime)
                    .IsRequired()
                    .HasDefaultValue(3 * 60);

                entity.Property(e => e.GangwarActionTime)
                    .IsRequired()
                    .HasDefaultValue(15 * 60);

                entity.Property(e => e.DeleteRequestsDaysAfterClose)
                    .IsRequired()
                    .HasDefaultValue(30);

                entity.Property(e => e.DeleteOfflineMessagesAfterDays)
                    .IsRequired()
                    .HasDefaultValue(60);

                entity.Property(e => e.MinPlayersOnlineForGangwar)
                    .IsRequired()
                    .HasDefaultValue(3);

                entity.Property(e => e.GangwarAreaAttackCooldownMinutes)
                    .IsRequired()
                    .HasDefaultValue(60);

                entity.Property(e => e.AmountPlayersAllowedInGangwarTeamBeforeCountCheck)
                    .IsRequired()
                    .HasDefaultValue(3);

                entity.Property(e => e.GangwarAttackerCanBeMore)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(e => e.GangwarOwnerCanBeMore)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.GangwarTargetRadius)
                    .IsRequired()
                    .HasDefaultValue(5d);

                entity.Property(e => e.GangwarTargetWithoutAttackerMaxSeconds)
                    .IsRequired()
                    .HasDefaultValue(10);

                entity.Property(e => e.ReduceMapsBoughtCounterAfterMinute)
                    .IsRequired()
                    .HasDefaultValue(60);

                entity.Property(e => e.MapBuyBasePrice)
                    .IsRequired()
                    .HasDefaultValue(1000);

                entity.Property(e => e.MapBuyCounterMultiplicator)
                    .IsRequired()
                    .HasDefaultValue(1f);

                entity.Property(e => e.UsernameChangeCost)
                    .IsRequired()
                    .HasDefaultValue(20000);

                entity.Property(e => e.UsernameChangeCooldownDays)
                    .IsRequired()
                    .HasDefaultValue(60);

                entity.Property(e => e.AmountWeeklyChallenges)
                    .IsRequired()
                    .HasDefaultValue(3);
            });

            modelBuilder.Entity<ServerTotalStats>(entity =>
            {
                entity.Property(e => e.PlayerPeak).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.ArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.CustomArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
            });

            modelBuilder.Entity<SupportRequests>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.CreateTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', CURRENT_DATE)");

                entity.Property(e => e.CloseTime)
                   .HasConversion(v => v, v => v == null ? (DateTime?)null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

                entity.HasOne(e => e.Author)
                    .WithOne(a => a.SupportRequests)
                    .HasForeignKey<SupportRequests>(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SupportRequestMessages>(entity =>
            {
                entity.HasKey(e => new { e.RequestId, e.MessageIndex });

                entity.Property(e => e.Text).HasMaxLength(300);

                entity.Property(e => e.CreateTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', CURRENT_DATE)");

                entity.HasOne(e => e.Author)
                    .WithMany(a => a.SupportRequestMessages)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Request)
                    .WithMany(r => r.Messages)
                    .HasForeignKey(e => e.RequestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Teams>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValue("Spectator");

                entity.Property(e => e.SkinHash)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(e => e.BlipColor)
                    .HasDefaultValue(4);
                    
                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Lobby)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Weapons>(entity =>
            {
                entity.HasKey(e => e.Hash);

                entity.Property(e => e.Hash).IsRequired();

                entity.Property(e => e.ClipSize).HasDefaultValue(0);
                entity.Property(e => e.MinHeadShotDistance).HasDefaultValue(0);
                entity.Property(e => e.MaxHeadShotDistance).HasDefaultValue(0);
                entity.Property(e => e.HeadShotDamageModifier).HasDefaultValue(0);
                entity.Property(e => e.Damage).HasDefaultValue(0);
                entity.Property(e => e.HitLimbsDamageModifier).HasDefaultValue(0);
                entity.Property(e => e.ReloadTime).HasDefaultValue(0);
                entity.Property(e => e.TimeBetweenShots).HasDefaultValue(0);
                entity.Property(e => e.Range).HasDefaultValue(0);
            });
            #endregion

            #region Seed data
            modelBuilder.Entity<ServerSettings>().HasData(
                new ServerSettings {  Id = 1, GamemodeName = "tdm",
                    ErrorToPlayerOnNonExistentCommand = true, ToChatOnNonExistentCommand = false,
                    DistanceToSpotToPlant = 3, DistanceToSpotToDefuse = 3,
                    SavePlayerDataCooldownMinutes = 1, SaveLogsCooldownMinutes = 1, SaveSeasonsCooldownMinutes = 1, TeamOrderCooldownMs = 3000,
                    ArenaNewMapProbabilityPercent = 2, KillingSpreeMaxSecondsUntilNextKill = 18,
                    MapRatingAmountForCheck = 10, MinMapRatingForNewMaps = 3f, 
                    GiveMoneyFee = 0.05f, GiveMoneyMinAmount = 100,
                    NametagMaxDistance = 80, ShowNametagOnlyOnAiming = true,
                    MultiplierRankingKills = 75f, MultiplierRankingAssists = 25f, MultiplierRankingDamage = 1f
                }
            );

            modelBuilder.Entity<BonusbotSettings>().HasData(
                new BonusbotSettings
                {
                    Id = 1, GuildId = 320309924175282177, 
                    AdminApplicationsChannelId = 659072893526736896,
                    SupportRequestsChannelId = 659073029896142855,
                    ServerInfosChannelId = 659073271911809037,
                    ActionsInfoChannelId = 659088752890871818,
                    BansInfoChannelId = 659705941771550730,

                    ErrorLogsChannelId = 659073884796092426,

                    SendPrivateMessageOnBan = true,
                    SendPrivateMessageOnOfflineMessage = true
                }  
            );

            modelBuilder.Entity<ChallengeSettings>().HasData(
                new ChallengeSettings { Type = EChallengeType.Assists, Frequency = EChallengeFrequency.Weekly, MinNumber = 50, MaxNumber = 100 },
                new ChallengeSettings { Type = EChallengeType.BeHelpfulEnough, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = EChallengeType.BombDefuse, Frequency = EChallengeFrequency.Weekly, MinNumber = 5, MaxNumber = 10 },
                new ChallengeSettings { Type = EChallengeType.BombPlant, Frequency = EChallengeFrequency.Weekly, MinNumber = 5, MaxNumber = 10 },
                new ChallengeSettings { Type = EChallengeType.BuyMaps, Frequency = EChallengeFrequency.Forever, MinNumber = 500, MaxNumber = 500 },
                new ChallengeSettings { Type = EChallengeType.ChangeSettings, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = EChallengeType.CreatorOfAcceptedMap, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = EChallengeType.Damage, Frequency = EChallengeFrequency.Weekly, MinNumber = 20000, MaxNumber = 100000 },
                new ChallengeSettings { Type = EChallengeType.JoinDiscordServer, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = EChallengeType.Kills, Frequency = EChallengeFrequency.Weekly, MinNumber = 75, MaxNumber = 150 },
                new ChallengeSettings { Type = EChallengeType.Killstreak, Frequency = EChallengeFrequency.Weekly, MinNumber = 3, MaxNumber = 7 },
                new ChallengeSettings { Type = EChallengeType.PlayTime, Frequency = EChallengeFrequency.Weekly, MinNumber = 300, MaxNumber = 1500 },
                new ChallengeSettings { Type = EChallengeType.ReadTheFAQ, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = EChallengeType.ReadTheRules, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = EChallengeType.ReviewMaps, Frequency = EChallengeFrequency.Forever, MinNumber = 10, MaxNumber = 10 },
                new ChallengeSettings { Type = EChallengeType.RoundPlayed, Frequency = EChallengeFrequency.Weekly, MinNumber = 50, MaxNumber = 100 },
                new ChallengeSettings { Type = EChallengeType.WriteHelpfulIssue, Frequency = EChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 }
            );

            modelBuilder.Entity<Players>().HasData(
                new Players { Id = -1, SCName = "System", SCId = 0, Name = "System", Password = "" }
            );

            var seedLobbies = new List<Lobbies> {
                new Lobbies { Id = -4, OwnerId = -1, Type = ELobbyType.MainMenu, Name = "MainMenu", IsTemporary = false, IsOfficial = true, SpawnAgainAfterDeathMs = 0 },
                new Lobbies { Id = -1, OwnerId = -1, Type = ELobbyType.Arena, Name = "Arena", IsTemporary = false, IsOfficial = true, AmountLifes = 1, SpawnAgainAfterDeathMs = 400 },
                new Lobbies { Id = -2, OwnerId = -1, Type = ELobbyType.GangLobby, Name = "GangLobby", IsTemporary = false, IsOfficial = true, AmountLifes = 1, SpawnAgainAfterDeathMs = 400 },
   
                // only for map-creator ban
                new Lobbies { Id = -3, OwnerId = -1, Type = ELobbyType.MapCreateLobby, Name = "MapCreateLobby", IsTemporary = false, IsOfficial = true, AmountLifes = 1, SpawnAgainAfterDeathMs = 400 }
            };
            modelBuilder.Entity<Lobbies>().HasData(seedLobbies);

            modelBuilder.Entity<AdminLevels>().HasData(
                new AdminLevels { Level = 0, ColorR = 220, ColorG = 220, ColorB = 220 },
                new AdminLevels { Level = 1, ColorR = 113, ColorG = 202, ColorB = 113 },
                new AdminLevels { Level = 2, ColorR = 253, ColorG = 132, ColorB = 85 },
                new AdminLevels { Level = 3, ColorR = 222, ColorG = 50, ColorB = 50 }
            );

            var seedCommands = new List<Commands>
            {
                new Commands { Id = 1, Command = "AdminSay", NeededAdminLevel = 1 },
                new Commands { Id = 2, Command = "AdminChat", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 3, Command = "Ban", NeededAdminLevel = 2 },
                new Commands { Id = 4, Command = "Goto", NeededAdminLevel = 2, LobbyOwnerCanUse = true },
                new Commands { Id = 5, Command = "Kick", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 6, Command = "LobbyBan", NeededAdminLevel = 1, VipCanUse = true, LobbyOwnerCanUse = true },
                new Commands { Id = 7, Command = "LobbyKick", NeededAdminLevel = 1, VipCanUse = true, LobbyOwnerCanUse = true },
                new Commands { Id = 8, Command = "Mute", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 9, Command = "NextMap", NeededAdminLevel = 1, VipCanUse = true, LobbyOwnerCanUse = true },
                new Commands { Id = 10, Command = "LobbyLeave" },
                new Commands { Id = 11, Command = "Suicide" },
                new Commands { Id = 12, Command = "GlobalChat" },
                new Commands { Id = 13, Command = "TeamChat" },
                new Commands { Id = 14, Command = "PrivateChat" },
                new Commands { Id = 15, Command = "Position" },
                new Commands { Id = 16, Command = "ClosePrivateChat" },
                new Commands { Id = 17, Command = "OpenPrivateChat" },
                new Commands { Id = 18, Command = "PrivateMessage" },
                new Commands { Id = 19, Command = "UserId" },
                new Commands { Id = 20, Command = "BlockUser" },
                new Commands { Id = 21, Command = "UnblockUser" },
                new Commands { Id = 22, Command = "LoadMapOfOthers", NeededAdminLevel = 1, VipCanUse = true },    // not a command
                new Commands { Id = 23, Command = "VoiceMute", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 24, Command = "GiveMoney" },
                new Commands { Id = 25, Command = "LobbyInvitePlayer" },
                new Commands { Id = 26, Command = "Test", NeededAdminLevel = 3 }
            };
            modelBuilder.Entity<Commands>().HasData(seedCommands);


            modelBuilder.Entity<AdminLevelNames>().HasData(new List<AdminLevelNames> {
                new AdminLevelNames { Level = 0, Language = ELanguage.English, Name = "User" },
                new AdminLevelNames { Level = 0, Language = ELanguage.German, Name = "User" },
                new AdminLevelNames { Level = 1, Language = ELanguage.English, Name = "Supporter" },
                new AdminLevelNames { Level = 1, Language = ELanguage.German, Name = "Supporter" },
                new AdminLevelNames { Level = 2, Language = ELanguage.English, Name = "Administrator" },
                new AdminLevelNames { Level = 2, Language = ELanguage.German, Name = "Administrator" },
                new AdminLevelNames { Level = 3, Language = ELanguage.English, Name = "Projectleader" },
                new AdminLevelNames { Level = 3, Language = ELanguage.German, Name = "Projektleiter" }
            });

            modelBuilder.Entity<CommandAlias>().HasData(
                new CommandAlias { Alias = "Announce", Command = 1 },
                new CommandAlias { Alias = "Announcement", Command = 1 },
                new CommandAlias { Alias = "ASay", Command = 1 },
                new CommandAlias { Alias = "OChat", Command = 1 },
                new CommandAlias { Alias = "OSay", Command = 1 },
                new CommandAlias { Alias = "AChat", Command = 2 },
                new CommandAlias { Alias = "ChatAdmin", Command = 2 },
                new CommandAlias { Alias = "InternChat", Command = 2 },
                new CommandAlias { Alias = "WriteAdmin", Command = 2 },
                new CommandAlias { Alias = "PBan", Command = 3 },
                new CommandAlias { Alias = "Permaban", Command = 3 },
                new CommandAlias { Alias = "RBan", Command = 3 },
                new CommandAlias { Alias = "TBan", Command = 3 },
                new CommandAlias { Alias = "Timeban", Command = 3 },
                new CommandAlias { Alias = "UBan", Command = 3 },
                new CommandAlias { Alias = "UnBan", Command = 3 },
                new CommandAlias { Alias = "GotoPlayer", Command = 4 },
                new CommandAlias { Alias = "GotoXYZ", Command = 4 },
                new CommandAlias { Alias = "Warp", Command = 4 },
                new CommandAlias { Alias = "WarpTo", Command = 4 },
                new CommandAlias { Alias = "WarpToPlayer", Command = 4 },
                new CommandAlias { Alias = "XYZ", Command = 4 },
                new CommandAlias { Alias = "RKick", Command = 5 },
                new CommandAlias { Alias = "BanLobby", Command = 6 },
                new CommandAlias { Alias = "KickLobby", Command = 7 },
                new CommandAlias { Alias = "PermaMute", Command = 8 },
                new CommandAlias { Alias = "PMute", Command = 8 },
                new CommandAlias { Alias = "RMute", Command = 8 },
                new CommandAlias { Alias = "TimeMute", Command = 8 },
                new CommandAlias { Alias = "TMute", Command = 8 },
                new CommandAlias { Alias = "EndRound", Command = 9 },
                new CommandAlias { Alias = "Next", Command = 9 },
                new CommandAlias { Alias = "Skip", Command = 9 },
                new CommandAlias { Alias = "Back", Command = 10 },
                new CommandAlias { Alias = "Leave", Command = 10 },
                new CommandAlias { Alias = "LeaveLobby", Command = 10 },
                new CommandAlias { Alias = "Mainmenu", Command = 10 },
                new CommandAlias { Alias = "Dead", Command = 11 },
                new CommandAlias { Alias = "Death", Command = 11 },
                new CommandAlias { Alias = "Die", Command = 11 },
                new CommandAlias { Alias = "Kill", Command = 11 },
                new CommandAlias { Alias = "AllChat", Command = 12 },
                new CommandAlias { Alias = "AllSay", Command = 12 },
                new CommandAlias { Alias = "G", Command = 12 },
                new CommandAlias { Alias = "GChat", Command = 12 },
                new CommandAlias { Alias = "Global", Command = 12 },
                new CommandAlias { Alias = "GlobalSay", Command = 12 },
                new CommandAlias { Alias = "PublicChat", Command = 12 },
                new CommandAlias { Alias = "PublicSay", Command = 12 },
                new CommandAlias { Alias = "TChat", Command = 13 },
                new CommandAlias { Alias = "TeamSay", Command = 13 },
                new CommandAlias { Alias = "TSay", Command = 13 },
                new CommandAlias { Alias = "PChat", Command = 14 },
                new CommandAlias { Alias = "PrivateSay", Command = 14 },
                new CommandAlias { Alias = "Coord", Command = 15 },
                new CommandAlias { Alias = "Coordinate", Command = 15 },
                new CommandAlias { Alias = "Coordinates", Command = 15 },
                new CommandAlias { Alias = "CurrentPos", Command = 15 },
                new CommandAlias { Alias = "CurrentPosition", Command = 15 },
                new CommandAlias { Alias = "GetPos", Command = 15 },
                new CommandAlias { Alias = "GetPosition", Command = 15 },
                new CommandAlias { Alias = "Pos", Command = 15 },
                new CommandAlias { Alias = "MSG", Command = 18 },
                new CommandAlias { Alias = "PM", Command = 18 },
                new CommandAlias { Alias = "PSay", Command = 18 },
                new CommandAlias { Alias = "CPC", Command = 16 },
                new CommandAlias { Alias = "ClosePM", Command = 16 },
                new CommandAlias { Alias = "ClosePrivateSay", Command = 16 },
                new CommandAlias { Alias = "StopPrivateChat", Command = 16 },
                new CommandAlias { Alias = "StopPrivateSay", Command = 17 },
                new CommandAlias { Alias = "OpenPrivateSay", Command = 17 },
                new CommandAlias { Alias = "OpenPM", Command = 17 },
                new CommandAlias { Alias = "OPC", Command = 17 },
                new CommandAlias { Alias = "UID", Command = 19 },
                new CommandAlias { Alias = "Ignore", Command = 20 },
                new CommandAlias { Alias = "IgnoreUser", Command = 20 },
                new CommandAlias { Alias = "Block", Command = 20 },
                new CommandAlias { Alias = "Unblock", Command = 21 },
                new CommandAlias { Alias = "PermaVoiceMute", Command = 23 },
                new CommandAlias { Alias = "PVoiceMute", Command = 23 },
                new CommandAlias { Alias = "RVoiceMute", Command = 23 },
                new CommandAlias { Alias = "TimeVoiceMute", Command = 23 },
                new CommandAlias { Alias = "TVoiceMute", Command = 23 },
                new CommandAlias { Alias = "PermaMuteVoice", Command = 23 },
                new CommandAlias { Alias = "PMuteVoice", Command = 23 },
                new CommandAlias { Alias = "RMuteVoice", Command = 23 },
                new CommandAlias { Alias = "TimeMuteVoice", Command = 23 },
                new CommandAlias { Alias = "TMuteVoice", Command = 23 },
                new CommandAlias { Alias = "VoicePermaMute", Command = 23 },
                new CommandAlias { Alias = "VoicePMute", Command = 23 },
                new CommandAlias { Alias = "VoiceRMute", Command = 23 },
                new CommandAlias { Alias = "VoiceTimeMute", Command = 23 },
                new CommandAlias { Alias = "VoiceTMute", Command = 23 },
                new CommandAlias { Alias = "MuteVoice", Command = 23 },
                new CommandAlias { Alias = "MoneyGive", Command = 24 },
                new CommandAlias { Alias = "SendMoney", Command = 24 },
                new CommandAlias { Alias = "MoneySend", Command = 24 },
                new CommandAlias { Alias = "LobbyInvite", Command = 25 },
                new CommandAlias { Alias = "InviteLobby", Command = 25 },
                new CommandAlias { Alias = "InvitePlayerLobby", Command = 25 }
            );

            modelBuilder.Entity<CommandInfos>().HasData(
                new CommandInfos { Id = 1, Language = ELanguage.German, Info = "Schreibt öffentlich als ein Admin." },
                new CommandInfos { Id = 1, Language = ELanguage.English, Info = "Writes public as an admin." },
                new CommandInfos { Id = 2, Language = ELanguage.German, Info = "Schreibt intern nur den Admins." },
                new CommandInfos { Id = 2, Language = ELanguage.English, Info = "Writes intern to admins only." },
                new CommandInfos { Id = 3, Language = ELanguage.German, Info = "Bannt einen Spieler vom gesamten Server." },
                new CommandInfos { Id = 3, Language = ELanguage.English, Info = "Bans a player out of the server." },
                new CommandInfos { Id = 4, Language = ELanguage.German, Info = "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                new CommandInfos { Id = 4, Language = ELanguage.English, Info = "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
                new CommandInfos { Id = 5, Language = ELanguage.German, Info = "Kickt einen Spieler vom Server." },
                new CommandInfos { Id = 5, Language = ELanguage.English, Info = "Kicks a player out of the server." },
                new CommandInfos { Id = 6, Language = ELanguage.German, Info = "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                new CommandInfos { Id = 6, Language = ELanguage.English, Info = "Bans a player out of the lobby in which the command was used." },
                new CommandInfos { Id = 7, Language = ELanguage.German, Info = "Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                new CommandInfos { Id = 7, Language = ELanguage.English, Info = "Kicks a player out of the lobby in which the command was used." },
                new CommandInfos { Id = 8, Language = ELanguage.German, Info = "Mutet einen Spieler im normalen Chat." },
                new CommandInfos { Id = 8, Language = ELanguage.English, Info = "Mutes a player in the normal chat." },
                new CommandInfos { Id = 9, Language = ELanguage.German, Info = "Beendet die jetzige Runde in der jeweiligen Lobby." },
                new CommandInfos { Id = 9, Language = ELanguage.English, Info = "Ends the current round in the lobby." },
                new CommandInfos { Id = 10, Language = ELanguage.German, Info = "Verlässt die jetzige Lobby." },
                new CommandInfos { Id = 10, Language = ELanguage.English, Info = "Leaves the current lobby." },
                new CommandInfos { Id = 11, Language = ELanguage.German, Info = "Tötet den Nutzer (Selbstmord)." },
                new CommandInfos { Id = 11, Language = ELanguage.English, Info = "Kills the user (suicide)." },
                new CommandInfos { Id = 12, Language = ELanguage.German, Info = "Globaler Chat, welcher überall gelesen werden kann." },
                new CommandInfos { Id = 12, Language = ELanguage.English, Info = "Global chat which can be read everywhere." },
                new CommandInfos { Id = 13, Language = ELanguage.German, Info = "Sendet die Nachricht nur zum eigenen Team." },
                new CommandInfos { Id = 13, Language = ELanguage.English, Info = "Sends the message to the current team only." },
                new CommandInfos { Id = 14, Language = ELanguage.German, Info = "Gibt die Position des Spielers aus." },
                new CommandInfos { Id = 14, Language = ELanguage.English, Info = "Outputs the position of the player." },
                new CommandInfos { Id = 15, Language = ELanguage.German, Info = "Sendet eine Nachricht im Privatchat." },
                new CommandInfos { Id = 15, Language = ELanguage.English, Info = "Sends a message in private chat." },
                new CommandInfos { Id = 16, Language = ELanguage.German, Info = "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." },
                new CommandInfos { Id = 16, Language = ELanguage.English, Info = "Closes a private chat or withdraws a private chat request." },
                new CommandInfos { Id = 17, Language = ELanguage.German, Info = "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                new CommandInfos { Id = 17, Language = ELanguage.English, Info = "Sends a private chat request or accepts the request of another user." },
                new CommandInfos { Id = 18, Language = ELanguage.German, Info = "Private Nachricht an einen bestimmten Spieler." },
                new CommandInfos { Id = 18, Language = ELanguage.English, Info = "Private message to a specific player." },
                new CommandInfos { Id = 19, Language = ELanguage.German, Info = "Gibt dir deine User-Id aus." },
                new CommandInfos { Id = 19, Language = ELanguage.English, Info = "Outputs your user-id to yourself." },
                new CommandInfos { Id = 20, Language = ELanguage.German, Info = "Fügt das Ziel in deine Blocklist ein, sodass du keine Nachrichten mehr von ihm liest, er dich nicht einladen kann usw." },
                new CommandInfos { Id = 20, Language = ELanguage.English, Info = "Adds the target into your blocklist so you won't see messages from him, he can't invite you anymore etc." },
                new CommandInfos { Id = 21, Language = ELanguage.German, Info = "Entfernt das Ziel aus der Blockliste." },
                new CommandInfos { Id = 21, Language = ELanguage.English, Info = "Removes the target from the blocklist." },
                new CommandInfos { Id = 23, Language = ELanguage.German, Info = "Mutet einen Spieler im Voice-Chat." },
                new CommandInfos { Id = 23, Language = ELanguage.English, Info = "Mutes a player in the voice-chat." },
                new CommandInfos { Id = 24, Language = ELanguage.German, Info = "Gibt einem Spieler Geld." },
                new CommandInfos { Id = 24, Language = ELanguage.English, Info = "Gives money to a player." },
                new CommandInfos { Id = 25, Language = ELanguage.German, Info = "Ladet einen Spieler in die eigene Lobby ein (falls möglich)." },
                new CommandInfos { Id = 25, Language = ELanguage.English, Info = "Invites a player to your lobby (if possible)." },
                new CommandInfos { Id = 26, Language = ELanguage.German, Info = "Befehl zum schnellen Testen von Codes." },
                new CommandInfos { Id = 26, Language = ELanguage.English, Info = "Command for quick testing of codes." }
            );

            modelBuilder.Entity<FreeroamDefaultVehicle>().HasData(
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Car, VehicleHash = VehicleHash.Pfister811 },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Helicopter, VehicleHash = VehicleHash.Akula },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Plane, VehicleHash = VehicleHash.Pyro },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Bike, VehicleHash = VehicleHash.Hakuchou2 },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Boat, VehicleHash = VehicleHash.Speeder2 }
            );

            modelBuilder.Entity<LobbyKillingspreeRewards>().HasData(
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 3, HealthOrArmor = 30 },
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 5, HealthOrArmor = 50 },
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 10, HealthOrArmor = 100 },
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 15, HealthOrArmor = 100 }
            );

            var seedTeams = new List<Teams> {
                new Teams { Id = -1, Index = 0, Name = "Spectator", Lobby = -4, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 1004114196 },
                new Teams { Id = -2, Index = 0, Name = "Spectator", Lobby = -1, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 0 },
                new Teams { Id = -3, Index = 1, Name = "SWAT", Lobby = -1, ColorR = 0, ColorG = 150, ColorB = 0, BlipColor = 52, SkinHash = -1920001264 },
                new Teams { Id = -4, Index = 2, Name = "Terrorist", Lobby = -1, ColorR = 150, ColorG = 0, ColorB = 0, BlipColor = 1, SkinHash = 275618457 },
                new Teams { Id = -5, Index = 0, Name = "None", Lobby = -2, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 0 }
            };
            modelBuilder.Entity<Teams>().HasData(seedTeams);

            modelBuilder.Entity<Gangs>().HasData(
                new Gangs { Id = -1, TeamId = -5, Short = "-" }
            );

            modelBuilder.Entity<GangRanks>().HasData(
                new GangRanks { GangId = -1, Rank = 0, Name = "-" }
            );

            modelBuilder.Entity<GangRankPermissions>().HasData(
                new GangRankPermissions { GangId = -1, InviteMembers = 5, KickMembers = 5, ManagePermissions = 5, ManageRanks = 5, StartGangwar = 5 }
            );

            modelBuilder.Entity<LobbyMaps>().HasData(
                new LobbyMaps { LobbyId = -1, MapId = -1 }
            );

            modelBuilder.Entity<LobbyRewards>().HasData(
                new LobbyRewards { LobbyId = -1, MoneyPerKill = 20, MoneyPerAssist = 10, MoneyPerDamage = 0.1 },
                new LobbyRewards { LobbyId = -2, MoneyPerKill = 20, MoneyPerAssist = 10, MoneyPerDamage = 0.1 }
            );

            modelBuilder.Entity<LobbyRoundSettings>().HasData(
                new LobbyRoundSettings { LobbyId = -1, RoundTime = 240, CountdownTime = 5, BombDetonateTimeMs = 45000, BombDefuseTimeMs = 8000, BombPlantTimeMs = 3000, MixTeamsAfterRound = true, 
                    ShowRanking = true }
            );

            modelBuilder.Entity<LobbyMapSettings>().HasData(
                new LobbyMapSettings { LobbyId = -1, MapLimitTime = 10, MapLimitType = EMapLimitType.KillAfterTime }  
            );

            modelBuilder.Entity<Weapons>().HasData(
                new Weapons { Hash = WeaponHash.Sniperrifle, Type = EWeaponType.SniperRifle, Damage = 101, HeadShotDamageModifier = 1000 },
                new Weapons { Hash = WeaponHash.Fireextinguisher, Type = EWeaponType.Rest, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Compactlauncher, Type = EWeaponType.HeavyWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Snowball, Type = EWeaponType.ThrownWeapon, Damage = 10, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Vintagepistol, Type = EWeaponType.Handgun, Damage = 34, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Combatpdw, Type = EWeaponType.MachineGun, Damage = 28, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Heavysniper, Type = EWeaponType.SniperRifle, Damage = 216, HeadShotDamageModifier = 2 },
                new Weapons { Hash = WeaponHash.Heavysniper_mk2, Type = EWeaponType.SniperRifle, Damage = 216, HeadShotDamageModifier = 2 },
                new Weapons { Hash = WeaponHash.Autoshotgun, Type = EWeaponType.Shotgun, Damage = 162, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Microsmg, Type = EWeaponType.MachineGun, Damage = 21, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Wrench, Type = EWeaponType.Melee, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Pistol, Type = EWeaponType.Handgun, Damage = 26, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Pistol_mk2, Type = EWeaponType.Handgun, Damage = 26, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Pumpshotgun, Type = EWeaponType.Shotgun, Damage = 58, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Pumpshotgun_mk2, Type = EWeaponType.Shotgun, Damage = 58, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Appistol, Type = EWeaponType.Handgun, Damage = 28, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Ball, Type = EWeaponType.ThrownWeapon, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Molotov, Type = EWeaponType.ThrownWeapon, Damage = 10, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Smg, Type = EWeaponType.MachineGun, Damage = 22, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Smg_mk2, Type = EWeaponType.MachineGun, Damage = 22, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Stickybomb, Type = EWeaponType.ThrownWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Petrolcan, Type = EWeaponType.Rest, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Stungun, Type = EWeaponType.Handgun, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Heavyshotgun, Type = EWeaponType.Shotgun, Damage = 117, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Minigun, Type = EWeaponType.HeavyWeapon, Damage = 30, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Golfclub, Type = EWeaponType.Melee, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Flaregun, Type = EWeaponType.Handgun, Damage = 50, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Flare, Type = EWeaponType.ThrownWeapon, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Grenadelauncher_smoke, Type = EWeaponType.HeavyWeapon, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Hammer, Type = EWeaponType.Melee, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Combatpistol, Type = EWeaponType.Handgun, Damage = 27, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Gusenberg, Type = EWeaponType.MachineGun, Damage = 34, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Compactrifle, Type = EWeaponType.AssaultRifle, Damage = 34, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Hominglauncher, Type = EWeaponType.HeavyWeapon, Damage = 150, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Nightstick, Type = EWeaponType.Melee, Damage = 35, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Railgun, Type = EWeaponType.HeavyWeapon, Damage = 50, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Sawnoffshotgun, Type = EWeaponType.Shotgun, Damage = 160, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Bullpuprifle, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Firework, Type = EWeaponType.HeavyWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Combatmg, Type = EWeaponType.MachineGun, Damage = 28, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Combatmg_mk2, Type = EWeaponType.MachineGun, Damage = 28, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Carbinerifle, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Crowbar, Type = EWeaponType.Melee, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Flashlight, Type = EWeaponType.Melee, Damage = 30, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Dagger, Type = EWeaponType.Melee, Damage = 45, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Grenade, Type = EWeaponType.ThrownWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Poolcue, Type = EWeaponType.Melee, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Bat, Type = EWeaponType.Melee, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Pistol50, Type = EWeaponType.Handgun, Damage = 51, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Knife, Type = EWeaponType.Melee, Damage = 45, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Mg, Type = EWeaponType.MachineGun, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Bullpupshotgun, Type = EWeaponType.Shotgun, Damage = 112, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Bzgas, Type = EWeaponType.ThrownWeapon, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Unarmed, Type = EWeaponType.Melee, Damage = 15, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Grenadelauncher, Type = EWeaponType.HeavyWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Musket, Type = EWeaponType.Shotgun, Damage = 165, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Proximine, Type = EWeaponType.ThrownWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Advancedrifle, Type = EWeaponType.AssaultRifle, Damage = 30, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Rpg, Type = EWeaponType.HeavyWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Pipebomb, Type = EWeaponType.ThrownWeapon, Damage = 100, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Minismg, Type = EWeaponType.MachineGun, Damage = 22, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Snspistol, Type = EWeaponType.Handgun, Damage = 28, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Snspistol_mk2, Type = EWeaponType.Handgun, Damage = 28, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Assaultrifle, Type = EWeaponType.AssaultRifle, Damage = 30, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Assaultrifle_mk2, Type = EWeaponType.AssaultRifle, Damage = 30, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Specialcarbine, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Revolver, Type = EWeaponType.Handgun, Damage = 110, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Revolver_mk2, Type = EWeaponType.Handgun, Damage = 110, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Doubleaction, Type = EWeaponType.Handgun, Damage = 110, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Marksmanrifle, Type = EWeaponType.SniperRifle, Damage = 65, HeadShotDamageModifier = 2 },
                new Weapons { Hash = WeaponHash.Marksmanrifle_mk2, Type = EWeaponType.SniperRifle, Damage = 65, HeadShotDamageModifier = 2 },
                new Weapons { Hash = WeaponHash.Battleaxe, Type = EWeaponType.Melee, Damage = 50, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Heavypistol, Type = EWeaponType.Handgun, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Knuckle, Type = EWeaponType.Melee, Damage = 30, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Machinepistol, Type = EWeaponType.MachineGun, Damage = 20, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Marksmanpistol, Type = EWeaponType.Handgun, Damage = 150, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Machete, Type = EWeaponType.Melee, Damage = 45, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Switchblade, Type = EWeaponType.Melee, Damage = 50, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Assaultshotgun, Type = EWeaponType.Shotgun, Damage = 192, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Dbshotgun, Type = EWeaponType.Shotgun, Damage = 166, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Assaultsmg, Type = EWeaponType.MachineGun, Damage = 23, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Hatchet, Type = EWeaponType.Melee, Damage = 50, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Stone_hatchet, Type = EWeaponType.Melee, Damage = 50, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Bottle, Type = EWeaponType.Melee, Damage = 10, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Parachute, Type = EWeaponType.Rest, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Raypistol, Type = EWeaponType.Handgun, Damage = 80, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Raycarbine, Type = EWeaponType.MachineGun, Damage = 23, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Carbinerifle_mk2, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Rayminigun, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Bullpuprifle_mk2, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Specialcarbine_mk2, Type = EWeaponType.AssaultRifle, Damage = 32, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.Smokegrenade, Type = EWeaponType.ThrownWeapon, Damage = 0, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.CeramicPistol, Type = EWeaponType.Handgun, Damage = 20, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.NavyRevolver, Type = EWeaponType.Handgun, Damage = 40, HeadShotDamageModifier = 1 },
                new Weapons { Hash = WeaponHash.HazardCan, Type = EWeaponType.Rest, Damage = 0, HeadShotDamageModifier = 1 }
            );

            modelBuilder.Entity<LobbyWeapons>().HasData(
                new LobbyWeapons { Hash = WeaponHash.Sniperrifle,  Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Fireextinguisher, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Compactlauncher, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Snowball, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Vintagepistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Combatpdw, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Heavysniper, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Heavysniper_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Autoshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Microsmg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Wrench, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Pistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Pistol_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Pumpshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Pumpshotgun_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Appistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Ball, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Molotov, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Smg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Smg_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Stickybomb, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Petrolcan, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Stungun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Heavyshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Minigun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Golfclub, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Flaregun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Flare, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Grenadelauncher_smoke, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Hammer, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Combatpistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Gusenberg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Compactrifle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Hominglauncher, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Nightstick, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Railgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Sawnoffshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Bullpuprifle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Firework, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Combatmg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Combatmg_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Carbinerifle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Crowbar, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Flashlight, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Dagger, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Grenade, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Poolcue, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Bat, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Pistol50, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Knife, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Mg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Bullpupshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Bzgas, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Grenadelauncher, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Musket, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Proximine, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Advancedrifle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Rpg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Pipebomb, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Minismg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Snspistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Snspistol_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Assaultrifle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Assaultrifle_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Specialcarbine, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Revolver, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Revolver_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Doubleaction, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Marksmanrifle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Marksmanrifle_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Battleaxe, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Heavypistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Knuckle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Machinepistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Marksmanpistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Machete, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Switchblade, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Assaultshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Dbshotgun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Assaultsmg, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Hatchet, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Bottle, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Parachute, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Raypistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Raycarbine, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Carbinerifle_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Rayminigun, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Bullpuprifle_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Specialcarbine_mk2, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Smokegrenade, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.CeramicPistol, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.NavyRevolver, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.HazardCan, Lobby = -1, Ammo = 9999 },
                new LobbyWeapons { Hash = WeaponHash.Stone_hatchet, Lobby = -1, Ammo = 9999 }
            );

            modelBuilder.Entity<Maps>().HasData(
                new Maps { Id = -4, Name = "All Sniper", CreatorId = -1 },
                new Maps { Id = -3, Name = "All Bombs", CreatorId = -1 },
                new Maps { Id = -2, Name = "All Normals", CreatorId = -1 },
                new Maps { Id = -1, Name = "All", CreatorId = -1 }
            );

            modelBuilder.Entity<ServerTotalStats>().HasData(
                new ServerTotalStats { Id = 1 }
            );

            modelBuilder.Entity<Rules>().HasData(
                new Rules { Id = 1, Target = ERuleTarget.User, Category = ERuleCategory.General },
                new Rules { Id = 2, Target = ERuleTarget.User, Category = ERuleCategory.Chat },
                new Rules { Id = 3, Target = ERuleTarget.Admin, Category = ERuleCategory.General },
                new Rules { Id = 4, Target = ERuleTarget.Admin, Category = ERuleCategory.General },
                new Rules { Id = 5, Target = ERuleTarget.Admin, Category = ERuleCategory.General },
                new Rules { Id = 6, Target = ERuleTarget.VIP, Category = ERuleCategory.General },
                new Rules { Id = 7, Target = ERuleTarget.VIP, Category = ERuleCategory.General }
            );

            modelBuilder.Entity<RuleTexts>().HasData(
                new RuleTexts 
                { 
                    RuleId = 1, 
                    Language = ELanguage.English, 
                    RuleStr = @"Teaming with opposing players is strictly forbidden!"
                        + "\nThis means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members."
                        + "\nIf such behaviour is noticed, it can lead to severe penalties and is permanently noted."
                },
                new RuleTexts
                {
                    RuleId = 1,
                    Language = ELanguage.German,
                    RuleStr = @"Teamen mit gegnerischen Spielern ist strengstens verboten!"
                        + "\nDamit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint."
                        + "\nWird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert."
                },

                new RuleTexts
                {
                    RuleId = 2,
                    Language = ELanguage.English,
                    RuleStr = @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none."
                        + "\nBy 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area."
                        + "\nThe chat rules listed here are ONLY for the normal chat in an official lobby."
                        + "\nChats in private lobbies can be freely monitored by the lobby owners."
                },
                new RuleTexts
                {
                    RuleId = 2,
                    Language = ELanguage.German,
                    RuleStr = "Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine."
                        + "\nUnter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich."
                        + "\nDie hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby."
                        + "\nChats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden."
                },

                new RuleTexts
                {
                    RuleId = 3,
                    Language = ELanguage.English,
                    RuleStr = "Admins have to follow the same rules as players do."
                },
                new RuleTexts
                {
                    RuleId = 3,
                    Language = ELanguage.German,
                    RuleStr = "Admins haben genauso die Regeln zu befolgen wie auch die Spieler."
                },

                new RuleTexts
                {
                    RuleId = 4,
                    Language = ELanguage.English,
                    RuleStr = "Exploitation of the commands is strictly forbidden!"
                        + "\nAdmin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules."
                },
                new RuleTexts
                {
                    RuleId = 4,
                    Language = ELanguage.German,
                    RuleStr = "Ausnutzung der Befehle ist strengstens verboten!"
                        + "\nAdmin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden."
                },

                new RuleTexts
                {
                    RuleId = 5,
                    Language = ELanguage.English,
                    RuleStr = "If you are not sure if the time for e.g. Mute or Bann could be too high,"
                        + "\nask your team leader - if you can't reach someone quickly, choose a lower time."
                        + "\nToo high times are bad, too low times are no problem."
                },
                new RuleTexts
                {
                    RuleId = 5,
                    Language = ELanguage.German,
                    RuleStr = "Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,"
                        + "\nfrage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit."
                        + "\nZu hohe Zeiten sind schlecht, zu niedrige kein Problem."
                },

                new RuleTexts
                {
                    RuleId = 6,
                    Language = ELanguage.English,
                    RuleStr = "All admin rules with the exception of activity duty are also valid for VIPs."
                },
                new RuleTexts
                {
                    RuleId = 6,
                    Language = ELanguage.German,
                    RuleStr = "Alle Admin-Regeln mit Ausnahme von Aktivitäts-Pflicht sind auch gültig für VIPs."
                },

                new RuleTexts
                {
                    RuleId = 7,
                    Language = ELanguage.English,
                    RuleStr = "The VIPs are free to decide whether they want to use their rights or not."
                },
                new RuleTexts
                {
                    RuleId = 7,
                    Language = ELanguage.German,
                    RuleStr = "Den VIPs ist es frei überlassen, ob sie ihre Rechte nutzen wollen oder nicht."
                }
            );

            modelBuilder.Entity<FAQs>().HasData(
                new FAQs 
                { 
                    Id = 1,
                    Language = ELanguage.English,
                    Question = "How do I activate my cursor?",
                    Answer = "With the END key on your keyboard."
                },
                new FAQs
                {
                    Id = 1,
                    Language = ELanguage.German,
                    Question = "Wie aktiviere ich meinen Cursor?",
                    Answer = "Mit der ENDE Taste auf deiner Tastatur."
                },

                new FAQs
                {
                    Id = 2,
                    Language = ELanguage.English,
                    Question = "What is the 'Allow data transfer' setting in the userpanel?",
                    Answer = "In case of a transfer of TDS-V, the database will also be transferred, but without the player data (for data protection reasons)."
                            + "\nHowever, if you want to keep your data, you must allow it in the user panel."
                            + "\nThe data does not contain any sensitive information - IPs are not stored, passwords are secure (hash + salt)."
                },
                new FAQs
                {
                    Id = 2,
                    Language = ELanguage.German,
                    Question = "Was ist die 'Erlaube Daten-Transfer' Einstellung im Userpanel?",
                    Answer = "Im Falle einer Übergabe von TDS-V wird die Datenbank auch übergeben, jedoch ohne die Spieler-Daten (aus Datenschutz-Gründen)."
                            + "\nFalls du jedoch deine Daten auch dann weiterhin behalten willst, musst du es im Userpanel erlauben."
                            + "\nDie Daten beinhalten keine sensiblen Informationen - IPs werden nicht gespeichert, Passwörter sind sicher (Hash + Salt)."
                }
            );
            #endregion

            #region Autoincrement
            /* Use this code at the before the first InsertData in the Up Method in the migration.
            migrationBuilder.Sql("ALTER TABLE gangs ALTER COLUMN \"ID\" DROP IDENTITY");
            migrationBuilder.Sql("ALTER TABLE lobbies ALTER COLUMN \"ID\" DROP IDENTITY");
            migrationBuilder.Sql("ALTER TABLE maps ALTER COLUMN \"ID\" DROP IDENTITY");
            migrationBuilder.Sql("ALTER TABLE players ALTER COLUMN \"ID\" DROP IDENTITY");
            migrationBuilder.Sql("ALTER TABLE commands ALTER COLUMN \"ID\" DROP IDENTITY");
            migrationBuilder.Sql("ALTER TABLE teams ALTER COLUMN \"ID\" DROP IDENTITY");
            migrationBuilder.Sql("ALTER TABLE server_settings ALTER COLUMN \"ID\" DROP IDENTITY");
            */

            /* Use this code at the END (or atleast after all InsertDatas) of the Up Method in the migration.
               Maybe modify the "START WITH" numbers if you added more default rows.
            migrationBuilder.Sql("ALTER TABLE gangs ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY");
            migrationBuilder.Sql("ALTER TABLE lobbies ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY (START WITH 3)");
            migrationBuilder.Sql("ALTER TABLE maps ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY");
            migrationBuilder.Sql("ALTER TABLE players ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY");
            migrationBuilder.Sql("ALTER TABLE commands ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY (START WITH 20)");
            migrationBuilder.Sql("ALTER TABLE teams ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY (START WITH 5)");
            migrationBuilder.Sql("ALTER TABLE server_settings ALTER COLUMN \"ID\" ADD GENERATED ALWAYS AS IDENTITY");
            */


            // Sql("DBCC CHECKIDENT ('Offers', RESEED, 100);");

            /*modelBuilder.HasSequence<int>("players_ID_seq");

            modelBuilder.HasSequence<int>("lobbies_ID_seq")
                .StartsAt(seedLobbies.Count(l => l.Id > 0));

            modelBuilder.HasSequence<int>("teams_ID_seq")
                .StartsAt(seedTeams.Count(t => t.Id > 0));

            modelBuilder.HasSequence<int>("gangs_ID_seq");

            modelBuilder.HasSequence<int>("maps_ID_seq");*/
            #endregion

            IsConfigured = true;
        }
    }
}
