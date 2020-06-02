﻿using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Ped
{
    internal class PedAPI : IPedAPI
    {
        #region Public Constructors

        public PedAPI()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IPed Create(PedHash model, Position3D position, Position3D rotation, uint dimension)
            => Create(model, position, rotation.Z, dimension);

        public IPed Create(PedHash model, Position3D position, float heading, uint dimension)
        {
            var instance = new Ped((uint)model, position.ToVector3(), heading, dimension);
            return instance;
        }

        public int GetPedArmor(int handle)
            => RAGE.Game.Ped.GetPedArmour(handle);

        public int GetPedBoneIndex(int ped, int boneId)
            => RAGE.Game.Ped.GetPedBoneIndex(ped, boneId);

        #endregion Public Methods
    }
}
