using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class ColorExtension
    {
        public static Color ToMod(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static System.Drawing.Color ToTDS(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.ToInt32());
        }
    }
}
