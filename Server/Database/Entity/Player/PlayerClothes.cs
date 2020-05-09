namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerClothes
    {
        public int PlayerId { get; set; }

        public virtual Players Player { get; set; }
    }
}
