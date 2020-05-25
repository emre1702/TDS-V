using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.ColShape
{
    internal class ColShapeAPI : IColShapeAPI
    {
        #region Public Methods

        public IColShape CreateSphere(Position3D position, double range, ILobby lobby)
        {
            var modColshape = NAPI.ColShape.CreateSphereColShape(position.ToMod(), (float)range, lobby.Dimension);
            return new ColShape(modColshape);
        }

        #endregion Public Methods
    }
}
