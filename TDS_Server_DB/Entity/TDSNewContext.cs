using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using TDS_Common.Enum;
using TDS_Common.Enum.Userpanel;
using TDS_Server_DB.Entity.Admin;
using TDS_Server_DB.Entity.Command;
using TDS_Server_DB.Entity.Gang;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Log;
using TDS_Server_DB.Entity.Player;
using TDS_Server_DB.Entity.Rest;
using TDS_Server_DB.Entity.Server;
using TDS_Server_DB.Entity.Userpanel;

namespace TDS_Server_DB.Entity
{
    public partial class TDSNewContext : DbContext
    {
        public static bool IsConfigured { get; private set; }

        private static string _connectionString;

        public TDSNewContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TDSNewContext()
        {
        }

        public TDSNewContext(DbContextOptions<TDSNewContext> options)
            : base(options)
        {
        }

        static TDSNewContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EPlayerRelation>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EWeaponHash>();
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
        }

        public virtual DbSet<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual DbSet<AdminLevels> AdminLevels { get; set; }
        public virtual DbSet<ApplicationAnswers> ApplicationAnswers { get; set; }
        public virtual DbSet<ApplicationInvitations> ApplicationInvitations { get; set; }
        public virtual DbSet<ApplicationQuestions> ApplicationQuestions { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
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
        public virtual DbSet<LogRests> LogRests { get; set; }
        public virtual DbSet<Maps> Maps { get; set; }
        public virtual DbSet<Offlinemessages> Offlinemessages { get; set; }
        public virtual DbSet<PlayerBans> PlayerBans { get; set; }
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
                    .UseNpgsql(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseIdentityByDefaultColumns();

            #region Enum
            modelBuilder.HasPostgresEnum<EPlayerRelation>();
            modelBuilder.HasPostgresEnum<EWeaponHash>();
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
            #endregion

            #region Tables
            modelBuilder.Entity<AdminLevels>(entity =>
            {
                entity.HasKey(e => e.Level)
                    .HasName("admin_levels_pkey");

                entity.ToTable("admin_levels");

                entity.Property(e => e.Level).ValueGeneratedNever();
            });

            modelBuilder.Entity<AdminLevelNames>(entity =>
            {
                entity.HasKey(e => new { e.Level, e.Language });

                entity.ToTable("admin_level_names");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.LevelNavigation)
                    .WithMany(p => p.AdminLevelNames)
                    .HasForeignKey(d => d.Level)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_admin_level_names_admin_level");
            });

            modelBuilder.Entity<ApplicationAnswers>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.QuestionId });

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.ToTable("application_answers");

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

                entity.Property(e => e.Id).HasColumnName("ID").UseIdentityAlwaysColumn();
                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.ToTable("application_invitations");

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

                entity.Property(e => e.Id).HasColumnName("ID").UseIdentityAlwaysColumn();

                entity.ToTable("application_questions");

                entity.HasOne(question => question.Admin)
                    .WithMany(admin => admin.ApplicationQuestions)
                    .HasForeignKey(answer => answer.AdminId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ID").UseIdentityAlwaysColumn();

                entity.ToTable("applications");

                entity.Property(e => e.CreateTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(app => app.Player)
                    .WithOne(player => player.Application)
                    .HasForeignKey<Applications>(app => app.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CommandAlias>(entity =>
            {
                entity.HasKey(e => new { e.Alias, e.Command })
                    .HasName("command_alias_pkey");

                entity.ToTable("command_alias");

                entity.Property(e => e.Alias).HasMaxLength(100);

                entity.HasOne(d => d.CommandNavigation)
                    .WithMany(p => p.CommandAlias)
                    .HasForeignKey(d => d.Command)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("command_alias_Command_fkey");
            });

            modelBuilder.Entity<CommandInfos>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Language })
                    .HasName("command_infos_pkey");

                entity.ToTable("command_infos");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.CommandInfos)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("command_infos_ID_fkey");
            });

            modelBuilder.Entity<Commands>(entity =>
            {
                entity.ToTable("commands");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.NeededAdminLevelNavigation)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.NeededAdminLevel)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_commands_admin_levels");
            });

            modelBuilder.Entity<FAQs>(entity =>
            {
                entity.ToTable("faqs");

                entity.HasKey(e => new { e.Id, e.Language });
            });

            modelBuilder.Entity<FreeroamDefaultVehicle>(entity =>
            {
                entity.HasKey(e => e.VehicleType);

                entity.ToTable("freeroam_default_vehicle");

                entity.Property(e => e.VehicleType);

                entity.Property(e => e.Note).HasColumnType("character varying");
            });

            modelBuilder.Entity<GangMembers>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.ToTable("gang_members");

                entity.Property(e => e.GangId).HasColumnName("GangID");
                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

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

                entity.ToTable("gang_rank_permissions");

                entity.Property(e => e.GangId).HasColumnName("GangID");

                entity.HasOne(e => e.Gang)
                    .WithOne(g => g.RankPermissions)
                    .HasForeignKey<GangRankPermissions>(e => e.GangId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GangRanks>(entity =>
            {
                entity.HasKey(e => new { e.GangId, e.Rank });

                entity.ToTable("gang_ranks");

                entity.Property(e => e.GangId).HasColumnName("GangID");

                entity.HasOne(e => e.Gang)
                    .WithMany(g => g.Ranks)
                    .HasForeignKey(e => e.GangId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Gangs>(entity =>
            {
                entity.ToTable("gangs");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Short)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.OwnerId)
                    .IsRequired(false);

                entity.Property(e => e.CreateTime)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.Property(e => e.TeamId).HasColumnName("TeamId");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Gangs)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("gangs_TeamId_fkey");

                entity.HasOne(d => d.Owner)
                    .WithOne(o => o.OwnedGang)
                    .HasForeignKey<Gangs>(o => o.OwnerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<GangwarAreas>(entity =>
            {
                entity.ToTable("gangwar_areas");

                entity.HasKey(e => e.MapId);

                entity.Property(e => e.MapId)
                    .HasColumnName("MapID");

                entity.Property(e => e.OwnerGangId)
                    .HasColumnName("OwnerGangID");

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
                entity.HasKey(e => new { e.LobbyId, e.KillsAmount })
                    .HasName("killingspree_rewards_pkey");

                entity.ToTable("killingspree_rewards");

                entity.Property(e => e.KillsAmount).ValueGeneratedNever();

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.LobbyKillingspreeRewards)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_killingspree_rewards_LobbyID_fkey");
            });

            modelBuilder.Entity<Lobbies>(entity =>
            {
                entity.ToTable("lobbies");

                entity.Property(e => e.Id).HasColumnName("ID").UseIdentityAlwaysColumn();

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
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobbies_Owner_fkey");
            });

            modelBuilder.Entity<LobbyMaps>(entity =>
            {
                entity.HasKey(e => new { e.LobbyId, e.MapId })
                    .HasName("lobby_maps_pkey");

                entity.ToTable("lobby_maps");

                entity.HasIndex(e => e.MapId)
                    .HasName("fki_FK_lobby_maps_maps");

                entity.Property(e => e.LobbyId).HasColumnName("LobbyID");

                entity.Property(e => e.MapId).HasColumnName("MapID");

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_maps_LobbyID_fkey");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_lobby_maps_maps");
            });

            modelBuilder.Entity<LobbyRewards>(entity =>
            {
                entity.HasKey(e => e.LobbyId)
                    .HasName("lobby_rewards_pkey");

                entity.ToTable("lobby_rewards");

                entity.Property(e => e.LobbyId)
                    .HasColumnName("LobbyID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Lobby)
                    .WithOne(p => p.LobbyRewards)
                    .HasForeignKey<LobbyRewards>(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_rewards_LobbyID_fkey");
            });

            modelBuilder.Entity<LobbyRoundSettings>(entity =>
            {
                entity.HasKey(e => e.LobbyId)
                    .HasName("lobby_round_infos_pkey");

                entity.ToTable("lobby_round_settings");

                entity.Property(e => e.LobbyId)
                    .HasColumnName("LobbyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BombDefuseTimeMs).HasDefaultValueSql("8000");

                entity.Property(e => e.BombDetonateTimeMs).HasDefaultValueSql("45000");

                entity.Property(e => e.BombPlantTimeMs).HasDefaultValueSql("3000");

                entity.Property(e => e.CountdownTime).HasDefaultValueSql("5");

                entity.Property(e => e.RoundTime).HasDefaultValueSql("240");

                entity.HasOne(d => d.Lobby)
                    .WithOne(p => p.LobbyRoundSettings)
                    .HasForeignKey<LobbyRoundSettings>(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_round_infos_LobbyID_fkey");
            });

            modelBuilder.Entity<LobbyMapSettings>(entity =>
            {
                entity.HasKey(e => e.LobbyId)
                    .HasName("lobby_map_settings_pkey");

                entity.ToTable("lobby_map_settings");

                entity.Property(e => e.LobbyId)
                   .HasColumnName("LobbyID")
                   .ValueGeneratedNever();

                entity.Property(e => e.MapLimitTime).HasDefaultValueSql("10");

                entity.HasOne(d => d.Lobby)
                    .WithOne(p => p.LobbyMapSettings)
                    .HasForeignKey<LobbyMapSettings>(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_map_settings_LobbyID_fkey");
            });

            modelBuilder.Entity<LobbyWeapons>(entity =>
            {
                entity.HasKey(e => new { e.Hash, e.Lobby })
                    .HasName("lobby_weapons_pkey");

                entity.ToTable("lobby_weapons");

                entity.Property(e => e.Hash).ValueGeneratedNever();

                entity.HasOne(d => d.HashNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Hash)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_weapons_Hash_fkey");

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Lobby)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lobby_weapons_Lobby_fkey");
            });

            modelBuilder.Entity<LogAdmins>(entity =>
            {
                entity.ToTable("log_admins");

                entity.Property(e => e.Id).HasColumnName("ID").UseHiLo();

                entity.Property(e => e.AsVip).HasColumnName("AsVIP");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
                entity.Property(e => e.LengthOrEndTime).IsRequired(false);
            });

            modelBuilder.Entity<LogChats>(entity =>
            {
                entity.ToTable("log_chats");

                entity.Property(e => e.Id).HasColumnName("ID").UseHiLo();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<LogErrors>(entity =>
            {
                entity.ToTable("log_errors");

                entity.Property(e => e.Id).HasColumnName("ID").UseHiLo();

                entity.Property(e => e.Info).IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<LogRests>(entity =>
            {
                entity.ToTable("log_rests");

                entity.Property(e => e.Id).HasColumnName("ID").UseHiLo();

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.Serial).HasMaxLength(200);

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");
            });

            modelBuilder.Entity<Maps>(entity =>
            {
                entity.ToTable("maps");

                entity.HasIndex(e => e.Name)
                    .HasName("Index_maps_name")
                    .HasMethod("hash");

                entity.Property(e => e.Id).HasColumnName("ID").UseIdentityAlwaysColumn();

                entity.Property(e => e.CreateTimestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Maps)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("maps_CreatorID_fkey");
            });

            modelBuilder.Entity<Offlinemessages>(entity =>
            {
                entity.ToTable("offlinemessages");

                entity.Property(e => e.Id)
                    .HasColumnName("ID").UseIdentityAlwaysColumn();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.TargetId).HasColumnName("TargetID");

                entity.Property(e => e.Timestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.OfflinemessagesSource)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("offlinemessages_SourceID_fkey");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.OfflinemessagesTarget)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("offlinemessages_TargetID_fkey");
            });

            modelBuilder.Entity<PlayerBans>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.LobbyId })
                    .HasName("player_bans_pkey");

                entity.HasIndex(e => e.IP);
                entity.HasIndex(e => e.SCName);
                entity.HasIndex(e => e.SCId);
                entity.HasIndex(e => e.Serial);

                entity.ToTable("player_bans");

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
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("player_bans_AdminID_fkey");

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.PlayerBans)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_bans_LobbyID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerBansPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("player_bans_PlayerID_fkey");
            });

            modelBuilder.Entity<PlayerClothes>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.ToTable("player_clothes");

                entity.HasOne(c => c.Player)
                    .WithOne(p => p.PlayerClothes)
                    .HasForeignKey<PlayerClothes>(c => c.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PlayerLobbyStats>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.LobbyId })
                    .HasName("player_lobby_stats_pkey");

                entity.ToTable("player_lobby_stats");

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.LobbyId).HasColumnName("LobbyID");

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.PlayerLobbyStats)
                    .HasForeignKey(d => d.LobbyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_lobby_stats_LobbyID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerLobbyStats)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_lobby_stats_PlayerID_fkey");
            });

            modelBuilder.Entity<PlayerMapFavourites>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.MapId })
                    .HasName("player_map_favourites_pkey");

                entity.ToTable("player_map_favourites");

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.MapId).HasColumnName("MapID");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.PlayerMapFavourites)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_map_favourites_MapID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMapFavourites)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_map_favourites_PlayerID_fkey");
            });

            modelBuilder.Entity<PlayerMapRatings>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.MapId })
                    .HasName("player_map_ratings_pkey");

                entity.ToTable("player_map_ratings");

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.MapId).HasColumnName("MapID");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.PlayerMapRatings)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_map_ratings_MapID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMapRatings)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_map_ratings_PlayerID_fkey");
            });

            modelBuilder.Entity<PlayerRelations>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.TargetId })
                    .HasName("player_relations_pkey");

                entity.Property(e => e.Relation).IsRequired();

                entity.ToTable("player_relations");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerRelationsPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_relations_PlayerId_fkey");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.PlayerRelationsTarget)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_relations_TargetId_fkey");
            });

            modelBuilder.Entity<PlayerSettings>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("player_settings_pkey");

                entity.ToTable("player_settings");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("PlayerID")
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
                entity.Property(e => e.DiscordIdentity);
                entity.Property(e => e.TimeZone)
                    .HasDefaultValue("UTC");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerSettings)
                    .HasForeignKey<PlayerSettings>(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_settings_PlayerID_fkey");
            });

            modelBuilder.Entity<PlayerStats>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("player_stats_pkey");

                entity.ToTable("player_stats");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("PlayerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.LastLoginTimestamp)
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("timezone('utc', now())");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerStats)
                    .HasForeignKey<PlayerStats>(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("player_stats_PlayerID_fkey");
            });

            modelBuilder.Entity<PlayerTotalStats>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("player_total_stats_pkey");

                entity.ToTable("player_total_stats");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("PlayerID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Player)
                   .WithOne(p => p.PlayerTotalStats)
                   .HasForeignKey<PlayerTotalStats>(d => d.PlayerId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("player_total_stats_PlayerID_fkey");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.ToTable("players");

                entity.HasKey(e => e.Id).HasName("PK_players");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsVip).HasColumnName("IsVIP").HasDefaultValue(false);

                entity.Property(e => e.Donation).HasDefaultValue(0);

                entity.Property(e => e.AdminLvl).HasDefaultValue(0);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AdminLeaderId)
                    .HasColumnName("AdminLeaderID")
                    .IsRequired(false);

                entity.Property(e => e.RegisterTimestamp)
                    .HasDefaultValueSql("timezone('utc', now())")
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

                entity.Property(e => e.SCName)
                    .IsRequired()
                    .HasColumnName("SCName")
                    .HasMaxLength(255);

                entity.Property(e => e.SCId)
                    .IsRequired()
                    .HasColumnName("SCID");

                entity.HasOne(d => d.AdminLvlNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.AdminLvl)
                    .HasConstraintName("players_AdminLvl_fkey")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.AdminLeader)
                    .WithMany(p => p.AdminMembers)
                    .HasForeignKey(p => p.AdminLeaderId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Rules>(entity =>
            {
                entity.ToTable("rules");

                entity.Property(e => e.Id)
                    .HasColumnName("ID");
            });

            modelBuilder.Entity<RuleTexts>(entity =>
            {
                entity.ToTable("rule_texts");

                entity.HasKey(e => new { e.RuleId, e.Language });
                entity.Property(e => e.RuleId)
                    .HasColumnName("RuleID");

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.RuleTexts)
                    .HasForeignKey(d => d.RuleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ServerDailyStats>(entity =>
            {
                entity.ToTable("server_daily_stats");

                entity.HasKey(e => e.Date).HasName("server_daily_stats_date_pkey");

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
                entity.ToTable("server_settings");

                entity.Property(e => e.Id)
                    .HasColumnName("ID");

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

                entity.Property(e => e.GangwarPreparationTimeMs)
                    .IsRequired()
                    .HasDefaultValue(3 * 60 * 1000);

                entity.Property(e => e.GangwarActionTimeMs)
                    .IsRequired()
                    .HasDefaultValue(15 * 60 * 1000);

                entity.Property(e => e.DeleteRequestsDaysAfterClose)
                    .IsRequired()
                    .HasDefaultValue(30);

                entity.Property(e => e.MinPlayersOnlineForGangwar)
                    .IsRequired()
                    .HasDefaultValue(3);

                entity.Property(e => e.GangwarAreaAttackCooldownMinutes)
                    .IsRequired()
                    .HasDefaultValue(60);
            });

            modelBuilder.Entity<ServerTotalStats>(entity =>
            {
                entity.ToTable("server_total_stats");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.PlayerPeak).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.ArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.CustomArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
            });

            modelBuilder.Entity<SupportRequests>(entity =>
            {
                entity.ToTable("support_requests");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ID");

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
                entity.ToTable("support_request_messages");

                entity.HasKey(e => new { e.RequestId, e.MessageIndex });

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

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
                entity.ToTable("teams");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
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
                    
                entity.Property(e => e.ColorR)
                    .HasDefaultValue(255);

                entity.Property(e => e.ColorG)
                    .HasDefaultValue(255);

                entity.Property(e => e.ColorB)
                    .HasDefaultValue(255);

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Lobby)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("teams_Lobby_fkey");
            });


            modelBuilder.Entity<Weapons>(entity =>
            {
                entity.HasKey(e => e.Hash)
                    .HasName("weapons_pkey");

                entity.ToTable("weapons");

                entity.Property(e => e.Hash).IsRequired();

                entity.Property(e => e.DefaultHeadMultiplicator).HasDefaultValueSql("1");
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
                new Commands { Id = 24, Command = "GiveMoney" }
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
                new CommandAlias { Alias = "MoneySend", Command = 24 }
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
                new CommandInfos { Id = 24, Language = ELanguage.English, Info = "Gives money to a player." }
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
                new LobbyRoundSettings { LobbyId = -1, RoundTime = 240, CountdownTime = 5, BombDetonateTimeMs = 45000, BombDefuseTimeMs = 8000, BombPlantTimeMs = 3000, MixTeamsAfterRound = true }
            );

            modelBuilder.Entity<LobbyMapSettings>().HasData(
                new LobbyMapSettings { LobbyId = -1, MapLimitTime = 10, MapLimitType = EMapLimitType.KillAfterTime }  
            );

            modelBuilder.Entity<Weapons>().HasData(
                new Weapons { Hash = EWeaponHash.SniperRifle, Type = EWeaponType.SniperRifle, DefaultDamage = 101, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.FireExtinguisher, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CompactGrenadeLauncher, Type = EWeaponType.HeavyWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Snowball, Type = EWeaponType.ThrownWeapon, DefaultDamage = 10, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.VintagePistol, Type = EWeaponType.Handgun, DefaultDamage = 34, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CombatPDW, Type = EWeaponType.MachineGun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HeavySniper, Type = EWeaponType.SniperRifle, DefaultDamage = 216, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.HeavySniperMk2, Type = EWeaponType.SniperRifle, DefaultDamage = 216, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.SweeperShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 162, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MicroSMG, Type = EWeaponType.MachineGun, DefaultDamage = 21, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Wrench, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Pistol, Type = EWeaponType.Handgun, DefaultDamage = 26, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PistolMk2, Type = EWeaponType.Handgun, DefaultDamage = 26, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PumpShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 58, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PumpShotgunMk2, Type = EWeaponType.Shotgun, DefaultDamage = 58, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.APPistol, Type = EWeaponType.Handgun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Baseball, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Molotov, Type = EWeaponType.ThrownWeapon, DefaultDamage = 10, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SMG, Type = EWeaponType.MachineGun, DefaultDamage = 22, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SMGMk2, Type = EWeaponType.MachineGun, DefaultDamage = 22, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.StickyBomb, Type = EWeaponType.ThrownWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PetrolCan, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.StunGun, Type = EWeaponType.Handgun, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HeavyShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 117, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Minigun, Type = EWeaponType.HeavyWeapon, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.GolfClub, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.FlareGun, Type = EWeaponType.Handgun, DefaultDamage = 50, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Flare, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.GrenadeLauncherSmoke, Type = EWeaponType.HeavyWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Hammer, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CombatPistol, Type = EWeaponType.Handgun, DefaultDamage = 27, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Gusenberg, Type = EWeaponType.MachineGun, DefaultDamage = 34, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CompactRifle, Type = EWeaponType.AssaultRifle, DefaultDamage = 34, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HomingLauncher, Type = EWeaponType.HeavyWeapon, DefaultDamage = 150, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Nightstick, Type = EWeaponType.Melee, DefaultDamage = 35, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Railgun, Type = EWeaponType.HeavyWeapon, DefaultDamage = 50, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SawnOffShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 160, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.BullpupRifle, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Firework, Type = EWeaponType.HeavyWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CombatMG, Type = EWeaponType.MachineGun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CombatMGMk2, Type = EWeaponType.MachineGun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CarbineRifle, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Crowbar, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Flashlight, Type = EWeaponType.Melee, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Dagger, Type = EWeaponType.Melee, DefaultDamage = 45, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Grenade, Type = EWeaponType.ThrownWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PoolCue, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Bat, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Pistol50, Type = EWeaponType.Handgun, DefaultDamage = 51, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Knife, Type = EWeaponType.Melee, DefaultDamage = 45, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MG, Type = EWeaponType.MachineGun, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.BullpupShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 112, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.BZGas, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Unarmed, Type = EWeaponType.Melee, DefaultDamage = 15, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.GrenadeLauncher, Type = EWeaponType.HeavyWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.NightVision, Type = EWeaponType.Rest, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Musket, Type = EWeaponType.Shotgun, DefaultDamage = 165, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.ProximityMine, Type = EWeaponType.ThrownWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.AdvancedRifle, Type = EWeaponType.AssaultRifle, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.RPG, Type = EWeaponType.HeavyWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PipeBomb, Type = EWeaponType.ThrownWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MiniSMG, Type = EWeaponType.MachineGun, DefaultDamage = 22, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SNSPistol, Type = EWeaponType.Handgun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SNSPistolMk2, Type = EWeaponType.Handgun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.AssaultRifle, Type = EWeaponType.AssaultRifle, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.AssaultRifleMk2, Type = EWeaponType.AssaultRifle, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SpecialCarbine, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HeavyRevolver, Type = EWeaponType.Handgun, DefaultDamage = 110, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HeavyRevolverMk2, Type = EWeaponType.Handgun, DefaultDamage = 110, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.DoubleActionRevolver, Type = EWeaponType.Handgun, DefaultDamage = 110, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MarksmanRifle, Type = EWeaponType.SniperRifle, DefaultDamage = 65, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.MarksmanRifleMk2, Type = EWeaponType.SniperRifle, DefaultDamage = 65, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.BattleAxe, Type = EWeaponType.Melee, DefaultDamage = 50, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HeavyPistol, Type = EWeaponType.Handgun, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.KnuckleDuster, Type = EWeaponType.Melee, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MachinePistol, Type = EWeaponType.MachineGun, DefaultDamage = 20, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MarksmanPistol, Type = EWeaponType.Handgun, DefaultDamage = 150, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Machete, Type = EWeaponType.Melee, DefaultDamage = 45, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SwitchBlade, Type = EWeaponType.Melee, DefaultDamage = 50, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.AssaultShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 192, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.DoubleBarrelShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 166, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.AssaultSMG, Type = EWeaponType.MachineGun, DefaultDamage = 23, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Hatchet, Type = EWeaponType.Melee, DefaultDamage = 50, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Bottle, Type = EWeaponType.ThrownWeapon, DefaultDamage = 10, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Parachute, Type = EWeaponType.Rest, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SmokeGrenade, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.UpnAtomizer, Type = EWeaponType.Handgun, DefaultDamage = 80, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.UnholyHellbringer, Type = EWeaponType.MachineGun, DefaultDamage = 23, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CarbineRifleMK2, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Widowmaker, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 }
            );

            modelBuilder.Entity<LobbyWeapons>().HasData(
                new LobbyWeapons { Hash = EWeaponHash.SniperRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.FireExtinguisher, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CompactGrenadeLauncher, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Snowball, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.VintagePistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CombatPDW, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HeavySniper, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HeavySniperMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SweeperShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MicroSMG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Wrench, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Pistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.PistolMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.PumpShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.PumpShotgunMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.APPistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Baseball, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Molotov, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SMG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SMGMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.StickyBomb, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.PetrolCan, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.StunGun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HeavyShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Minigun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.GolfClub, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.FlareGun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Flare, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.GrenadeLauncherSmoke, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Hammer, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CombatPistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Gusenberg, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CompactRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HomingLauncher, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Nightstick, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Railgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SawnOffShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.BullpupRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Firework, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CombatMG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CombatMGMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CarbineRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Crowbar, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Flashlight, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Dagger, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Grenade, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.PoolCue, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Bat, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Pistol50, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Knife, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.BullpupShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.BZGas, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.GrenadeLauncher, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.NightVision, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Musket, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.ProximityMine, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.AdvancedRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.RPG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.PipeBomb, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MiniSMG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SNSPistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SNSPistolMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.AssaultRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.AssaultRifleMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SpecialCarbine, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HeavyRevolver, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HeavyRevolverMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.DoubleActionRevolver, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MarksmanRifle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MarksmanRifleMk2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.BattleAxe, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.HeavyPistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.KnuckleDuster, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MachinePistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.MarksmanPistol, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Machete, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SwitchBlade, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.AssaultShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.DoubleBarrelShotgun, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.AssaultSMG, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Hatchet, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Bottle, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Parachute, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.SmokeGrenade, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.UpnAtomizer, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.UnholyHellbringer, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.CarbineRifleMK2, Lobby = -1, Ammo = 99999 },
                new LobbyWeapons { Hash = EWeaponHash.Widowmaker, Lobby = -1, Ammo = 99999 }
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
