namespace TDS_Server.Data.Interfaces.ModAPI.ColShape
{
#nullable enable

    public interface IColShape : IEntity
    {
        #region Public Delegates

        public delegate void ColshapeEnterExitDelegate(ITDSPlayer player);

        #endregion Public Delegates

        #region Public Events

        public event ColshapeEnterExitDelegate? PlayerEntered;

        public event ColshapeEnterExitDelegate? PlayerExited;

        #endregion Public Events
    }
}
