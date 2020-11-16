using RAGE.Game;
using System.Drawing;
using TDS.Client.Data.Enums;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS.Client.Handler.Draw.Dx.Grid
{
    public class DxGridCell : DxBase
    {
        public DxGridRow Row;

        private readonly Alignment _Alignment;
        private readonly Color? _backColor;
        private readonly DxGridColumn _column;
        private DxText _dxText;

        public DxGridCell(DxHandler dxHandler, TimerHandler timerHandler, string text, DxGridRow row, DxGridColumn column, Color? backColor = null,
            Color? textColor = null, float? scale = null, Font? font = null,
            Alignment? alignment = null, int frontPriority = 0) : base(dxHandler, frontPriority, false)
        {
            Row = row;
            _column = column;
            _backColor = backColor;
            _Alignment = alignment ?? row.TextAlignment;

            _dxText = new DxText(dxHandler, timerHandler, text, column.X, Row.Y, scale ?? Row.Scale, textColor ?? Row.TextColor, font ?? Row.Font,
                alignment ?? Row.TextAlignment, AlignmentY.Center, _column.RelativePos, false, true, amountLines: 1, frontPriority: 99)
            {
                Activated = false
            };

            row.AddCell(this);
        }

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

        public override void Draw()
        {
            var x = GetXPos();
            if (_column.RelativePos)
                _dxText.SetRelativeX(x);
            else
                _dxText.SetAbsoluteX((int)x);

            if (Row.RelativePos)
                _dxText.SetRelativeY(Row.Y);
            else
                _dxText.SetAbsoluteY((int)Row.Y);
            _dxText.Draw();
        }

        public void DrawBackground()
        {
            Color backcolor = BackColor;
            RAGE.Game.Graphics.DrawRect(_column.X, Row.Y, _column.Width, Row.Height, backcolor.R, backcolor.G, backcolor.B, backcolor.A, 0);
        }

        public override DxType GetDxType()
        {
            return DxType.GridCell;
        }

        public override void Remove()
        {
            base.Remove();
            _dxText.Remove();
            _dxText = null;
        }

        public void SetText(string text)
        {
            _dxText.Text = text;
        }

        private float GetXPos()
        {
            switch (_Alignment)
            {
                case Alignment.Left:
                    return _column.X;

                case Alignment.Centered:
                    return _column.X + _column.Width / 2;

                case Alignment.Right:
                    return _column.X + _column.Width;
            }
            return 0;
        }
    }
}
