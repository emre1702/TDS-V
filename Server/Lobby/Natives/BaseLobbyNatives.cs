using GTANetworkAPI;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Natives;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.Natives
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
