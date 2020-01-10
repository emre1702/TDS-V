using RAGE.Game;
using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx
{
    internal class DxText : Dx
    {
        public string Text;
        private readonly int _xPos;
        private int _y;
        private float _scale;
        private readonly Color _color;
        private readonly Font _font;
        private readonly UIResText.Alignment _alignmentX;
        private readonly EAlignmentY _alignmentY;
        private readonly bool _relative;
        private readonly bool _dropShadow;
        private readonly bool _outline;
        private readonly int _wordWrap;
        private readonly int _amountLines;

        private int? _endAlpha;
        private ulong _endAlphaStartTick;
        private ulong _endAlphaEndTick;

        private float? _endScale;
        private ulong _endScaleStartTick;
        private ulong _endScaleEndTick;

        public DxText(string text, float x, float y, float scale, Color color, Font font = Font.ChaletLondon,
            UIResText.Alignment alignmentX = UIResText.Alignment.Left, EAlignmentY alignmentY = EAlignmentY.Top, bool relative = true,
            bool dropShadow = false, bool outline = false, int wordWrap = 999, int amountLines = 1, int frontPriority = 0) : base(frontPriority: frontPriority)
        {
            Text = text;
            _xPos = GetAbsoluteX(x, relative);
            _y = GetAbsoluteY(y, relative);
            _scale = scale;
            _color = color;
            _font = font;
            _alignmentX = alignmentX;
            _alignmentY = alignmentY;
            _relative = relative;
            _dropShadow = dropShadow;
            _outline = outline;
            _wordWrap = wordWrap;
            _amountLines = amountLines;

            ApplyTextAlignmentY();
        }

        protected override int GetAbsoluteX(float x, bool relative)
        {
            return (int)Math.Round(relative ? x * 1920 : x);
        }

        protected override int GetAbsoluteY(float y, bool relative)
        {
            return (int)Math.Round(relative ? y * 1080 : y);
        }

        public void SetAbsoluteY(int y)
        {
            _y = y;
            ApplyTextAlignmentY();
        }

        public void SetRelativeY(float y)
        {
            _y = GetAbsoluteY(y, _relative);
            ApplyTextAlignmentY();
        }

        public void BlendAlpha(int endAlpha, ulong msToEnd)
        {
            this._endAlpha = endAlpha;
            _endAlphaStartTick = TimerManager.ElapsedTicks;
            _endAlphaEndTick = _endAlphaStartTick + msToEnd;
        }

        public void BlendScale(float endScale, ulong msToEnd)
        {
            this._endScale = endScale;
            _endScaleStartTick = TimerManager.ElapsedTicks;
            _endScaleEndTick = _endScaleStartTick + msToEnd;
        }

        /*private static int GetStringWidth(string text, float scale, Font font)
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
        }*/

        private void ApplyTextAlignmentY()
        {
            float textheight = Ui.GetTextScaleHeight(_scale, (int)_font);
            if (_alignmentY == EAlignmentY.Center)
                _y -= GetAbsoluteY(textheight * _amountLines / 2, true);
            else if (_alignmentY == EAlignmentY.Bottom)
                _y -= GetAbsoluteY(textheight * _amountLines, true);
        }

        public void SetScale(float scale)
        {
            this._scale = scale;
            this._endScale = null;
        }

        public override void Draw()
        {
            ulong elapsedticks = TimerManager.ElapsedTicks;

            Color theColor = _color;
            if (_endAlpha.HasValue)
                theColor = Color.FromArgb(GetBlendValue(elapsedticks, _color.A, _endAlpha.Value, _endAlphaStartTick, _endAlphaEndTick), _color);

            float scale = this._scale;
            if (_endScale.HasValue)
            {
                if (elapsedticks >= _endScaleEndTick)
                {
                    this._scale = _endScale.Value;
                    scale = this._scale;
                    _endScale = null;
                }
                else
                    scale = GetBlendValue(elapsedticks, this._scale, _endScale.Value, _endScaleStartTick, _endScaleEndTick);
            }

            UIResText.Draw(Text, _xPos, _y, _font, scale, theColor, _alignmentX, _dropShadow, _outline, _wordWrap);
        }

        public override EDxType GetDxType()
        {
            return EDxType.Text;
        }
    }
}
