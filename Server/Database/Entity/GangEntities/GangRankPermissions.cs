namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangRankPermissions
    {
        public int GangId { get; set; }

        // Administration //
        public ushort ManagePermissions { get; set; }
        public ushort InviteMembers { get; set; }
        public ushort KickMembers { get; set; }
        public ushort ManageRanks { get; set; }

        // Action //
        public ushort StartGangwar { get; set; }

        public virtual Gangs Gang { get; set; }
    }
}
