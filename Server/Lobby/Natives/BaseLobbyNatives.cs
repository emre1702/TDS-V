using System;
using GTANetworkAPI;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.Natives
{
    public class BaseLobbyNatives
    {
        private readonly Func<uint> _dimensionProvider;

        public BaseLobbyNatives(Func<uint> dimensionProvider)
            => _dimensionProvider = dimensionProvider;

        public void Send(NativeHash nativeHash, params object[] args)
        {
            NAPI.Native.SendNativeToPlayersInDimension(_dimensionProvider(), (Hash)nativeHash, args);
        }
    }
}
