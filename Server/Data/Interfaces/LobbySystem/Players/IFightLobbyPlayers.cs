namespace TDS.Server.Data.Interfaces.LobbySystem.Players
{
    public interface IFightLobbyPlayers : IBaseLobbyPlayers
    {
        bool SavePlayerLobbyStats { get; }
    }
}
