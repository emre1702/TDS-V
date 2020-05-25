using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Draw.Dx
{
    internal class DxText : DxBase
    {
        #region Public Fields

        public string Text;

        #endregion Public Fields

        #region Private Fields

        private readonly AlignmentX _alignmentX;
        private readonly AlignmentY _alignmentY;
        private readonly int _amountLines;
        private readonly Color _color;
        private readonly bool _dropShadow;
        private readonly Font _font;
        private readonly bool _outline;
        private readonly bool _relative;
        private readonly TimerHandler _timerHandler;
        private readonly int _wordWrap;
        private int? _endAlpha;
        private int _endAlphaEndTick;
        private int _endAlphaStartTick;
        private float? _endScale;
        private int _endScaleEndTick;
        private int _endScaleStartTick;
        private float _scale;
        private int _x;
        private int _y;

        #endregion Private Fields

        #region Public Constructors

        public DxText(DxHandler dxHandler, IModAPI modAPI, TimerHandler timerHandler, string text, float x, float y, float scale, Color color, Font font = Font.ChaletLondon,
            AlignmentX alignmentX = AlignmentX.Left, AlignmentY alignmentY = AlignmentY.Top, bool relative = true,
            bool dropShadow = false, bool outline = false, int wordWrap = 999, int amountLines = 0, int frontPriority = 0) : base(dxHandler, modAPI, frontPriority: frontPriority)
        {
            _timerHandler = timerHandler;

            Text = text;
            _x = GetAbsoluteX(x, relative);
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

        #endregion Public Constructors

        #region Public Methods

        public void BlendAlpha(int endAlpha, int msToEnd)
        {
            this._endAlpha = endAlpha;
            _endAlphaStartTick = _timerHandler.ElapsedMs;
            _endAlphaEndTick = _endAlphaStartTick + msToEnd;
        }

        public void BlendScale(float endScale, int msToEnd)
        {
            this._endScale = endScale;
            _endScaleStartTick = _timerHandler.ElapsedMs;
            _endScaleEndTick = _endScaleStartTick + msToEnd;
        }

        public override void Draw()
        {
            int elapsedMs = _timerHandler.ElapsedMs;

            Color theColor = _color;
            if (_endAlpha.HasValue)
                theColor = Color.FromArgb(GetBlendValue(elapsedMs, _color.A, _endAlpha.Value, _endAlphaStartTick, _endAlphaEndTick), _color);

            float scale = this._scale;
            if (_endScale.HasValue)
            {
                if (elapsedMs >= _endScaleEndTick)
                {
                    this._scale = _endScale.Value;
                    scale = this._scale;
                    _endScale = null;
                }
                else
                    scale = GetBlendValue(elapsedMs, this._scale, _endScale.Value, _endScaleStartTick, _endScaleEndTick);
            }

            ModAPI.Graphics.DrawText(Text, _x, _y, _font, scale, theColor, _alignmentX, _dropShadow, _outline, _wordWrap);
        }

        public override DxType GetDxType()
        {
            return DxType.Text;
        }

        public void SetAbsoluteX(int x)
        {
            _x = x;
        }

        public void SetAbsoluteY(int y)
        {
            _y = y;
            ApplyTextAlignmentY();
        }

        public void SetRelativeX(float x)
        {
            _x = GetAbsoluteX(x, true);
        }

        public void SetRelativeY(float y)
        {
            _y = GetAbsoluteY(y, true);
            ApplyTextAlignmentY();
        }

        public void SetScale(float scale)
        {
            this._scale = scale;
            this._endScale = null;
        }

        #endregion Public Methods

        #region Private Methods

        private void ApplyTextAlignmentY()
        {
            int textHeight = GetTextAbsoluteHeight(_amountLines != 0 ? _amountLines : GetLineCount(), _scale, _font, _relative);

            if (_alignmentY == AlignmentY.Center)
                _y -= textHeight / 2;
            else if (_alignmentY == AlignmentY.Bottom)
                _y -= textHeight;

            // ((((UI::_GET_TEXT_SCALE_HEIGHT(0.35f, 0) * iVar6) + 0.00138888f * 13f) + 0.00138888f
            // * 5f * (iVar6 - 1) * 0.5f)) - 0.00138888f)
        }

        private int GetAbsoluteX(float x, bool relative)
        {
            return GetAbsoluteX(x, relative, true);
        }

        private int GetAbsoluteY(float y, bool relative)
        {
            return GetAbsoluteY(y, relative, true);
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

        private int GetLineCount()
        {
            ModAPI.Native.Invoke(NativeHash._BEGIN_TEXT_COMMAND_LINE_COUNT, "STRING");
            ModAPI.Native.Invoke(NativeHash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Text);
            return ModAPI.Native.Invoke<int>(NativeHash._GET_TEXT_SCREEN_LINE_COUNT, _x, _y);
        }

        #endregion Private Methods
    }
}
