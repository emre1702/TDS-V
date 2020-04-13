using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx.Grid
{
    public class DxGridCell : DxBase
    {
        private string _text;
        public DxGridRow Row;
        private DxGridColumn _column;
        private Color? _backColor;
        private Color? _textColor;
        private float? _scale;
        private Font? _font;
        private AlignmentX? _alignmentX;

        private float? _textHeight;

        public Color BackColor
        {
            get => _backColor ?? Row.BackColor;
            set
            {
                if (value != null && value != Row.BackColor)
                    Row.UseColorForWholeRow = false;
                else
                    Row.CheckUseColorForWholeRow();
            }
        }

        public DxGridCell(DxHandler dxHandler, IModAPI modAPI, string text, DxGridRow row, DxGridColumn column, Color? backColor = null, Color? textColor = null, float? scale = null, Font? font = null,
            AlignmentX? alignment = null, int frontPriority = 0) : base(dxHandler, modAPI, frontPriority, false)
        {
            this._text = text;
            this.Row = row;
            this._column = column;
            this._backColor = backColor;
            this._textColor = textColor;
            this._scale = scale;
            this._font = font;
            this._alignmentX = alignment;

            if (scale.HasValue && font.HasValue)
                _textHeight = modAPI.Ui.GetTextScaleHeight(scale.Value, font.Value);

            row.AddCell(this);
        }

        public override void Draw()
        {
            int y = GetAbsoluteY(GetRelativeY(Row.Y, Row.RelativePos), true, true) - 5;
            y -= (int)(_textHeight ?? Row.TextHeight) / 2;
            AlignmentX align = _alignmentX ?? Row.TextAlignment;
            if (align == AlignmentX.Left)
            {
                int x = GetAbsoluteX(_column.X, _column.RelativePos, true);
                ModAPI.Graphics.DrawText(_text, x, y, _font ?? Row.Font, _scale ?? Row.Scale, _textColor ?? Row.TextColor, align, false, false, 0);
            }
            else if (align == AlignmentX.Center)
            {
                int x = GetAbsoluteX(_column.X + _column.Width / 2, _column.RelativePos, true);
                ModAPI.Graphics.DrawText(_text, x, y, _font ?? Row.Font, _scale ?? Row.Scale, _textColor ?? Row.TextColor, align, false, false, 0);
            }
            else
            {
                int x = GetAbsoluteX(_column.X + _column.Width, _column.RelativePos, true);
                ModAPI.Graphics.DrawText(_text, x, y, _font ?? Row.Font, _scale ?? Row.Scale, _textColor ?? Row.TextColor, align, false, false, 0);
            }
        }

        public void DrawBackground()
        {
            Color backcolor = BackColor;
            ModAPI.Graphics.DrawRect(_column.X, Row.Y, _column.Width, Row.Height, backcolor.R, backcolor.G, backcolor.B, backcolor.A);
        }

        public void SetText(string text)
        {
            this._text = text;
        }

        public override DxType GetDxType()
        {
            return DxType.GridCell;
        }
    }
}
