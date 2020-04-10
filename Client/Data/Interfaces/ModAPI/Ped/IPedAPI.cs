using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Ped
{
    public interface IPedAPI
    {
        int GetPedBoneIndex(int targetValue, int bone);
        int GetPedArmor(int handle);
        IPed Create(uint v, Position3D position, Position3D rotation, int dimension);
    }
}
