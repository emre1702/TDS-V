using System;
using System.Collections.Generic;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx
{
    // UIResText.Draw is top aligned
    // Graphics.DrawRect is center aligned (X & Y)
    public abstract class DxBase : IDisposable
    {
        public bool Activated { get; set; }

        public int FrontPriority;

        protected readonly List<DxBase> Children = new List<DxBase>();

        protected readonly DxHandler DxHandler;
        protected readonly IModAPI ModAPI;

        protected DxBase(DxHandler dxHandler, IModAPI modAPI, int frontPriority = 0, bool activated = true)
        {
            DxHandler = dxHandler;
            ModAPI = modAPI;

            Activated = activated;
            FrontPriority = frontPriority;
            dxHandler.Add(this);
        }

        ~DxBase() => Dispose(false);

        public virtual void Draw()
        {
        }

        public virtual void Remove()
        {
            DxHandler.Remove(this);
            Dispose(true);
        }

        public virtual DxType GetDxType()
        {
            return DxType.None;
        }

        protected static int GetBlendValue(int currenttick, int start, int end, int starttick, int endtick)
        {
            float progress = (currenttick - starttick) / (endtick - starttick);
            if (progress > 1)
                progress = 1;
            return (int)Math.Floor(start + progress * (end - start));
        }

        protected static float GetBlendValue(int currenttick, float start, float end, int starttick, int endtick)
        {
            float progress = (currenttick - starttick) / (endtick - starttick);
            if (progress > 1)
                progress = 1;
            return (float)Math.Floor(start + progress * (end - start));
        }

        protected float GetRelativeX(float x, bool relative, bool isText = false)
        {
            return relative ? x : x / (isText ? 1920 : DxHandler.ResX);
        }

        protected float GetRelativeY(float y, bool relative, bool isText = false)
        {
            return relative ? y : y / (isText ? 1080 : DxHandler.ResY);
        }

        protected virtual int GetAbsoluteX(float x, bool relative, bool isText = false)
        {
            return (int)Math.Round(relative ? x * (isText ? 1920 : DxHandler.ResX) : x * (isText ? (1920 / DxHandler.ResX) : 1));
        }

        protected virtual int GetAbsoluteY(float y, bool relative, bool isText = false)
        {
            return (int)Math.Round(relative ? y * (isText ? 1080 : DxHandler.ResY) : y * (isText ? (1080 / DxHandler.ResY) : 1));
        }

        protected int GetTextAbsoluteHeight(float lineCount, float scale, Font font, bool relative)
        {
            int textHeight = GetAbsoluteY(ModAPI.Graphics.GetTextScaleHeight(scale, font), relative, true);

            // + 5 ... because of the margin between the lines
            textHeight = (int)(textHeight * lineCount + textHeight * 0.4 * (lineCount - 0.3));
            return textHeight;
        }

        #region IDisposable Support

        private bool _disposed = false; // To detect redundant calls

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (disposing)
            {
                foreach (var child in Children)
                    if (!child._disposed)
                        child.Remove();
                Children.Clear();
            }
        }

        #endregion IDisposable Support
    }
}
