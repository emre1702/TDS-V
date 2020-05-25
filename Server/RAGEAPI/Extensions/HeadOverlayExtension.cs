namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class HeadOverlayExtension
    {
        #region Public Methods

        public static GTANetworkAPI.HeadOverlay ToMod(this Data.Models.HeadOverlay headBlend)
        {
            return new GTANetworkAPI.HeadOverlay
            {
                Index = headBlend.Index,
                Opacity = headBlend.Opacity,
                Color = headBlend.Color,
                SecondaryColor = headBlend.SecondaryColor
            };
        }

        public static Data.Models.HeadOverlay ToTDS(this GTANetworkAPI.HeadOverlay headBlend)
        {
            return new Data.Models.HeadOverlay
            {
                Index = headBlend.Index,
                Opacity = headBlend.Opacity,
                Color = headBlend.Color,
                SecondaryColor = headBlend.SecondaryColor
            };
        }

        #endregion Public Methods
    }
}
