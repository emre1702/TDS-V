﻿using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class PlayerFactory
    {
        public PlayerFactory()
        {
            RAGE.Elements.Entities.Players.CreateEntity = (ushort id, ushort remoteId) => new TDSPlayer(id, remoteId);
            RAGE.Elements.Player.RecreateLocal();
        }
    }
}