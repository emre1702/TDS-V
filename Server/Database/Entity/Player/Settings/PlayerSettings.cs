using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS_Server.Database.Interfaces;
using TDS_Shared.Data.Models;

namespace TDS_Server.Database.Entity.Player.Settings
{
    public class PlayerSettings : IPlayerDataTable
    {

        [JsonIgnore]
        public virtual Players Player { get; set; }

        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonIgnore]
        public virtual PlayerChatSettings Chat { get; set; }
        [JsonIgnore]
        public virtual PlayerCooldownsAndDurationsSettings CooldownsAndDurations { get; set; }
        [JsonIgnore]
        public virtual PlayerFightEffectSettings FightEffect { get; set; }
        [JsonIgnore]
        public virtual PlayerGeneralSettings General { get; set; }
        [JsonIgnore]
        public virtual PlayerInfoSettings Info { get; set; }
        [JsonIgnore]
        public virtual PlayerIngameColorsSettings IngameColors { get; set; }
        [JsonIgnore]
        public virtual PlayerScoreboardSettings Scoreboard { get; set; }
        [JsonIgnore]
        public virtual PlayerVoiceSettings Voice { get; set; }
    }

    internal class PlayerSettingsConfiguration : IEntityTypeConfiguration<PlayerSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.HasOne(e => e.Chat)
                .WithOne()
                .HasForeignKey<PlayerChatSettings>(e => e.PlayerId);
            builder.HasOne(e => e.CooldownsAndDurations)
                .WithOne()
                .HasForeignKey<PlayerCooldownsAndDurationsSettings>(e => e.PlayerId);
            builder.HasOne(e => e.FightEffect)
                .WithOne()
                .HasForeignKey<PlayerFightEffectSettings>(e => e.PlayerId);
            builder.HasOne(e => e.General)
                .WithOne()
                .HasForeignKey<PlayerGeneralSettings>(e => e.PlayerId);
            builder.HasOne(e => e.Info)
                .WithOne()
                .HasForeignKey<PlayerInfoSettings>(e => e.PlayerId);
            builder.HasOne(e => e.IngameColors)
                .WithOne()
                .HasForeignKey<PlayerIngameColorsSettings>(e => e.PlayerId);
            builder.HasOne(e => e.Scoreboard)
                .WithOne()
                .HasForeignKey<PlayerScoreboardSettings>(e => e.PlayerId);
            builder.HasOne(e => e.Voice)
                .WithOne()
                .HasForeignKey<PlayerVoiceSettings>(e => e.PlayerId);

            builder.HasOne(d => d.Player)
                .WithOne(p => p.PlayerSettings)
                .HasForeignKey<PlayerSettings>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
