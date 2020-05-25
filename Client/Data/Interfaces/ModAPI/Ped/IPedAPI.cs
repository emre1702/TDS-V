using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Ped
{
    public interface IPedAPI
    {
        #region Public Methods

        IPed Create(PedHash model, Position3D position, Position3D rotation, uint dimension);

        IPed Create(PedHash model, Position3D position, float heading, uint dimension);

        int GetPedArmor(int handle);

        int GetPedBoneIndex(int ped, int boneId);

        #endregion Public Methods
    }
}
