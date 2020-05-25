using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Native
{
    public interface INativeAPI
    {
        #region Public Methods

        void Invoke(NativeHash native, params object[] args);

        T Invoke<T>(NativeHash native, params object[] args);

        #endregion Public Methods
    }
}
