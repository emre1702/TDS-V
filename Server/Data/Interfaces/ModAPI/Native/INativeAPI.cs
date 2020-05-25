using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.ModAPI.Native
{
    public interface INativeAPI
    {
        #region Public Methods

        public void Send(ITDSPlayer player, ulong hash, params object[] args);

        public void Send(ITDSPlayer player, NativeHash hash, params object[] args);

        public void Send(ILobby player, NativeHash hash, params object[] args);

        #endregion Public Methods
    }
}
