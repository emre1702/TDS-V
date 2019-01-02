using RAGE.NUI;
using System;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    class Dx
    {
        public bool Activated { get; set; }
        protected ScreenResolutionType ScreenRes
        {
            get => Game.ScreenResolution;
        }

        private static List<Dx> dxDraws = new List<Dx>();

        public Dx(bool activated = true)
        {
            Activated = activated;
            dxDraws.Add(this);
        }

        public static void RenderAll()
        {
            foreach (Dx draw in dxDraws)
            {
                if (draw.Activated)
                    draw.Draw();
            }
        }

        protected virtual void Draw() { }

        public virtual void Remove()
        {
            dxDraws.Remove(this);
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
            return relative ? x : x / ScreenRes.Width;
        }

        protected float GetRelativeY(float y, bool relative)
        {
            return relative ? y : y / ScreenRes.Height;
        }

        protected int GetAbsoluteX(float x, bool relative)
        {
            return (int) (relative ? x * ScreenRes.Width : x);
        }

        protected int GetAbsoluteY(float y, bool relative)
        {
            return (int) (relative ? y * ScreenRes.Height : y);
        }
    }
}
