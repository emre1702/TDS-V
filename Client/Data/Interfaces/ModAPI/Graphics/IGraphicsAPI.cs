using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Graphics
{
    public interface IGraphicsAPI
    {
        void GetScreenResolution(ref int resX, ref int resY);
        // Graphics.GetActiveScreenResolution(ref ResX, ref ResY);
        float GetTextScaleHeight(float scale, Font font);
        void DrawLine(float startX, float startY, float startZ, float endX, float endY, float endZ, byte r, byte g, byte b, byte a);
        bool GetScreenCoordFromWorldCoord(float x, float y, float z, ref float point2DX, ref float point2DY);
        // Ui.GetTextScaleHeight(scale, (int)font)

    }
}
