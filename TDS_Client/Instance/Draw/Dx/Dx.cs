using RAGE.NUI;
using System;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    // UIResText.Draw is top aligned
    // Graphics.DrawRect is center aligned (X & Y)
    class Dx
    {
        public bool Activated { get; set; }

        public static int ResX;
        public static int ResY;

        private readonly static List<Dx> dxDraws = new List<Dx>();

        public Dx(bool activated = true)
        {
            Activated = activated;
            dxDraws.Add(this);
        }

        public static void RenderAll()
        {
            for (int i = dxDraws.Count - 1; i >= 0; --i)
            {
                Dx draw = dxDraws[i];
                if (draw.Activated)
                    draw.Draw();
            }
        }

        public static void RefreshResolution()
        {
            RAGE.Game.Graphics.GetActiveScreenResolution(ref ResX, ref ResY);
        }

        public virtual void Draw() { }

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
            return relative ? x : x / ResX;
        }

        protected float GetRelativeY(float y, bool relative)
        {
            return relative ? y : y / ResY;
        }

        protected int GetAbsoluteX(float x, bool relative)
        {
            return (int) Math.Round(relative ? x * ResX : x);
        }

        protected int GetAbsoluteY(float y, bool relative)
        {
            return (int)Math.Round(relative ? y * ResY : y);
        }
    }
}
