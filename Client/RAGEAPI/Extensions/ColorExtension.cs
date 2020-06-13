namespace TDS_Client.RAGEAPI.Extensions
{
    internal static class ColorExtension
    {
        /*public static Color ToColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }*/

        #region Public Methods

        public static RAGE.RGBA ToRGBA(this System.Drawing.Color color)
        {
            return new RAGE.RGBA(color.R, color.G, color.B, color.A);
        }

        #endregion Public Methods
    }
}
