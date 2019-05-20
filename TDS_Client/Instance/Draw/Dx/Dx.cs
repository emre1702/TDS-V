using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    // UIResText.Draw is top aligned
    // Graphics.DrawRect is center aligned (X & Y)
    internal abstract class Dx : IDisposable
    {
        public bool Activated { get; set; }

        public static int ResX;
        public static int ResY;

        private readonly static List<Dx> _dxDraws = new List<Dx>();

        protected readonly List<Dx> Children = new List<Dx>();

        public Dx(bool activated = true)
        {
            Activated = activated;
            _dxDraws.Add(this);
        }

        ~Dx() => Dispose(false);

        public static void RenderAll()
        {
            for (int i = _dxDraws.Count - 1; i >= 0; --i)
            {
                Dx draw = _dxDraws[i];
                if (draw.Activated)
                    draw.Draw();
            }
        }

        public static void RefreshResolution()
        {
            Graphics.GetScreenResolution(ref ResX, ref ResY);
        }

        public virtual void Draw()
        {
        }

        public virtual void Remove()
        {
            _dxDraws.Remove(this);
            Dispose(true);
        }

        public virtual EDxType GetDxType()
        {
            return EDxType.None;
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

        protected float GetRelativeX(float x, bool relative)
        {
            return relative ? x : x / ResX;
        }

        protected float GetRelativeY(float y, bool relative)
        {
            return relative ? y : y / ResY;
        }

        protected int GetAbsoluteX(float x, bool relative)
        {
            return (int)Math.Round(relative ? x * ResX : x);
        }

        protected int GetAbsoluteY(float y, bool relative)
        {
            return (int)Math.Round(relative ? y * ResY : y);
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