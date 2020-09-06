using TDS_Client.Data.Interfaces.RAGE.Game.Utils;

namespace TDS_Client.RAGEAPI.Utils
{
    internal class UtilsAPI : IUtilsAPI
    {
        #region Public Methods

        public void Settimera(int value)
        {
            RAGE.Game.Utils.Settimera(value);
        }

        public int Timera()
        {
            return RAGE.Game.Utils.Timera();
        }

        public void Wait(int ms)
        {
            RAGE.Game.Utils.Wait(ms);
        }

        #endregion Public Methods
    }
}