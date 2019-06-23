using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Npgsql;
using TDS_Common.Enum;

/**
 * Rules on migration:
 * 1. Add migration in Package Manager Console with: "Add-Migration [name]"
 * 2. Use all the migrations with "Update-Database"
 * 3. Before using the first migration:

        1. use this sql code before ALL InsertData, BUT AFTER Admin_Levels!!, in the migration file (before you can't add ID 0):
migrationBuilder.Sql("INSERT INTO players (\"ID\", \"SCName\", \"Name\", \"Password\") VALUES (0, 'System', 'System', '-')");
migrationBuilder.Sql("INSERT INTO lobbies (\"ID\", \"OwnerId\", \"Type\", \"Name\", \"IsTemporary\", \"IsOfficial\", \"AmountLifes\", \"SpawnAgainAfterDeathMs\") " +
    "VALUES (0, 0, 'main_menu', 'MainMenu', FALSE, TRUE, 0, 0)");
migrationBuilder.Sql("INSERT INTO teams (\"ID\", \"Index\", \"Name\", \"Lobby\", \"ColorR\", \"ColorG\", \"ColorB\", \"BlipColor\", \"SkinHash\") " +
    "VALUES (0, 0, 'Spectator', 0, 255, 255, 255, 4, 1004114196)");
migrationBuilder.Sql("INSERT INTO gangs (\"ID\", \"TeamId\", \"Short\") VALUES (0, 0, '-')");

        2. Use this code at the END (or atleast after all InsertDatas) of the Up Method in the migration.
           Maybe modify the "START WITH" numbers if you added more default rows.
migrationBuilder.Sql("ALTER TABLE gangs ALTER COLUMN \"ID\" SET GENERATED ALWAYS");
migrationBuilder.Sql("ALTER TABLE lobbies ALTER COLUMN \"ID\" SET GENERATED ALWAYS RESTART WITH 3");
migrationBuilder.Sql("ALTER TABLE maps ALTER COLUMN \"ID\" SET GENERATED ALWAYS");
migrationBuilder.Sql("ALTER TABLE players ALTER COLUMN \"ID\" SET GENERATED ALWAYS");
migrationBuilder.Sql("ALTER TABLE commands ALTER COLUMN \"ID\" SET GENERATED ALWAYS RESTART WITH 20");
migrationBuilder.Sql("ALTER TABLE teams ALTER COLUMN \"ID\" SET GENERATED ALWAYS RESTART WITH 5");
migrationBuilder.Sql("ALTER TABLE server_settings ALTER COLUMN \"ID\" SET GENERATED ALWAYS");

 */

namespace TDS_Server_DB.Entity
{
    public partial class TDSNewContext : DbContext
    {
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
        }

        public virtual DbSet<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual DbSet<AdminLevels> AdminLevels { get; set; }
        public virtual DbSet<CommandAlias> CommandAlias { get; set; }
        public virtual DbSet<CommandInfos> CommandInfos { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<FreeroamDefaultVehicle> FreeroamDefaultVehicle { get; set; }
        public virtual DbSet<Gangs> Gangs { get; set; }
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
        public virtual DbSet<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual DbSet<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual DbSet<PlayerMapRatings> PlayerMapRatings { get; set; }
        public virtual DbSet<PlayerRelations> PlayerRelations { get; set; }
        public virtual DbSet<PlayerSettings> PlayerSettings { get; set; }
        public virtual DbSet<PlayerStats> PlayerStats { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<ServerSettings> ServerSettings { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<Weapons> Weapons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /*optionsBuilder.EnableSensitiveDataLogging();
                var loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddConsole().SetMinimumLevel(LogLevel.Debug)
                );*/

                optionsBuilder
                    //.UseLoggerFactory(loggerFactory)
                    //.EnableSensitiveDataLogging()
                    //.UseNpgsql("Server=localhost;Database=TDSV;User ID=tdsv;Password=ajagrebo;");
                    .UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ForNpgsqlUseIdentityByDefaultColumns();

            #region Enum
            modelBuilder.ForNpgsqlHasEnum<EPlayerRelation>();
            modelBuilder.ForNpgsqlHasEnum<EWeaponHash>();
            modelBuilder.ForNpgsqlHasEnum<EWeaponType>();
            modelBuilder.ForNpgsqlHasEnum<ELogType>();
            modelBuilder.ForNpgsqlHasEnum<ELobbyType>();
            modelBuilder.ForNpgsqlHasEnum<ELanguage>();
            modelBuilder.ForNpgsqlHasEnum<VehicleHash>();
            modelBuilder.ForNpgsqlHasEnum<EFreeroamVehicleType>();
            #endregion

            #region Tables
            modelBuilder.Entity<AdminLevels>(entity =>
            {
                entity.HasKey(e => e.Level)
                    .HasName("admin_levels_pkey");

                entity.ToTable("admin_levels");

                entity.Property(e => e.Level).ValueGeneratedNever();
            });

            modelBuilder.HasAnnotation("ProductVersion", "3.0.0-preview5.19227.1");

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
                    .HasConstraintName("FK_admin_level_names_admin_level");
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
                    .HasConstraintName("FK_commands_admin_levels");
            });

            modelBuilder.Entity<FreeroamDefaultVehicle>(entity =>
            {
                entity.HasKey(e => e.VehicleType);

                entity.ToTable("freeroam_default_vehicle");

                entity.Property(e => e.VehicleType);

                entity.Property(e => e.Note).HasColumnType("character varying");
            });

            modelBuilder.Entity<Gangs>(entity =>
            {
                entity.ToTable("gangs");

                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Short)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.TeamId).HasColumnName("TeamId");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Gangs)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("gangs_TeamId_fkey");
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
                    .HasConstraintName("lobby_killingspree_rewards_LobbyID_fkey");
            });

            modelBuilder.Entity<Lobbies>(entity =>
            {
                entity.ToTable("lobbies");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AroundSpawnPoint).HasDefaultValueSql("3");

                entity.Property(e => e.CreateTimestamp).HasDefaultValueSql("now()");

                entity.Property(e => e.DefaultSpawnX).HasDefaultValueSql("0");
                entity.Property(e => e.DefaultSpawnY).HasDefaultValueSql("0");
                entity.Property(e => e.DefaultSpawnZ).HasDefaultValueSql("900");
                entity.Property(e => e.DefaultSpawnRotation).HasDefaultValueSql("0");
                entity.Property(e => e.IsTemporary);
                entity.Property(e => e.IsOfficial);

                entity.Property(e => e.DieAfterOutsideMapLimitTime).HasDefaultValueSql("10");

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
                    .HasConstraintName("lobby_maps_LobbyID_fkey");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.MapId)
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
                    .HasConstraintName("lobby_round_infos_LobbyID_fkey");
            });

            modelBuilder.Entity<LobbyWeapons>(entity =>
            {
                entity.HasKey(e => e.Hash)
                    .HasName("lobby_weapons_pkey");

                entity.ToTable("lobby_weapons");

                entity.Property(e => e.Hash).ValueGeneratedNever();

                entity.HasOne(d => d.HashNavigation)
                    .WithOne(p => p.LobbyWeapons)
                    .HasForeignKey<LobbyWeapons>(d => d.Hash)
                    .HasConstraintName("lobby_weapons_Hash_fkey");

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Lobby)
                    .HasConstraintName("lobby_weapons_Lobby_fkey");
            });

            modelBuilder.Entity<LogAdmins>(entity =>
            {
                entity.ToTable("log_admins");

                entity.Property(e => e.Id).HasColumnName("ID").ForNpgsqlUseSequenceHiLo();

                entity.Property(e => e.AsVip).HasColumnName("AsVIP");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogChats>(entity =>
            {
                entity.ToTable("log_chats");

                entity.Property(e => e.Id).HasColumnName("ID").ForNpgsqlUseSequenceHiLo();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogErrors>(entity =>
            {
                entity.ToTable("log_errors");

                entity.Property(e => e.Id).HasColumnName("ID").ForNpgsqlUseSequenceHiLo();

                entity.Property(e => e.Info).IsRequired();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogRests>(entity =>
            {
                entity.ToTable("log_rests");

                entity.Property(e => e.Id).HasColumnName("ID").ForNpgsqlUseSequenceHiLo();

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.Serial).HasMaxLength(200);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Maps>(entity =>
            {
                entity.ToTable("maps");

                entity.HasIndex(e => e.Name)
                    .HasName("Index_maps_name")
                    .ForNpgsqlHasMethod("hash");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTimestamp).HasDefaultValueSql("now()");

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
                    .HasColumnName("ID")
                    .ForNpgsqlUseSequenceHiLo();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.TargetId).HasColumnName("TargetID");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.OfflinemessagesSource)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("offlinemessages_SourceID_fkey");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.OfflinemessagesTarget)
                    .HasForeignKey(d => d.TargetId)
                    .HasConstraintName("offlinemessages_TargetID_fkey");
            });

            modelBuilder.Entity<PlayerBans>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.LobbyId })
                    .HasName("player_bans_pkey");

                entity.ToTable("player_bans");

                entity.Property(e => e.EndTimestamp).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.StartTimestamp)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.PlayerBansAdmin)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("player_bans_AdminID_fkey");

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.PlayerBans)
                    .HasForeignKey(d => d.LobbyId)
                    .HasConstraintName("player_bans_LobbyID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerBansPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("player_bans_PlayerID_fkey");
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
                    .HasConstraintName("player_lobby_stats_LobbyID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerLobbyStats)
                    .HasForeignKey(d => d.PlayerId)
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
                    .HasConstraintName("player_map_favourites_MapID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMapFavourites)
                    .HasForeignKey(d => d.PlayerId)
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
                    .HasConstraintName("player_map_ratings_MapID_fkey");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMapRatings)
                    .HasForeignKey(d => d.PlayerId)
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
                    .HasConstraintName("player_relations_PlayerId_fkey");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.PlayerRelationsTarget)
                    .HasForeignKey(d => d.TargetId)
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

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerSettings)
                    .HasForeignKey<PlayerSettings>(d => d.PlayerId)
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

                entity.Property(e => e.LastLoginTimestamp).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.PlayerStats)
                    .HasForeignKey<PlayerStats>(d => d.PlayerId)
                    .HasConstraintName("player_stats_PlayerID_fkey");
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

                entity.Property(e => e.RegisterTimestamp)
                    .HasColumnType("timestamp(4) without time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SCName)
                    .IsRequired()
                    .HasColumnName("SCName")
                    .HasMaxLength(255);

                entity.HasOne(d => d.AdminLvlNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.AdminLvl)
                    .HasConstraintName("players_AdminLvl_fkey");

                entity.HasOne(d => d.Gang)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.GangId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("players_GangId_fkey");
            });

            modelBuilder.Entity<ServerSettings>(entity =>
            {
                entity.ToTable("server_settings");

                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.GamemodeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MapsPath)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.NewMapsPath)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.SavedMapsPath)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.ToTable("teams");

                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Lobby)
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
                new ServerSettings {  Id = 1, GamemodeName = "tdm", MapsPath = "bridge/resources/tds/maps/",
                    NewMapsPath = "bridge/resources/tds/newmaps/", SavedMapsPath = "bridge/resources/tds/savedmaps/",
                    ErrorToPlayerOnNonExistentCommand = true, ToChatOnNonExistentCommand = false,
                    DistanceToSpotToPlant = 3, DistanceToSpotToDefuse = 3,
                    SavePlayerDataCooldownMinutes = 1, SaveLogsCooldownMinutes = 1, SaveSeasonsCooldownMinutes = 1, TeamOrderCooldownMs = 3000,
                    ArenaNewMapProbabilityPercent = 2
                }
            );

            /*modelBuilder.Seed(new List<Players> {
                new Players { Id = 0, Scname = "System", Name = "System", Password = "" }
            });*/

            var seedLobbies = new List<Lobbies> {
                //new Lobbies { Id = 0, OwnerId = 0, Type = ELobbyType.MainMenu, Name = "MainMenu", IsTemporary = false, IsOfficial = true, SpawnAgainAfterDeathMs = 0 },
                new Lobbies { Id = 1, OwnerId = 0, Type = ELobbyType.Arena, Name = "Arena", IsTemporary = false, IsOfficial = true, AmountLifes = 1, SpawnAgainAfterDeathMs = 400, DieAfterOutsideMapLimitTime = 10 },
                new Lobbies { Id = 2, OwnerId = 0, Type = ELobbyType.GangLobby, Name = "GangLobby", IsTemporary = false, IsOfficial = true, AmountLifes = 1, SpawnAgainAfterDeathMs = 400 }
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
                new Commands { Id = 19, Command = "UserId" }
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
                new CommandAlias { Alias = "UID", Command = 19 }
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
                new CommandInfos { Id = 19, Language = ELanguage.English, Info = "Outputs your user-id to yourself." }
            );

            modelBuilder.Entity<FreeroamDefaultVehicle>().HasData(
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Car, VehicleHash = VehicleHash.Pfister811 },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Helicopter, VehicleHash = VehicleHash.AKULA },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Plane, VehicleHash = VehicleHash.Pyro },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Bike, VehicleHash = VehicleHash.Hakuchou2 },
                new FreeroamDefaultVehicle { VehicleType = EFreeroamVehicleType.Boat, VehicleHash = VehicleHash.Speeder2 }
            );

            modelBuilder.Entity<LobbyKillingspreeRewards>().HasData(
                new LobbyKillingspreeRewards { LobbyId = 1, KillsAmount = 3, HealthOrArmor = 30 },
                new LobbyKillingspreeRewards { LobbyId = 1, KillsAmount = 5, HealthOrArmor = 50 },
                new LobbyKillingspreeRewards { LobbyId = 1, KillsAmount = 10, HealthOrArmor = 100 },
                new LobbyKillingspreeRewards { LobbyId = 1, KillsAmount = 15, HealthOrArmor = 100 }
            );

            var seedTeams = new List<Teams> {
                //new Teams { Id = 0, Index = 0, Name = "Spectator", Lobby = 0, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 1004114196 },
                new Teams { Id = 1, Index = 0, Name = "Spectator", Lobby = 1, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 1004114196 },
                new Teams { Id = 2, Index = 1, Name = "SWAT", Lobby = 1, ColorR = 0, ColorG = 150, ColorB = 0, BlipColor = 52, SkinHash = -1920001264 },
                new Teams { Id = 3, Index = 2, Name = "Terrorist", Lobby = 1, ColorR = 150, ColorG = 0, ColorB = 0, BlipColor = 1, SkinHash = 275618457 },
                new Teams { Id = 4, Index = 0, Name = "None", Lobby = 2, ColorR = 255, ColorG = 255, ColorB = 255, BlipColor = 4, SkinHash = 1004114196 }
            };
            modelBuilder.Entity<Teams>().HasData(seedTeams);

            /*modelBuilder.Seed(new List<Gangs> {
                new Gangs { Id = 0, TeamId = 4, Short = "-" }
            });*/

            modelBuilder.Entity<LobbyMaps>().HasData(
                new LobbyMaps { LobbyId = 1, MapId = -1 }
            );

            modelBuilder.Entity<LobbyRewards>().HasData(
                new LobbyRewards { LobbyId = 1, MoneyPerKill = 20, MoneyPerAssist = 10, MoneyPerDamage = 0.1 },
                new LobbyRewards { LobbyId = 2, MoneyPerKill = 20, MoneyPerAssist = 10, MoneyPerDamage = 0.1 }
            );

            modelBuilder.Entity<LobbyRoundSettings>().HasData(
                new LobbyRoundSettings { LobbyId = 1, RoundTime = 240, CountdownTime = 5, BombDetonateTimeMs = 45000, BombDefuseTimeMs = 8000, BombPlantTimeMs = 3000, MixTeamsAfterRound = true }
            );

            modelBuilder.Entity<Weapons>().HasData(
                new Weapons { Hash = EWeaponHash.SniperRifle, Type = EWeaponType.SniperRifle, DefaultDamage = 101, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.FireExtinguisher, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CompactGrenadeLauncher, Type = EWeaponType.HeavyWeapon, DefaultDamage = 100, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Snowball, Type = EWeaponType.ThrownWeapon, DefaultDamage = 10, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.VintagePistol, Type = EWeaponType.Handgun, DefaultDamage = 34, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.CombatPDW, Type = EWeaponType.MachineGun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.HeavySniper, Type = EWeaponType.SniperRifle, DefaultDamage = 216, DefaultHeadMultiplicator = 2 },
                new Weapons { Hash = EWeaponHash.SweeperShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 162, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MicroSMG, Type = EWeaponType.MachineGun, DefaultDamage = 21, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Wrench, Type = EWeaponType.Melee, DefaultDamage = 40, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Pistol, Type = EWeaponType.Handgun, DefaultDamage = 26, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.PumpShotgun, Type = EWeaponType.Shotgun, DefaultDamage = 58, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.APPistol, Type = EWeaponType.Handgun, DefaultDamage = 28, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Ball, Type = EWeaponType.ThrownWeapon, DefaultDamage = 0, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Molotov, Type = EWeaponType.ThrownWeapon, DefaultDamage = 10, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SMG, Type = EWeaponType.MachineGun, DefaultDamage = 22, DefaultHeadMultiplicator = 1 },
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
                new Weapons { Hash = EWeaponHash.AssaultRifle, Type = EWeaponType.AssaultRifle, DefaultDamage = 30, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.SpecialCarbine, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.Revolver, Type = EWeaponType.Handgun, DefaultDamage = 110, DefaultHeadMultiplicator = 1 },
                new Weapons { Hash = EWeaponHash.MarksmanRifle, Type = EWeaponType.SniperRifle, DefaultDamage = 65, DefaultHeadMultiplicator = 2 },
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
                new Weapons { Hash = EWeaponHash.CarbineRifleMK2, Type = EWeaponType.AssaultRifle, DefaultDamage = 32, DefaultHeadMultiplicator = 1 }
            );

            modelBuilder.Entity<LobbyWeapons>().HasData(
                new LobbyWeapons { Hash = EWeaponHash.AssaultRifle, Lobby = 1, Ammo = 2000 },
                new LobbyWeapons { Hash = EWeaponHash.Revolver, Lobby = 1, Ammo = 500 },
                new LobbyWeapons { Hash = EWeaponHash.UpnAtomizer, Lobby = 1, Ammo = 500 },
                new LobbyWeapons { Hash = EWeaponHash.SMG, Lobby = 1, Ammo = 2000 },
                new LobbyWeapons { Hash = EWeaponHash.MicroSMG, Lobby = 1, Ammo = 2000 },
                new LobbyWeapons { Hash = EWeaponHash.UnholyHellbringer, Lobby = 1, Ammo = 2000 },
                new LobbyWeapons { Hash = EWeaponHash.AssaultShotgun, Lobby = 1, Ammo = 2000 },
                new LobbyWeapons { Hash = EWeaponHash.CarbineRifleMK2, Lobby = 1, Ammo = 2000 }
            );

            modelBuilder.Entity<Maps>().HasData(
                new Maps { Id = -3, Name = "All Bombs", CreatorId = 0 },
                new Maps { Id = -2, Name = "All Normals", CreatorId = 0 },
                new Maps { Id = -1, Name = "All", CreatorId = 0 }
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
        }
    }
}
