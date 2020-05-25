namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangLevelSettings
    {
        #region Public Properties

        public bool CanChangeBlipColor { get; set; }
        public byte GangAreaSlots { get; set; }
        public float HouseAreaRadius { get; set; }
        public int HousePrice { get; set; }
        public byte Level { get; set; }
        public int NeededExperience { get; set; }
        public byte PlayerSlots { get; set; }
        public byte RankSlots { get; set; }
        public int UpgradePrice { get; set; }
        public byte VehicleSlots { get; set; }

        #endregion Public Properties
    }
}
