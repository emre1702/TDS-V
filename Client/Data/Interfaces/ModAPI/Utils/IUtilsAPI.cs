using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Utils
{
    public interface IUtilsAPI
    {
        uint GetHashKey(string str);
        void Settimera(int v);
        int Timera();
    }
}
