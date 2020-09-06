using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.RAGE.Game.Cutscene;
using TDS_Client.Data.Interfaces.RAGE.Game.Entity;
using TDS_Client.Data.Interfaces.RAGE.Game.Ped;

namespace TDS_Client.RAGEAPI.Cutscene
{
    public class CutsceneAPI : ICutsceneAPI
    {
        public void RegisterEntityForCutscene(IPedBase entity, string cutsceneEntName, int p2, uint modelHash, int p4)
            => RAGE.Game.Cutscene.RegisterEntityForCutscene(entity.Handle, cutsceneEntName, p2, modelHash, p4);

        public void RequestCutscene(CutsceneType cutscene)
            => RAGE.Game.Cutscene.RequestCutscene(cutscene.ToString(), 8);

        public void StartCutscene(int p0)
            => RAGE.Game.Cutscene.StartCutscene(p0);
    }
}