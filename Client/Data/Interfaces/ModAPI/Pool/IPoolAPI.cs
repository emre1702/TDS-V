namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolAPI
    {
        #region Public Properties

        IPoolObjectsAPI Objects { get; }
        IPoolPedsAPI Peds { get; }
        IPoolPlayersAPI Players { get; }
        IPoolVehiclesAPI Vehicles { get; }

        #endregion Public Properties
    }
}
