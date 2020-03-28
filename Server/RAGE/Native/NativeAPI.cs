﻿using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Native;

namespace TDS_Server.RAGE.Native
{
    class NativeAPI : INativeAPI
    {
        public void Send(ITDSPlayer player, ulong hash, params object[] args)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;
            GTANetworkAPI.NAPI.Native.SendNativeToPlayer(modPlayer._instance, hash, args);
        }

        public void Send(ITDSPlayer player, NativeHash hash, params object[] args)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;
            GTANetworkAPI.NAPI.Native.SendNativeToPlayer(modPlayer._instance, (GTANetworkAPI.Hash)hash, args);
        }

        public void Send(ILobby lobby, NativeHash hash, params object[] args)
        {
            GTANetworkAPI.NAPI.Native.SendNativeToPlayersInDimension(lobby.Dimension, (GTANetworkAPI.Hash)hash, args);
        }
    }
}
