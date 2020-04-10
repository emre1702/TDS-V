using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Graphics
{
    public interface IGraphicsAPI
    {
        void GetScreenResolution(ref int resX, ref int resY);
        bool HasScaleformMovieLoaded(int handle);

        // Graphics.GetActiveScreenResolution(ref ResX, ref ResY);
        float GetTextScaleHeight(float scale, Font font);
        void DrawLine(float startX, float startY, float startZ, float endX, float endY, float endZ, byte r, byte g, byte b, byte a);
        bool GetScreenCoordFromWorldCoord(float x, float y, float z, ref float point2DX, ref float point2DY);
        void StopScreenEffect(object dEATHFAILMPIN);
        void DrawPoly(float x1, float y1, float maxTop, float x2, float y2, float edgeTargetZ, float x3, float y3, float edgeStartZ, byte r, byte g, byte b, byte a);
        void DrawMarker(int type, float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, float x4, float y4, float z4, byte r, byte g, byte b, byte a, bool v1, bool v2, int v3, bool v4, string v5, string v6, bool v7);
        int RequestScaleformMovie(string scaleformName);
        void PushScaleformMovieFunction(int handle, string functionName);
        void PushScaleformMovieFunctionParameterString(string arg);
        void PushScaleformMovieFunctionParameterBool(bool arg);
        void PushScaleformMovieFunctionParameterInt(int arg);
        void PushScaleformMovieFunctionParameterFloat(float arg);
        void PopScaleformMovieFunctionVoid();
        void DrawScaleformMovieFullscreen(int handle, int v1, int v2, int v3, int v4, int v5);
        void DrawScaleformMovie(int handle, float x, float y, float width, float height, int v1, int v2, int v3, int v4, int v5);
        void DrawScaleformMovie3dNonAdditive(int handle, float x1, float y1, float z1, float x2, float y2, float z2, int v1, int v2, int v3, float x3, float z3, float z4, int v4);
        void StartScreenEffect(string dEATHFAILMPIN, int v1, bool v2);
        void DrawScaleformMovie3d(int handle, float x1, float y1, float z1, float x2, float y2, float z2, int v1, int v2, int v3, float x3, float z3, float z4, int v4);
        void SetScaleformMovieAsNoLongerNeeded(ref int handle);
        void UseParticleFxAssetNextCall(string v);
        int StartParticleFxNonLoopedAtCoord(string effectName, float x, float y, float z, int v1, int v2, int v3, float scale, bool v4, bool v5, bool v6);
        void DrawRect(float x, float y, float width, float height, byte r, byte g, byte b, byte a, int v);
        void DrawText(string name, int screenX, int screenY, Font chaletLondon, float scale, Color color, AlignmentX center, bool v3, bool v4, int v5);
        void DrawSprite(string v1, string v2, float x, float y, float v3, float v4, int v5, byte r, byte g, byte b, byte a, int v6);
        void DrawLine(Position3D p1, Position3D p2, Color highlightColor_Edge);
        void DrawPoly(Position3D p3, Position3D p4, Position3D p1, Color highlightColor_Full);
        // Ui.GetTextScaleHeight(scale, (int)font)

    }
}
