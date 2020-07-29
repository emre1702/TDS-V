using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Ped;

namespace TDS_Client.Data.Interfaces.ModAPI.Cutscene
{
    public interface ICutsceneAPI
    {
        void RequestCutscene(CutsceneType cutscene);
        void RegisterEntityForCutscene(IPedBase entity, string cutsceneEntName, int p2, uint modelHash, int p4);
        void StartCutscene(int p0);
    }
}
