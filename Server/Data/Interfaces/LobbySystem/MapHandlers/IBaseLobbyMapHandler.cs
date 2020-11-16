using GTANetworkAPI;

namespace TDS.Server.Data.Interfaces.LobbySystem.MapHandlers
{
    public interface IBaseLobbyMapHandler
    {
        uint Dimension { get; }
        Vector3 SpawnPoint { get; }
        float SpawnRotation { get; }
    }
}
