using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerMapCreatorRewardsWhileOffline : IPlayerDataTable
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int? MapId { get; set; }
        public MapRetrieveType Type { get; set; }
        public int Reward { get; set; }

        public virtual Players Player { get; set; }
        public virtual Maps Map { get; set; }
    }

    internal class PlayerMapCreatorRewardsWhileOfflineConfiguration : IEntityTypeConfiguration<PlayerMapCreatorRewardsWhileOffline>
    {
        public void Configure(EntityTypeBuilder<PlayerMapCreatorRewardsWhileOffline> builder)
        {
            builder.HasOne(e => e.Player)
                .WithMany(e => e.MapCreatorRewardsWhileOffline)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Map)
                .WithMany(e => e.CreatorRewardsWhileOffline)
                .HasForeignKey(e => e.MapId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}