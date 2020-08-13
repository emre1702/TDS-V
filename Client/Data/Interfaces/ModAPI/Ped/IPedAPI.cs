using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Ped
{
    public interface IPedAPI
    {
        #region Public Methods

        IPed Create(PedHash model, Position position, Position rotation, uint dimension);

        IPed Create(PedHash model, Position position, float heading, uint dimension);

        int GetPedArmor(int handle);

        Position GetPedBoneCoords(int ped, int boneId, float offsetX = 0, float offsetY = 0, float offsetZ = 0);

        int GetPedBoneIndex(int ped, int boneId);

        PedBone? GetPedLastDamageBone(int ped);

        #endregion Public Methods
    }
}
