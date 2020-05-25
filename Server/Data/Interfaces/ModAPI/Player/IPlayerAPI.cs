namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
#nullable enable

    public interface IPlayerAPI
    {
        #region Public Methods

        void SetHealth(IPlayer player, int health);

        #endregion Public Methods
    }
}
