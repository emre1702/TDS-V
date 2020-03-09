namespace TDS_Server.Data.Interfaces.ModAPI.Events
{
    public interface IEvents
    {
        delegate void PlayerDelegate(IPlayer player);

        event PlayerDelegate PlayerConnected;
        event PlayerDelegate PlayerDisconnected;
    }
}
