using TDS_Client.Data.Interfaces.RAGE.Game.Native;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Native
{
    internal class NativeAPI : INativeAPI
    {
        #region Public Methods

        public void Invoke(NativeHash native, params object[] args)
        {
            RAGE.Game.Invoker.Invoke((RAGE.Game.Natives)native, args);
        }

        public T Invoke<T>(NativeHash native, params object[] args)
        {
            return RAGE.Game.Invoker.Invoke<T>((RAGE.Game.Natives)native, args);
        }

        #endregion Public Methods
    }
}