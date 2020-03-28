using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Data.Enums;

namespace TDS_Server.Data.Interfaces.ModAPI.Native
{
    public interface INativeAPI
    {
        public void Send(ITDSPlayer player, ulong hash, params object[] args);

        public void Send(ITDSPlayer player, NativeHash hash, params object[] args);

        public void Send(ILobby player, NativeHash hash, params object[] args);
    }
}
