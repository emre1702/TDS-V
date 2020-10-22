﻿using System;

namespace TDS_Server.Data.Interfaces.LobbySystem.Notifications
{
#nullable enable

    public interface IBaseLobbyNotifications
    {
        void Send(Func<ILanguage, string> langGetter, bool flashing = false);
    }
}