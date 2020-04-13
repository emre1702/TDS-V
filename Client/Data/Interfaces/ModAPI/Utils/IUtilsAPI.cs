using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Utils
{
    public interface IUtilsAPI
    {
        void Settimera(int value);
        int Timera();
        void Wait(int ms);
    }
}
