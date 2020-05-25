using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Ped
{
    internal class Ped : PedBase, IPed
    {
        #region Private Fields

        private readonly RAGE.Elements.Ped _instance;

        #endregion Private Fields

        #region Public Constructors

        public Ped(RAGE.Elements.Ped instance) : base(instance)
            => _instance = instance;

        #endregion Public Constructors

        #region Public Properties

        public override Position3D Position
        {
            get => RAGE.Game.Entity.GetEntityCoords(_instance.Handle, true).ToPosition3D();
            set => RAGE.Game.Entity.SetEntityCoordsNoOffset(_instance.Handle, value.X, value.Y, value.Z, true, true, true);
        }

        #endregion Public Properties
    }
}
