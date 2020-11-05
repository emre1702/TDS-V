using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Database.ModelBuilding.Userpanel
{
    public class SupportRequestMessagesConfiguration : IEntityTypeConfiguration<SupportRequestMessages>
    {
        public void Configure(EntityTypeBuilder<SupportRequestMessages> builder)
        {
            builder.HasKey(e => new { e.RequestId, e.MessageIndex });

            builder.Property(e => e.Text).HasMaxLength(300);

            builder.Property(e => e.CreateTime)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', CURRENT_DATE)");

            builder.HasOne(e => e.Author)
                .WithMany(a => a.SupportRequestMessages)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Request)
                .WithMany(r => r.Messages)
                .HasForeignKey(e => e.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
