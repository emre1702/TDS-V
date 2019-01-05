using RAGE.Game;
using RAGE.NUI;
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
        private int xPos;
        private int yPos;
        private float scale;
        private Color color;
        private Font font;
        private UIResText.Alignment alignmentX;
        private EAlignmentY alignmentY;
        private bool relative;
        private bool dropShadow;
        private bool outline;
        private int wordWrap;

        private int? endAlpha;
        private ulong endAlphaStartTick;
        private ulong endAlphaEndTick;

        private float? endScale;
        private ulong endScaleStartTick;
        private ulong endScaleEndTick;

        public DxText(string text, float x, float y, float scale, Color color, Font font = Font.ChaletLondon,
            UIResText.Alignment alignmentX = UIResText.Alignment.Left, EAlignmentY alignmentY = EAlignmentY.Top, bool relative = true,
            bool dropShadow = false, bool outline = false, int wordWrap = 0) : base()
        {
            this.Text = text;
            this.xPos = GetAbsoluteX(x, relative);
            this.yPos = GetAbsoluteY(y, relative);
            this.scale = scale;
            this.color = color;
            this.font = font;
            this.alignmentX = alignmentX;
            this.alignmentY = alignmentY;
            this.relative = relative;

            ApplyTextAlignmentY();
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

        private void ApplyTextAlignmentY()
        {
            float textheight = Ui.GetTextScaleHeight(scale, (int)font);
            if (alignmentY == EAlignmentY.Center)
                yPos -= GetAbsoluteY(textheight / 2, true) - 5;
            else if (alignmentY == EAlignmentY.Bottom)
                yPos -= GetAbsoluteY(textheight, true) - 5;
        }

        public void SetScale(float scale)
        {
            this.scale = scale;
            this.endScale = null;
        }

        public override void Draw()
        {
            ulong elapsedticks = TimerManager.ElapsedTicks;

            int alpha = color.A;
            if (endAlpha.HasValue)
                alpha = GetBlendValue(elapsedticks, color.A, endAlpha.Value, endAlphaStartTick, endAlphaEndTick);

            float scale = this.scale;
            if (endScale.HasValue)
            {
                if (elapsedticks >= endScaleEndTick)
                {
                    this.scale = endScale.Value;
                    scale = this.scale;
                    endScale = null;
                } 
                else
                    scale = GetBlendValue(elapsedticks, this.scale, endScale.Value, endScaleStartTick, endScaleEndTick);
                
            }
                
            UIResText.Draw(Text, xPos, yPos, font, scale, color, alignmentX, dropShadow, outline, wordWrap);
        }

        public override EDxType GetDxType()
        {
            return EDxType.Text;
        }
    }
}
