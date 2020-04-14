using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Ped
{
    public interface IPedAPI
    {
        IPed Create(uint model, Position3D position, Position3D rotation, uint dimension);
        int GetPedBoneIndex(int ped, int boneId);
        int GetPedArmor(int handle);        
    }
}
