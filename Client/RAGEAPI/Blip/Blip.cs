using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Blip
{
    internal class Blip : Entity.Entity, IBlip
    {
        #region Private Fields

        private readonly RAGE.Elements.Blip _instance;

        #endregion Private Fields

        #region Public Constructors

        public Blip(RAGE.Elements.Blip instance) : base(instance)
            => _instance = instance;

        #endregion Public Constructors

        #region Public Properties

        public Position3D Rotation
        {
            set => _instance.SetRotation((int)value.Z);
        }

        #endregion Public Properties
    }
}
