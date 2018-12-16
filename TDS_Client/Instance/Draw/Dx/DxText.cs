using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxText : Dx
    {
        private string text;
        private Point position;
        private float scale;
        private Color color;
        private Font font;
        private Alignment alignment;

        private int? endAlpha;
        private int endAlphaStartTick;
        private int endAlphaEndTick;

        private float? endScale;
        private int endScaleStartTick;
        private int endScaleEndTick;

        public DxText(string text, float x, float y, float scale, Color color, Font font, Alignment alignment, bool relative = true) : base()
        {
            this.text = text;
            position = new Point(
                GetAbsoluteX(x, relative),
                GetAbsoluteY(y, relative)
            );
            if (alignment == Alignment.Right)
            {
                position.X -= GetStringWidth(text, scale, font);
            }
            this.scale = scale;
            this.color = color;
            this.font = font;
            this.alignment = alignment;
        }

        public void BlendAlpha(int endAlpha, int msToEnd)
        {
            this.endAlpha = endAlpha;
            endAlphaStartTick = Environment.TickCount;
            endAlphaEndTick = endAlphaStartTick + msToEnd;
        }

        public void BlendScale(float endScale, int msToEnd)
        {
            this.endScale = endScale;
            endScaleStartTick = Environment.TickCount;
            endScaleEndTick = endScaleStartTick + msToEnd;
        }

        private static int GetStringWidth(string text, float scale, Font font)
        {
            Ui.BeginTextCommandWidth("STRING");
            for (int i = 0; i < text.Length; i += 99)
            {
                string substr = text.Substring(i, Math.Min(99, text.Length - i));
                Ui.AddTextComponentSubstringPlayerName(substr);
            }
            Ui.SetTextFont((int)font);
            Ui.SetTextScale(scale, scale);
            return (int) Ui.EndTextCommandGetWidth(1);
        }

        protected override void Draw(int currentTick)
        {
            int alpha = color.A;
            if (endAlpha.HasValue)
                alpha = GetBlendValue(currentTick, color.A, endAlpha.Value, endAlphaStartTick, endAlphaEndTick);

            float scale = this.scale;
            if (endScale.HasValue)
                scale = GetBlendValue(currentTick, this.scale, endScale.Value, endScaleStartTick, endScaleEndTick);

            UIText.Draw(text, position, scale, color, font, alignment == Alignment.Center);
        }

        public override EDxType GetDxType()
        {
            return EDxType.Text;
        }
    }
}
