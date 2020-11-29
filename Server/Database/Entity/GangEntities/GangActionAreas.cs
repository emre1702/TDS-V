using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.Entity.GangEntities
{
    public class GangActionAreas
    {
        public int MapId { get; set; }
        public int OwnerGangId { get; set; }
        public int AttackCount { get; set; }
        // Amount defend since last capture
        public int DefendCountSinceLastCapture { get; set; }
        public DateTime LastAttacked { get; set; }
        public DateTime? CooldownStartTime { get; set; }


        public virtual Maps Map { get; set; }
        public virtual Gangs OwnerGang { get; set; }
    }

    public class GangActionAreasConfiguration : IEntityTypeConfiguration<GangActionAreas>
    {
        public void Configure(EntityTypeBuilder<GangActionAreas> builder)
        {
            builder.HasKey(e => e.MapId);

            builder.Property(e => e.LastAttacked)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("'2019-1-1'::timestamp");

            builder.Property(e => e.CooldownStartTime)
               .HasConversion(e => e, e => e == null ? (DateTime?)null : DateTime.SpecifyKind(e.Value, DateTimeKind.Utc));

            builder.Property(e => e.AttackCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.DefendCountSinceLastCapture)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(g => g.Map)
                .WithOne(m => m.GangwarArea)
                .HasForeignKey<GangActionAreas>(g => g.MapId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.OwnerGang)
                .WithMany(m => m.GangwarAreas)
                .HasForeignKey(g => g.OwnerGangId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
