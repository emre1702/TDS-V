namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class ComponentVariationExtension
    {
        #region Internal Methods

        internal static GTANetworkAPI.ComponentVariation ToMod(this Data.Models.ComponentVariation componentVariation)
            => new GTANetworkAPI.ComponentVariation
            {
                Drawable = componentVariation.Drawable,
                Palette = componentVariation.Palette,
                Texture = componentVariation.Texture
            };

        internal static Data.Models.ComponentVariation ToTDS(this GTANetworkAPI.ComponentVariation componentVariation)
            => new Data.Models.ComponentVariation
            {
                Drawable = componentVariation.Drawable,
                Palette = componentVariation.Palette,
                Texture = componentVariation.Texture
            };

        #endregion Internal Methods
    }
}
