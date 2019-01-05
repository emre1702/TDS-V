﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TDS_Server.Entity
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

        public virtual DbSet<Adminlevelnames> Adminlevelnames { get; set; }
        public virtual DbSet<Adminlevels> Adminlevels { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<CommandsAlias> CommandsAlias { get; set; }
        public virtual DbSet<CommandsInfo> CommandsInfo { get; set; }
        public virtual DbSet<KillingspreeRewards> KillingspreeRewards { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Lobbies> Lobbies { get; set; }
        public virtual DbSet<LobbyMaps> LobbyMaps { get; set; }
        public virtual DbSet<LobbyWeapons> LobbyWeapons { get; set; }
        public virtual DbSet<LogsAdmin> LogsAdmin { get; set; }
        public virtual DbSet<LogsChat> LogsChat { get; set; }
        public virtual DbSet<LogsError> LogsError { get; set; }
        public virtual DbSet<LogsRest> LogsRest { get; set; }
        public virtual DbSet<LogsTypes> LogsTypes { get; set; }
        public virtual DbSet<Maps> Maps { get; set; }
        public virtual DbSet<Offlinemessages> Offlinemessages { get; set; }
        public virtual DbSet<Playerbans> Playerbans { get; set; }
        public virtual DbSet<Playerlobbystats> Playerlobbystats { get; set; }
        public virtual DbSet<Playermapratings> Playermapratings { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Playersettings> Playersettings { get; set; }
        public virtual DbSet<Playerstats> Playerstats { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<WeaponsHash> WeaponsHash { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Host=localhost;Port=3306;Database=TDSNew;Username=root;Password=ajagrebo;TreatTinyAsBoolean=false");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adminlevelnames>(entity =>
            {
                entity.HasKey(e => new { e.Level, e.Language })
                    .HasName("PRIMARY");

                entity.ToTable("adminlevelnames");

                entity.HasIndex(e => e.Language)
                    .HasName("FK_adminlevels_languages");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.Adminlevelnames)
                    .HasForeignKey(d => d.Language)
                    .HasConstraintName("adminlevelnames_ibfk_1");

                entity.HasOne(d => d.LevelNavigation)
                    .WithMany(p => p.Adminlevelnames)
                    .HasForeignKey(d => d.Level)
                    .HasConstraintName("FK_adminlevelnames_adminlevels");
            });

            modelBuilder.Entity<Adminlevels>(entity =>
            {
                entity.HasKey(e => e.Level)
                    .HasName("PRIMARY");

                entity.ToTable("adminlevels");

                entity.Property(e => e.ColorB).HasColumnType("tinyint(3)");

                entity.Property(e => e.ColorG).HasColumnType("tinyint(3)");

                entity.Property(e => e.ColorR).HasColumnType("tinyint(3)");
            });

            modelBuilder.Entity<Commands>(entity =>
            {
                entity.ToTable("commands");

                entity.HasIndex(e => e.NeededAdminLevel)
                    .HasName("FK_admincommands_adminlevels");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.LobbyOwnerCanUse)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.VipCanUse)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.HasOne(d => d.NeededAdminLevelNavigation)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.NeededAdminLevel)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_admincommands_adminlevels");
            });

            modelBuilder.Entity<CommandsAlias>(entity =>
            {
                entity.HasKey(e => e.Alias)
                    .HasName("PRIMARY");

                entity.ToTable("commands_alias");

                entity.HasIndex(e => e.Command)
                    .HasName("FK_commandsalias_commands");

                entity.Property(e => e.Alias).HasColumnType("varchar(50)");

                entity.HasOne(d => d.CommandNavigation)
                    .WithMany(p => p.CommandsAlias)
                    .HasForeignKey(d => d.Command)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_commandsalias_commands");
            });

            modelBuilder.Entity<CommandsInfo>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Language })
                    .HasName("PRIMARY");

                entity.ToTable("commands_info");

                entity.HasIndex(e => e.Language)
                    .HasName("FK_commands_info_languages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.CommandsInfo)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK__commands");

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.CommandsInfo)
                    .HasForeignKey(d => d.Language)
                    .HasConstraintName("FK_commands_info_languages");
            });

            modelBuilder.Entity<KillingspreeRewards>(entity =>
            {
                entity.HasKey(e => e.KillsAmount)
                    .HasName("PRIMARY");

                entity.ToTable("killingspree_rewards");

                entity.Property(e => e.KillsAmount).HasColumnType("int(10)");

                entity.Property(e => e.HealthOrArmor).HasDefaultValueSql("'0'");

                entity.Property(e => e.OnlyArmor).HasDefaultValueSql("'0'");

                entity.Property(e => e.OnlyHealth).HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.ToTable("languages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Lobbies>(entity =>
            {
                entity.ToTable("lobbies");

                entity.HasIndex(e => e.Owner)
                    .HasName("FK_lobbies_players");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmountLifes)
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.AroundSpawnPoint).HasDefaultValueSql("'3'");

                entity.Property(e => e.BombDefuseTimeMs).HasDefaultValueSql("'8000'");

                entity.Property(e => e.BombDetonateTimeMs).HasDefaultValueSql("'45000'");

                entity.Property(e => e.BombPlantTimeMs).HasDefaultValueSql("'3000'");

                entity.Property(e => e.CountdownTime).HasDefaultValueSql("'5'");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.DefaultSpawnRotation).HasDefaultValueSql("'0'");

                entity.Property(e => e.DefaultSpawnX).HasDefaultValueSql("'0'");

                entity.Property(e => e.DefaultSpawnY).HasDefaultValueSql("'0'");

                entity.Property(e => e.DefaultSpawnZ).HasDefaultValueSql("'900'");

                entity.Property(e => e.DieAfterOutsideMapLimitTime).HasDefaultValueSql("'10'");

                entity.Property(e => e.IsOfficial).HasColumnType("bit(1)");

                entity.Property(e => e.IsTemporary).HasColumnType("bit(1)");

                entity.Property(e => e.MixTeamsAfterRound).HasColumnType("bit(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Password).HasColumnType("varchar(100)");

                entity.Property(e => e.RoundTime).HasDefaultValueSql("'240'");

                entity.Property(e => e.SpawnAgainAfterDeathMs)
                    .HasColumnName("SpawnAgainAfterDeathMS")
                    .HasDefaultValueSql("'3000'");

                entity.Property(e => e.StartArmor)
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.StartHealth)
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("'100'");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Lobbies)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_lobbies_players");
            });

            modelBuilder.Entity<LobbyMaps>(entity =>
            {
                entity.HasKey(e => new { e.LobbyId, e.MapId })
                    .HasName("PRIMARY");

                entity.ToTable("lobby_maps");

                entity.HasIndex(e => e.MapId)
                    .HasName("FK_lobbies_maps_maps");

                entity.Property(e => e.LobbyId).HasColumnName("LobbyID");

                entity.Property(e => e.MapId)
                    .HasColumnName("MapID")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.Lobby)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.LobbyId)
                    .HasConstraintName("FK_lobbies_maps_lobbies");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.LobbyMaps)
                    .HasForeignKey(d => d.MapId)
                    .HasConstraintName("FK_lobbies_maps_maps");
            });

            modelBuilder.Entity<LobbyWeapons>(entity =>
            {
                entity.HasKey(e => new { e.Lobby, e.Hash })
                    .HasName("PRIMARY");

                entity.ToTable("lobby_weapons");

                entity.HasIndex(e => e.Hash)
                    .HasName("FK_lobby_weapons_weapons_hash");

                entity.Property(e => e.Damage).HasColumnType("smallint(5)");

                entity.Property(e => e.HeadMultiplicator).HasDefaultValueSql("'1'");

                entity.HasOne(d => d.HashNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Hash)
                    .HasConstraintName("FK_lobby_weapons_weapons_hash");

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.LobbyWeapons)
                    .HasForeignKey(d => d.Lobby)
                    .HasConstraintName("FK_lobby_weapons_lobbies");
            });

            modelBuilder.Entity<LogsAdmin>(entity =>
            {
                entity.ToTable("logs_admin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AsDonator).HasColumnType("bit(1)");

                entity.Property(e => e.AsVip)
                    .HasColumnName("AsVIP")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Reason).HasColumnType("varchar(500)");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<LogsChat>(entity =>
            {
                entity.ToTable("logs_chat");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsAdminChat).HasColumnType("bit(1)");

                entity.Property(e => e.IsTeamChat).HasColumnType("bit(1)");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("varchar(300)");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<LogsError>(entity =>
            {
                entity.ToTable("logs_error");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.StackTrace).HasColumnType("text");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<LogsRest>(entity =>
            {
                entity.ToTable("logs_rest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Serial).HasColumnType("varchar(200)");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<LogsTypes>(entity =>
            {
                entity.ToTable("logs_types");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Maps>(entity =>
            {
                entity.ToTable("maps");

                entity.HasIndex(e => e.CreatorId)
                    .HasName("FK_maps_players");

                entity.HasIndex(e => e.Name)
                    .HasName("Name");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.CreatorId).HasColumnName("CreatorID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Maps)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_maps_players");
            });

            modelBuilder.Entity<Offlinemessages>(entity =>
            {
                entity.ToTable("offlinemessages");

                entity.HasIndex(e => e.SourceId)
                    .HasName("FK__offlinemessages_source");

                entity.HasIndex(e => e.TargetId)
                    .HasName("FK__offlinemessages_target");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlreadyLoadedOnce).HasColumnType("bit(1)");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.TargetId).HasColumnName("TargetID");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.OfflinemessagesSource)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK__offlinemessages_source");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.OfflinemessagesTarget)
                    .HasForeignKey(d => d.TargetId)
                    .HasConstraintName("FK__offlinemessages_target");
            });

            modelBuilder.Entity<Playerbans>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ForLobby })
                    .HasName("PRIMARY");

                entity.ToTable("playerbans");

                entity.HasIndex(e => e.Admin)
                    .HasName("FK_playerbans_players");

                entity.HasIndex(e => e.ForLobby)
                    .HasName("FK_playerbans_lobbies");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ForLobby).HasDefaultValueSql("'0'");

                entity.Property(e => e.EndTimestamp).HasColumnType("timestamp");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("IP")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Scname)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Serial)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.StartTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.HasOne(d => d.AdminNavigation)
                    .WithMany(p => p.PlayerbansAdminNavigation)
                    .HasForeignKey(d => d.Admin)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_playerbans_players");

                entity.HasOne(d => d.ForLobbyNavigation)
                    .WithMany(p => p.Playerbans)
                    .HasForeignKey(d => d.ForLobby)
                    .HasConstraintName("FK_playerbans_lobbies");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.PlayerbansIdNavigation)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__players");
            });

            modelBuilder.Entity<Playerlobbystats>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Lobby })
                    .HasName("PRIMARY");

                entity.ToTable("playerlobbystats");

                entity.HasIndex(e => e.Lobby)
                    .HasName("FK_playerlobbystats_lobbies");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Assists).HasDefaultValueSql("'0'");

                entity.Property(e => e.Damage).HasDefaultValueSql("'0'");

                entity.Property(e => e.Deaths).HasDefaultValueSql("'0'");

                entity.Property(e => e.Kills).HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalAssists).HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalDamage).HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalDeaths).HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalKills).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Playerlobbystats)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_playerlobbystats_player");

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Playerlobbystats)
                    .HasForeignKey(d => d.Lobby)
                    .HasConstraintName("FK_playerlobbystats_lobbies");
            });

            modelBuilder.Entity<Playermapratings>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MapName })
                    .HasName("PRIMARY");

                entity.ToTable("playermapratings");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MapName).HasColumnType("varchar(100)");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Playermapratings)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_playermapratings_players");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.ToTable("players");

                entity.HasIndex(e => e.AdminLvl)
                    .HasName("FK_players_adminlevels");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdminLvl).HasDefaultValueSql("'0'");

                entity.Property(e => e.Donation)
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Email).HasColumnType("varchar(300)");

                entity.Property(e => e.IsVip)
                    .HasColumnName("IsVIP")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(300)");

                entity.Property(e => e.RegisterTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Scname)
                    .IsRequired()
                    .HasColumnName("SCName")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.AdminLvlNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.AdminLvl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_players_adminlevels");
            });

            modelBuilder.Entity<Playersettings>(entity =>
            {
                entity.ToTable("playersettings");

                entity.HasIndex(e => e.Language)
                    .HasName("FK_playersettings_languages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AllowDataTransfer).HasColumnType("bit(1)");

                entity.Property(e => e.HitsoundOn).HasColumnType("bit(1)");

                entity.Property(e => e.Language).HasDefaultValueSql("'9'");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Playersettings)
                    .HasForeignKey<Playersettings>(d => d.Id)
                    .HasConstraintName("FK_playersettings_player");

                entity.HasOne(d => d.LanguageNavigation)
                    .WithMany(p => p.Playersettings)
                    .HasForeignKey(d => d.Language)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_playersettings_languages");
            });

            modelBuilder.Entity<Playerstats>(entity =>
            {
                entity.ToTable("playerstats");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastLoginTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.LoggedIn).HasColumnType("bit(1)");

                entity.Property(e => e.Money).HasDefaultValueSql("'0'");

                entity.Property(e => e.PlayTime).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Playerstats)
                    .HasForeignKey<Playerstats>(d => d.Id)
                    .HasConstraintName("FK__playerstats_player");
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.ToTable("settings");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DistanceToSpotToDefuse).HasColumnType("int(5)");

                entity.Property(e => e.DistanceToSpotToPlant).HasColumnType("int(5)");

                entity.Property(e => e.ErrorToPlayerOnNonExistentCommand).HasColumnType("bit(1)");

                entity.Property(e => e.GamemodeName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MapsPath)
                    .IsRequired()
                    .HasColumnType("varchar(300)");

                entity.Property(e => e.NewMapsPath)
                    .IsRequired()
                    .HasColumnType("varchar(300)");

                entity.Property(e => e.SaveLogsCooldownMinutes)
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.SavePlayerDataCooldownMinutes)
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'20'");

                entity.Property(e => e.SaveSeasonsCooldownMinutes)
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'30'");

                entity.Property(e => e.ToChatOnNonExistentCommand).HasColumnType("bit(1)");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.ToTable("teams");

                entity.HasIndex(e => e.Lobby)
                    .HasName("FK_Teams_lobbies");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SkinHash).HasColumnType("int(10)");

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Lobby)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Teams_lobbies");
            });

            modelBuilder.Entity<WeaponsHash>(entity =>
            {
                entity.HasKey(e => e.Hash)
                    .HasName("PRIMARY");

                entity.ToTable("weapons_hash");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
        }
    }
}