using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS_Server.Database.Interfaces;

namespace TDS_Server.Database.Entity.Player.Settings
{
    public class PlayerChatSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public float ChatWidth { get; set; }

        [JsonProperty("1")]
        public float ChatMaxHeight { get; set; }

        [JsonProperty("2")]
        public float ChatFontSize { get; set; }

        [JsonProperty("3")]
        public bool HideDirtyChat { get; set; }

        [JsonProperty("4")]
        public bool ShowCursorOnChatOpen { get; set; }

        [JsonProperty("5")]
        public bool HideChatInfo { get; set; }

        [JsonProperty("6")]
        public float ChatInfoFontSize { get; set; }

        [JsonProperty("7")]
        public int ChatInfoMoveTimeMs { get; set; }
    }

    internal class PlayerChatSettingsConfiguration : IEntityTypeConfiguration<PlayerChatSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerChatSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);
            builder.Property(e => e.ChatWidth).HasColumnName("ChatWidth").HasDefaultValue(30);
            builder.Property(e => e.ChatMaxHeight).HasColumnName("ChatMaxHeight").HasDefaultValue(35);
            builder.Property(e => e.ChatFontSize).HasColumnName("ChatFontSize").HasDefaultValue(1.4);
            builder.Property(e => e.HideDirtyChat).HasColumnName("HideDirtyChat");
            builder.Property(e => e.ShowCursorOnChatOpen).HasColumnName("ShowCursorOnChatOpen");
            builder.Property(e => e.HideChatInfo).HasColumnName("HideChatInfo");
            builder.Property(e => e.ChatInfoFontSize).HasColumnName("ChatInfoFontSize").HasDefaultValue(1f);
            builder.Property(e => e.ChatInfoMoveTimeMs).HasColumnName("ChatInfoMoveTimeMs").HasDefaultValue(15000);
        }
    }
}
