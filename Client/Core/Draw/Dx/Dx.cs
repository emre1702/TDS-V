using System;
using System.Collections.Generic;
using TDS_Client.Data.Enums;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    // UIResText.Draw is top aligned
    // Graphics.DrawRect is center aligned (X & Y)
    internal abstract class Dx : IDisposable
    {
        public bool Activated { get; set; }

        public static int ResX;
        public static int ResY;
        public int FrontPriority;

        private readonly static List<Dx> _dxDraws = new List<Dx>();

        protected readonly List<Dx> Children = new List<Dx>();

        public Dx(int frontPriority = 0, bool activated = true)
        {
            Activated = activated;
            FrontPriority = frontPriority;
            _dxDraws.Add(this);
            _dxDraws.Sort((a, b) => a.FrontPriority.CompareTo(b.FrontPriority));
        }

        static Dx()
        {
            TickManager.Add(RenderAll);
        }

        ~Dx() => Dispose(false);

        public static void RenderAll()
        {
            foreach (Dx draw in _dxDraws)
            {
                if (draw.Activated)
                    draw.Draw();
            }
        }

        public static void RefreshResolution()
        {
            Graphics.GetActiveScreenResolution(ref ResX, ref ResY);
        }

        public virtual void Draw()
        {
        }

        public virtual void Remove()
        {
            _dxDraws.Remove(this);
            Dispose(true);
        }

        public virtual DxType GetDxType()
        {
            return DxType.None;
        }

        protected static int GetBlendValue(ulong currenttick, int start, int end, ulong starttick, ulong endtick)
        {
            float progress = (currenttick - starttick) / (endtick - starttick);
            if (progress > 1)
                progress = 1;
            return (int)Math.Floor(start + progress * (end - start));
        }

        protected static float GetBlendValue(ulong currenttick, float start, float end, ulong starttick, ulong endtick)
        {
            float progress = (currenttick - starttick) / (endtick - starttick);
            if (progress > 1)
                progress = 1;
            return (float)Math.Floor(start + progress * (end - start));
        }

        protected float GetRelativeX(float x, bool relative, bool isText = false)
        {
            return relative ? x : x / (isText ? 1920 : ResX);
        }

        protected float GetRelativeY(float y, bool relative, bool isText = false)
        {
            return relative ? y : y / (isText ? 1080 : ResY);
        }

        protected virtual int GetAbsoluteX(float x, bool relative, bool isText = false)
        {
            return (int)Math.Round(relative ? x * (isText ? 1920 : ResX) : x * (isText ? (1920 / ResX) : 1));
        }

        protected virtual int GetAbsoluteY(float y, bool relative, bool isText = false)
        {
            return (int)Math.Round(relative ? y * (isText ? 1080 : ResY) : y * (isText ? (1080 / ResY) : 1));
        }

        protected int GetTextAbsoluteHeight(float lineCount, float scale, Font font, bool relative)
        {
            int textHeight = GetAbsoluteY(Ui.GetTextScaleHeight(scale, (int)font), relative, true);

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
            }
        }

        #endregion IDisposable Support
    }
}
