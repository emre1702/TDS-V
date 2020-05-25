using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Native;
using TDS_Shared.Data.Enums;

namespace TDS_Server.RAGEAPI.Native
{
    internal class NativeAPI : INativeAPI
    {
        #region Public Methods

        public void Send(ITDSPlayer player, ulong hash, params object[] args)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;
            GTANetworkAPI.NAPI.Native.SendNativeToPlayer(modPlayer, hash, args);
        }

        public void Send(ITDSPlayer player, NativeHash hash, params object[] args)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;
            GTANetworkAPI.NAPI.Native.SendNativeToPlayer(modPlayer, (GTANetworkAPI.Hash)hash, args);
        }

        public void Send(ILobby lobby, NativeHash hash, params object[] args)
        {
            GTANetworkAPI.NAPI.Native.SendNativeToPlayersInDimension(lobby.Dimension, (GTANetworkAPI.Hash)hash, args);
        }

        #endregion Public Methods
    }
}
