using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.RAGE.Game.Graphics;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Graphics
{
    internal class GraphicsAPI : IGraphicsAPI
    {
        #region Public Methods

        public void DrawLine(float startX, float startY, float startZ, float endX, float endY, float endZ, byte r, byte g, byte b, byte a)
        {
            RAGE.Game.Graphics.DrawLine(startX, startY, startZ, endX, endY, endZ, r, g, b, a);
        }

        public void DrawLine(Position3D startPos, Position3D endPos, Color color)
        {
            RAGE.Game.Graphics.DrawLine(startPos.X, startPos.Y, startPos.Z, endPos.X, endPos.Y, endPos.Z, color.R, color.G, color.B, color.A);
        }

        /**
         * <summary>Doesn't work, use JS method instead</summary>
         */

        public void DrawMarker(MarkerType type, float posX, float posY, float posZ, float dirX, float dirY, float dirZ, float rotX, float rotY, float rotZ,
            float scaleX, float scaleY, float scaleZ, byte r, byte g, byte b, byte a,
            bool bobUpAndDown, bool faceCamera, bool rotate, string textureDict, string textureName, bool drawOnEnts)
        {
            RAGE.Game.Graphics.DrawMarker((int)type, posX, posY, posZ, dirX, dirY, dirZ, rotX, rotY, rotZ, scaleX, scaleY, scaleZ, r, g, b, a, bobUpAndDown, faceCamera, 2,
                rotate, textureDict, textureName, drawOnEnts);
        }

        public void DrawPoly(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, byte r, byte g, byte b, byte a)
        {
            RAGE.Game.Graphics.DrawPoly(x1, y1, z1, x2, y2, z2, x3, y3, z3, r, g, b, a);
        }

        public void DrawPoly(Position3D edge1, Position3D edge2, Position3D edge3, Color color)
        {
            RAGE.Game.Graphics.DrawPoly(edge1.X, edge1.Y, edge1.Z, edge2.X, edge2.Y, edge2.Z, edge3.X, edge3.Y, edge3.Z, color.R, color.G, color.B, color.A);
        }

        public void DrawRect(float x, float y, float width, float height, byte r, byte g, byte b, byte a)
        {
            RAGE.Game.Graphics.DrawRect(x, y, width, height, r, g, b, a, 0);
        }

        public void DrawScaleformMovie(int handle, float x, float y, float width, float height, int r, int g, int b, int a)
        {
            RAGE.Game.Graphics.DrawScaleformMovie(handle, x, y, width, height, r, g, b, a, 0);
        }

        public void DrawScaleformMovie3d(int handle, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, int p7, int p8, int p9,
            float scaleX, float scaleY, float scaleZ, int p13)
        {
            RAGE.Game.Graphics.DrawScaleformMovie3d(handle, posX, posY, posZ, rotX, rotY, rotZ, p7, p8, p9, scaleX, scaleY, scaleZ, p13);
        }

        public void DrawScaleformMovie3dNonAdditive(int handle, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, int p7, int p8, int p9,
            float scaleX, float scaleY, float scaleZ, int p13)
        {
            RAGE.Game.Graphics.DrawScaleformMovie3dNonAdditive(handle, posX, posY, posZ, rotX, rotY, rotZ, p7, p8, p9, scaleX, scaleY, scaleZ, p13);
        }

        public void DrawScaleformMovieFullscreen(int handle, int r, int g, int b, int a)
        {
            RAGE.Game.Graphics.DrawScaleformMovieFullscreen(handle, r, g, b, a, 0);
        }

        public void DrawSprite(string textureDict, string textureName, float screenX, float screenY, float width, float height, int heading, byte r, byte g, byte b, byte a)
        {
            RAGE.Game.Graphics.DrawSprite(textureDict, textureName, screenX, screenY, width, height, heading, r, g, b, a, 0);
        }

        public void DrawText(string text, int screenX, int screenY, Font font, float scale, Color color, Alignment Alignment, bool dropShadow, bool outline, int wordWrap)
        {
            RAGE.NUI.UIResText.Draw(text, screenX, screenY, (RAGE.Game.Font)font, scale, color, (RAGE.NUI.UIResText.Alignment)Alignment, dropShadow, outline, wordWrap);
        }

        public bool GetScreenCoordFromWorldCoord(float worldX, float worldY, float worldZ, ref float screenX, ref float screenY)
        {
            return RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(worldX, worldY, worldZ, ref screenX, ref screenY);
        }

        public void GetScreenResolution(ref int x, ref int y)
        {
            RAGE.Game.Graphics.GetScreenResolution(ref x, ref y);
        }

        public float GetTextScaleHeight(float scale, Font font)
        {
            return RAGE.Game.Ui.GetTextScaleHeight(scale, (int)font);
        }

        public bool HasScaleformMovieLoaded(int handle)
        {
            return RAGE.Game.Graphics.HasScaleformMovieLoaded(handle);
        }

        public void PopScaleformMovieFunctionVoid()
        {
            RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
        }

        public void PushScaleformMovieFunction(int handle, string functionName)
        {
            RAGE.Game.Graphics.PushScaleformMovieFunction(handle, functionName);
        }

        public void PushScaleformMovieFunctionParameterBool(bool arg)
        {
            RAGE.Game.Graphics.PushScaleformMovieFunctionParameterBool(arg);
        }

        public void PushScaleformMovieFunctionParameterFloat(float arg)
        {
            RAGE.Game.Graphics.PushScaleformMovieFunctionParameterFloat(arg);
        }

        public void PushScaleformMovieFunctionParameterInt(int arg)
        {
            RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(arg);
        }

        public void PushScaleformMovieFunctionParameterString(string arg)
        {
            RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(arg);
        }

        public int RequestScaleformMovie(string scaleformName)
        {
            return RAGE.Game.Graphics.RequestScaleformMovie(scaleformName);
        }

        public void SetScaleformMovieAsNoLongerNeeded(ref int handle)
        {
            RAGE.Game.Graphics.SetScaleformMovieAsNoLongerNeeded(ref handle);
        }

        public int StartParticleFxNonLoopedAtCoord(string effectName, float xPos, float yPos, float zPos, int xRot, int yRot, int zRot, float scale, bool xAxis, bool yAxis, bool zAxis)
        {
            return RAGE.Game.Graphics.StartParticleFxNonLoopedAtCoord(effectName, xPos, yPos, zPos, xRot, yRot, zRot, scale, xAxis, yAxis, zAxis);
        }

        public void StartScreenEffect(string effectName, int duration, bool looped)
        {
            RAGE.Game.Graphics.StartScreenEffect(effectName, duration, looped);
        }

        public void StopScreenEffect(string effectName)
        {
            RAGE.Game.Graphics.StopScreenEffect(effectName);
        }

        public void UseParticleFxAssetNextCall(string name)
        {
            RAGE.Game.Graphics.UseParticleFxAssetNextCall(name);
        }

        #endregion Public Methods
    }
}