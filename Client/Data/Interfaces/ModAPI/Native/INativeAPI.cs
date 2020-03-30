using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Native
{
    public interface INativeAPI
    {
        void Invoke(NativeHash native, params object[] args);
    }
}
