﻿using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerChat
    {
        void ClosePrivateChat(bool becauseOfDisconnectOfTarget);

        void Init(ITDSPlayer player);

        void SendChatMessage(string msg);

        void SendNotification(string msg, bool flashing = false);
        void SendAlert(string msg);
    }
}
