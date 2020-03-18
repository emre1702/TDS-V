using GTANetworkAPI;

namespace TDS_Server.RAGE.Extensions
{
    internal static class ColorExtension
    {
        public static Color ToColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
