using TDS_Client.Data.Interfaces.ModAPI.Utils;

namespace TDS_Client.RAGEAPI.Utils
{
    class UtilsAPI : IUtilsAPI
    {
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
    }
}
