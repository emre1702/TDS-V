using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Graphics
{
    public interface IGraphicsAPI
    {
        #region Public Methods

        void DrawLine(float startX, float startY, float startZ, float endX, float endY, float endZ, byte r, byte g, byte b, byte a);

        void DrawLine(Position startPos, Position endPos, Color color);

        void DrawMarker(MarkerType type, float posX, float posY, float posZ, float dirX, float dirY, float dirZ, float rotX, float rotY, float rotZ,
            float scaleX, float scaleY, float scaleZ, byte r, byte g, byte b, byte a,
            bool bobUpAndDown, bool faceCamera, bool rotate, string textureDict, string textureName, bool drawOnEnts);

        void DrawPoly(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, byte r, byte g, byte b, byte a);

        void DrawPoly(Position edge1, Position edge2, Position edge3, Color color);

        void DrawRect(float x, float y, float width, float height, byte r, byte g, byte b, byte a);

        void DrawScaleformMovie(int handle, float x, float y, float width, float height, int r, int g, int b, int a);

        void DrawScaleformMovie3d(int handle, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, int p7, int p8, int p9,
            float scaleX, float scaleY, float scaleZ, int p13);

        void DrawScaleformMovie3dNonAdditive(int handle, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, int p7, int p8, int p9,
            float scaleX, float scaleY, float scaleZ, int p13);

        void DrawScaleformMovieFullscreen(int handle, int r, int g, int b, int a);

        void DrawSprite(string textureDict, string textureName, float screenX, float screenY, float width, float height, int heading, byte r, byte g, byte b, byte a);

        void DrawText(string text, int screenX, int screenY, Font font, float scale, Color color, AlignmentX alignmentX, bool dropShadow, bool outline, int wordWrap);

        bool GetScreenCoordFromWorldCoord(float worldX, float worldY, float worldZ, ref float screenX, ref float screenY);

        void GetScreenResolution(ref int x, ref int y);

        // Graphics.GetActiveScreenResolution(ref ResX, ref ResY);
        float GetTextScaleHeight(float scale, Font font);

        bool HasScaleformMovieLoaded(int handle);

        void PopScaleformMovieFunctionVoid();

        void PushScaleformMovieFunction(int handle, string functionName);

        void PushScaleformMovieFunctionParameterBool(bool arg);

        void PushScaleformMovieFunctionParameterFloat(float arg);

        void PushScaleformMovieFunctionParameterInt(int arg);

        void PushScaleformMovieFunctionParameterString(string arg);

        int RequestScaleformMovie(string scaleformName);

        void SetScaleformMovieAsNoLongerNeeded(ref int handle);

        int StartParticleFxNonLoopedAtCoord(string effectName, float xPos, float yPos, float zPos, int xRot, int yRot, int zRot, float scale, bool xAxis, bool yAxis, bool zAxis);

        void StartScreenEffect(string effectName, int duration, bool looped);

        void StopScreenEffect(string effectName);

        /**
         * <summary>
         *  dirX/Y/Z represent a heading on each axis in which the marker should
         face, alternatively you can rotate each axis independently with rotX/Y/Z (and
         set dirX/Y/Z all to 0). faceCamera - Rotates only the y-axis (the heading) towards
         the camera p19 - no effect, default value in script is 2 rotate - Rotates only
         on the y-axis (the heading) textureDict - Name of texture dictionary to load
         texture from (e.g. GolfPutting) textureName - Name of texture inside dictionary
         to load (e.g. PuttingMarker) drawOnEnts - Draws the marker onto any entities
         that intersect it basically what he said, except textureDict and textureName
         are totally not const char, or if so, then they are always set to 0/NULL/nullptr
         in every script I checked, eg: bj.c: graphics::draw_marker(6, vParam0, 0f, 0f,
         1f, 0f, 0f, 0f, 4f, 4f, 4f, 240, 200, 80, iVar1, 0, 0, 2, 0, 0, 0, false); his
         is what I used to draw an amber downward pointing chevron V, has to be redrawn
         every frame. The 180 is for 180 degrees rotation around the Y axis, the 50 is
         alpha, assuming max is 100, but it will accept 255. GRAPHICS::DRAW_MARKER(2,
         v.x, v.y, v.z + 2, 0, 0, 0, 0, 180, 0, 2, 2, 2, 255, 128, 0, 50, 0, 1, 1, 0,
         0, 0, 0);
         * </summary>
         * */

        void UseParticleFxAssetNextCall(string name);

        #endregion Public Methods

        // Ui.GetTextScaleHeight(scale, (int)font)
    }
}
