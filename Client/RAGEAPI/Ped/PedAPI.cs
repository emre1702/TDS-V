using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Ped
{
    class PedAPI : IPedAPI
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PedAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        public IPed Create(uint model, Position3D position, Position3D rotation, uint dimension)
        {
            var instance = new RAGE.Elements.Ped(model, position.ToVector3(), rotation.Z, dimension);
            return _entityConvertingHandler.GetEntity(instance);
        }

        public int GetPedArmor(int handle)
        {
            throw new System.NotImplementedException();
        }

        public int GetPedBoneIndex(int targetValue, int bone)
        {
            throw new System.NotImplementedException();
        }
    }
}
