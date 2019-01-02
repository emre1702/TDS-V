using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxText : Dx
    {
        public string Text;
        private Point position;
        private float scale;
        private Color color;
        private Font font;
        private Alignment alignment;

        private int? endAlpha;
        private ulong endAlphaStartTick;
        private ulong endAlphaEndTick;

        private float? endScale;
        private ulong endScaleStartTick;
        private ulong endScaleEndTick;

        public DxText(string text, float x, float y, float scale, Color color, Font font = Font.ChaletLondon, Alignment alignment = Alignment.Left, bool relative = true) : base()
        {
            this.Text = text;
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

        public void BlendAlpha(int endAlpha, ulong msToEnd)
        {
            this.endAlpha = endAlpha;
            endAlphaStartTick = TimerManager.ElapsedTicks;
            endAlphaEndTick = endAlphaStartTick + msToEnd;
        }

        public void BlendScale(float endScale, ulong msToEnd)
        {
            this.endScale = endScale;
            endScaleStartTick = TimerManager.ElapsedTicks;
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

        public void SetText(string text)
        {
            if (alignment == Alignment.Right)
                position.X += GetStringWidth(this.Text, scale, font);
            this.Text = text;
            if (alignment == Alignment.Right)
                position.X -= GetStringWidth(text, scale, font);
        }

        public void SetScale(float scale)
        {
            this.scale = scale;
        }

        protected override void Draw()
        {
            int alpha = color.A;
            if (endAlpha.HasValue)
                alpha = GetBlendValue(TimerManager.ElapsedTicks, color.A, endAlpha.Value, endAlphaStartTick, endAlphaEndTick);

            float scale = this.scale;
            if (endScale.HasValue)
            {
                scale = GetBlendValue(TimerManager.ElapsedTicks, this.scale, endScale.Value, endScaleStartTick, endScaleEndTick);
                if (endScale.Value == scale)
                {
                    this.scale = endScale.Value;
                    endScale = null;
                }
            }
                
            UIText.Draw(Text, position, scale, color, font, alignment == Alignment.Center);
        }

        public override EDxType GetDxType()
        {
            return EDxType.Text;
        }
    }
}
