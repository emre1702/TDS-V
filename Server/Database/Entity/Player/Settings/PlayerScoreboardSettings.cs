using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Player.Settings
{
    public class PlayerScoreboardSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public ScoreboardPlayerSorting ScoreboardPlayerSorting { get; set; }

        [JsonProperty("1")]
        public bool ScoreboardPlayerSortingDesc { get; set; }

        [JsonProperty("2")]
        public TimeSpanUnitsOfTime ScoreboardPlaytimeUnit { get; set; }
    }

    internal class PlayerScoreboardSettingsConfiguration : IEntityTypeConfiguration<PlayerScoreboardSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerScoreboardSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.ScoreboardPlayerSorting)
                .HasColumnName("ScoreboardPlayerSorting")
                .HasDefaultValue(ScoreboardPlayerSorting.Name);
            builder.Property(e => e.ScoreboardPlayerSortingDesc)
                .HasColumnName("ScoreboardPlayerSortingDesc");
            builder.Property(e => e.ScoreboardPlaytimeUnit)
                .HasColumnName("ScoreboardPlaytimeUnit")
                .HasDefaultValue(TimeSpanUnitsOfTime.HourMinute);
        }
    }
}
