using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player.Settings
{
    public class PlayerInfoSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public bool ShowCursorInfo { get; set; }

        [JsonProperty("1")]
        public bool ShowLobbyLeaveInfo { get; set; }
    }

    internal class PlayerInfoSettingsConfiguration : IEntityTypeConfiguration<PlayerInfoSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerInfoSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.ShowCursorInfo).HasColumnName("ShowCursorInfo");
            builder.Property(e => e.ShowLobbyLeaveInfo).HasColumnName("ShowLobbyLeaveInfo");
        }
    }
}
