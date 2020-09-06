using TDS_Client.Data.Interfaces.RAGE.Game.Blip;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Blip
{
    internal class BlipAPI : IBlipAPI
    {
        #region Public Constructors

        public BlipAPI()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IBlip Create(uint sprite, Position3D position, string name = "", float scale = 1, int color = 0, int alpha = 255, float drawDistance = 0, bool shortRange = false,
            int rotation = 0, float radius = 0, uint dimension = 0)
        {
            return new Blip(sprite, position.ToVector3(), name, scale, color, alpha, drawDistance, shortRange, rotation, radius, dimension);
        }

        #endregion Public Methods
    }
}