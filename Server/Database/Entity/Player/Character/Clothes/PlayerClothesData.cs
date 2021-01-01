using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Clothes
{
    public class PlayerClothesData : IPlayerDataTable
    {
        public int PlayerId { get; set; }

        public byte Slot { get; set; }

        public virtual IEnumerable<PlayerClothesComponentOrPropData> ComponentOrPropDatas { get; set; }

        [JsonIgnore]
        public virtual PlayerClothesDatas ClothesDatas { get; set; }
    }

    public class PlayerClothesDataConfiguration : IEntityTypeConfiguration<PlayerClothesData>
    {
        public void Configure(EntityTypeBuilder<PlayerClothesData> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.HasOne(e => e.ClothesDatas)
                .WithMany(c => c.DatasPerSlot)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}