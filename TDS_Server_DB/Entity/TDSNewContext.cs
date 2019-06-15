using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TDS_Server_DB.Entity
{
    public partial class TDSNewContext : DbContext
    {
        public TDSNewContext()
        {
        }

        public TDSNewContext(DbContextOptions<TDSNewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual DbSet<AdminLevels> AdminLevels { get; set; }
        public virtual DbSet<CommandAlias> CommandAlias { get; set; }
        public virtual DbSet<CommandInfos> CommandInfos { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<FreeroamDefaultVehicle> FreeroamDefaultVehicle { get; set; }
        public virtual DbSet<FreeroamVehicleType> FreeroamVehicleType { get; set; }
        public virtual DbSet<Gangs> Gangs { get; set; }
        public virtual DbSet<KillingspreeRewards> KillingspreeRewards { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Lobbies> Lobbies { get; set; }
        public virtual DbSet<LobbyMaps> LobbyMaps { get; set; }
        public virtual DbSet<LobbyRewards> LobbyRewards { get; set; }
        public virtual DbSet<LobbyRoundSettings> LobbyRoundSettings { get; set; }
        public virtual DbSet<LobbyTypes> LobbyTypes { get; set; }
        public virtual DbSet<LobbyWeapons> LobbyWeapons { get; set; }
        public virtual DbSet<LogAdmins> LogAdmins { get; set; }
        public virtual DbSet<LogChats> LogChats { get; set; }
        public virtual DbSet<LogErrors> LogErrors { get; set; }
        public virtual DbSet<LogRests> LogRests { get; set; }
        public virtual DbSet<LogTypes> LogTypes { get; set; }
        public virtual DbSet<Maps> Maps { get; set; }
        public virtual DbSet<Offlinemessages> Offlinemessages { get; set; }
        public virtual DbSet<PlayerBans> PlayerBans { get; set; }
        public virtual DbSet<PlayerLobbyStats> PlayerLobbyStats { get; set; }
        public virtual DbSet<PlayerMapFavourites> PlayerMapFavourites { get; set; }
        public virtual DbSet<PlayerMapRatings> PlayerMapRatings { get; set; }
        public virtual DbSet<PlayerSettings> PlayerSettings { get; set; }
        public virtual DbSet<PlayerStats> PlayerStats { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<ServerSettings> ServerSettings { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<WeaponTypes> WeaponTypes { get; set; }
        public virtual DbSet<Weapons> Weapons { get; set; }

        // Unable to generate entity type for table 'public.pg_stat_statements'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=localhost;Database=TDSNew;User ID=tdsv;Password=ajagrebo;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_stat_statements")
                .HasAnnotation("ProductVersion", "3.0.0-preview5.19227.1");

            modelBuilder.Entity<AdminLevelNames>(entity =>
            {
                entity.HasKey(e => new { e.Level, e.Language });

                entity.ToTable("admin_level_names");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.AdminLevelNames)
                    .HasForeignKey(d => d.Language)
                    .HasConstraintName("FK_admin_level_names_language");

                entity.HasOne(d => d.LevelNavigation)
                    .WithMany(p => p.AdminLevelNames)
                    .HasForeignKey(d => d.Level)
                    .HasConstraintName("FK_admin_level_names_admin_level");
            });

            modelBuilder.Entity<AdminLevels>(entity =>
            {
                entity.HasKey(e => e.Level)
                    .HasName("admin_levels_pkey");

                entity.ToTable("admin_levels");

                entity.Property(e => e.Level).ValueGeneratedNever();
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

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.CommandInfos)
                    .HasForeignKey(d => d.Language)
                    .HasConstraintName("command_infos_Language_fkey");
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
                entity.HasKey(e => e.VehicleTypeId)
                    .HasName("freeroam_default_vehicle_pkey");

                entity.ToTable("freeroam_default_vehicle");

                entity.Property(e => e.VehicleTypeId).ValueGeneratedOnAdd();

                entity.Property(e => e.Note).HasColumnType("character varying");

                entity.HasOne(d => d.VehicleType)
                    .WithOne(p => p.FreeroamDefaultVehicle)
                    .HasForeignKey<FreeroamDefaultVehicle>(d => d.VehicleTypeId)
                    .HasConstraintName("freeroam_default_vehicle_VehicleTypeId_fkey");
            });

            modelBuilder.Entity<FreeroamVehicleType>(entity =>
            {
                entity.ToTable("freeroam_vehicle_type");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Gangs>(entity =>
            {
                entity.ToTable("gangs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Short)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Gangs)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("gangs_TeamID_fkey");
            });

            modelBuilder.Entity<KillingspreeRewards>(entity =>
            {
                entity.HasKey(e => e.KillsAmount)
                    .HasName("killingspree_rewards_pkey");

                entity.ToTable("killingspree_rewards");

                entity.Property(e => e.KillsAmount).ValueGeneratedNever();
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.ToTable("languages");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Lobbies>(entity =>
            {
                entity.ToTable("lobbies");

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"lobbies_ID_seq\"'::regclass)");

                entity.Property(e => e.AroundSpawnPoint).HasDefaultValueSql("3");

                entity.Property(e => e.CreateTimestamp).HasDefaultValueSql("now()");

                entity.Property(e => e.DefaultSpawnZ).HasDefaultValueSql("900");

                entity.Property(e => e.DieAfterOutsideMapLimitTime).HasDefaultValueSql("10");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.SpawnAgainAfterDeathMs).HasDefaultValueSql("400");

                entity.Property(e => e.StartArmor).HasDefaultValueSql("100");

                entity.Property(e => e.StartHealth).HasDefaultValueSql("100");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Lobbies)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("lobbies_Owner_fkey");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Lobbies)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("lobbies_Type_fkey");
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

            modelBuilder.Entity<LobbyTypes>(entity =>
            {
                entity.ToTable("lobby_types");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
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

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AsVip).HasColumnName("AsVIP");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogChats>(entity =>
            {
                entity.ToTable("log_chats");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogErrors>(entity =>
            {
                entity.ToTable("log_errors");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Info).IsRequired();

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogRests>(entity =>
            {
                entity.ToTable("log_rests");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.Serial).HasMaxLength(200);

                entity.Property(e => e.Timestamp).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<LogTypes>(entity =>
            {
                entity.ToTable("log_types");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("nextval('\"ID_ID_seq\"'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Maps>(entity =>
            {
                entity.ToTable("maps");

                entity.HasIndex(e => e.Name)
                    .HasName("Index_maps_name")
                    .ForNpgsqlHasMethod("hash");

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"maps_ID_seq\"'::regclass)");

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

                entity.Property(e => e.Id).HasColumnName("ID");

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

            modelBuilder.Entity<PlayerSettings>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("player_settings_pkey");

                entity.ToTable("player_settings");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("PlayerID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.PlayerSettings)
                    .HasForeignKey(d => d.Language)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("player_settings_Language_fkey");

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

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsVip).HasColumnName("IsVIP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RegisterTimestamp)
                    .HasColumnType("timestamp(4) without time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Scname)
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
                    .HasColumnName("ID")
                    .HasDefaultValueSql("nextval('\"ServerSettings_ID_seq\"'::regclass)");

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
                    .HasMaxLength(300)
                    .HasDefaultValueSql("'bridge/resources/tds/savedmaps/'::character varying");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.ToTable("teams");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Lobby)
                    .HasConstraintName("teams_Lobby_fkey");
            });

            modelBuilder.Entity<WeaponTypes>(entity =>
            {
                entity.ToTable("weapon_types");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Weapons>(entity =>
            {
                entity.HasKey(e => e.Hash)
                    .HasName("weapons_pkey");

                entity.ToTable("weapons");

                entity.Property(e => e.Hash).ValueGeneratedNever();

                entity.Property(e => e.DefaultHeadMultiplicator).HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Weapons)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("weapons_Type_fkey");
            });

            modelBuilder.HasSequence<short>("ID_ID_seq");

            modelBuilder.HasSequence<int>("lobbies_ID_seq");

            modelBuilder.HasSequence<int>("maps_ID_seq");

            modelBuilder.HasSequence<short>("ServerSettings_ID_seq");
        }
    }
}
