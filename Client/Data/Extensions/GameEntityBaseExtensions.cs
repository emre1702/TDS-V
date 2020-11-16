using RAGE;
using RAGE.Elements;

namespace TDS.Client.Data.Extensions
{
    public static class GameEntityBaseExtensions
    {
        public static Vector3 GetOffsetInWorldCoords(this GameEntityBase entity, float offsetX, float offsetY, float offsetZ)
            => RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(entity.Handle, offsetX, offsetY, offsetZ);
    }
}
