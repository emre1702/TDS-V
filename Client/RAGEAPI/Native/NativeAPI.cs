using TDS_Client.Data.Interfaces.ModAPI.Native;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Native
{
    class NativeAPI : INativeAPI
    {
        public void Invoke(NativeHash native, params object[] args)
        {
            RAGE.Game.Invoker.Invoke((RAGE.Game.Natives)native, args);
        }
    }
}
