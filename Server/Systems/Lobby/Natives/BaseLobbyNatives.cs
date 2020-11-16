using GTANetworkAPI;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Natives;
using TDS.Shared.Data.Enums;

namespace TDS.Server.LobbySystem.Natives
{
    public class BaseLobbyNatives : IBaseLobbyNatives
    {
        private readonly IBaseLobby _lobby;

        public BaseLobbyNatives(IBaseLobby lobby)
            => _lobby = lobby;

        public void Send(NativeHash nativeHash, params object[] args)
        {
            NAPI.Native.SendNativeToPlayersInDimension(_lobby.MapHandler.Dimension, (Hash)nativeHash, args);
        }
    }
}
