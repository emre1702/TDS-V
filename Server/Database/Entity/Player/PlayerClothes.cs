namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerClothes
    {
        #region Public Properties

        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
