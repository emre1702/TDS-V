namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolAPI
    {
        IPoolObjectsAPI Objects { get; }
        IPoolPedsAPI Peds { get; }
        IPoolPlayersAPI Players { get; }
        IPoolVehiclesAPI Vehicles { get; }
    }
}
