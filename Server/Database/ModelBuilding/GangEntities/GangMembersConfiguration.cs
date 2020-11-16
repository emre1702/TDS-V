using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangMembersConfiguration : IEntityTypeConfiguration<GangMembers>
    {
        public void Configure(EntityTypeBuilder<GangMembers> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.JoinTime)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Ignore(e => e.RankNumber);
            builder.Ignore(e => e.Name);
            builder.Ignore(e => e.LastLogin);

            builder.HasOne(e => e.Gang)
                .WithMany(g => g.Members)
                .HasForeignKey(e => e.GangId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Player)
                .WithOne(g => g.GangMemberNavigation)
                .HasForeignKey<GangMembers>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Rank)
                .WithMany(r => r.GangMembers)
                .HasForeignKey(e => e.RankId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
