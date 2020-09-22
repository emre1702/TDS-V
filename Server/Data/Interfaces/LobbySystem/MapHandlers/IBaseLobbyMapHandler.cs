using GTANetworkAPI;

namespace TDS_Server.Data.Interfaces.LobbySystem.MapHandlers
{
    public interface IBaseLobbyMapHandler
    {
        uint Dimension { get; }
        Vector3 SpawnPoint { get; }
        float SpawnRotation { get; }
    }
}
