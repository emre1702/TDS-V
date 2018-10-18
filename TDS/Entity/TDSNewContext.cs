using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TDS.Entities
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

        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Lobbies> Lobbies { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Playerlobbystats> Playerlobbystats { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Playersettings> Playersettings { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(System.Configuration.ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTimestamp).HasColumnType("timestamp");

                entity.Property(e => e.IsTemporary)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'1\\''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Password).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("logs");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Playerlobbystats>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Lobby });

                entity.ToTable("playerlobbystats");

                entity.HasIndex(e => e.Lobby)
                    .HasName("FK_playerlobbystats_lobbies");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lobby).HasDefaultValueSql("'0'");

                entity.Property(e => e.Assists).HasDefaultValueSql("'0'");

                entity.Property(e => e.Damage).HasDefaultValueSql("'0'");

                entity.Property(e => e.Deaths).HasDefaultValueSql("'0'");

                entity.Property(e => e.Kills).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Playerlobbystats)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_playerlobbystats_player");

                entity.HasOne(d => d.LobbyNavigation)
                    .WithMany(p => p.Playerlobbystats)
                    .HasForeignKey(d => d.Lobby)
                    .HasConstraintName("FK_playerlobbystats_lobbies");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.ToTable("players");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AllowDataTransfer)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Email).HasColumnType("varchar(100)");

                entity.Property(e => e.LastLoginTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.RegisterTimestamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Scname)
                    .IsRequired()
                    .HasColumnName("SCName")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Playersettings>(entity =>
            {
                entity.ToTable("playersettings");

                entity.HasIndex(e => e.Language)
                    .HasName("FK_playersettings_languages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HitsoundOn)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

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

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.ToTable("settings");

                entity.Property(e => e.Id).HasColumnName("ID");
            });
        }
    }
}
