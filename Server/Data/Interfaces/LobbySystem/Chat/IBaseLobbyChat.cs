using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Interfaces.LobbySystem.Chat
{
    public interface IBaseLobbyChat
    {
        void Send(Dictionary<ILanguage, string> texts);

        void Send(Func<ILanguage, string> langGetter);

        void Send(string msg);

        void Send(string msg, HashSet<int> blockingPlayerIds);
    }
}
