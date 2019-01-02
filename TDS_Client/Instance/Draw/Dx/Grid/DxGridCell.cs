using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    class DxGridCell : Dx
    {
        private string text;
        public DxGridRow Row;
        private DxGridColumn column;
        private Color? backColor;
        private Color? textColor;
        private float? scale;
        private Font? font;
        private Alignment? alignment;

        public Color BackColor
        {
            get => backColor ?? Row.BackColor;
            set
            {
                if (value != null && value != Row.BackColor)
                    Row.UseColorForWholeRow = false;
                else
                    Row.CheckUseColorForWholeRow();
            }
        }

        public DxGridCell(string text, DxGridRow row, DxGridColumn column, Color? backColor = null, Color? textColor = null, float? scale = null, Font? font = null, Alignment? alignment = null) : base(false)
        {
            this.text = text;
            this.Row = row;
            this.column = column;
            this.backColor = backColor;
            this.textColor = textColor;
            this.scale = scale;
            this.font = font;
            this.alignment = alignment;
        }

        public void Draw()
        {
            Alignment alignment = this.alignment ?? Row.Alignment;
            float x = alignment == Alignment.Left ? column.X : (column.X + column.Width / 2);
            float y = Row.Y + Row.Height / 2;
            Point pos = new Point(GetAbsoluteX(x, column.RelativePos), GetAbsoluteY(y, Row.RelativePos));
            UIText.Draw(text, pos, scale ?? Row.Scale, textColor ?? Row.TextColor, font ?? Row.Font, alignment == Alignment.Center);
        }

        public void DrawBackground()
        {
            Color backcolor = BackColor;
            Graphics.DrawRect(column.X, Row.Y, column.Width, Row.Height, backcolor.R, backcolor.G, backcolor.B, backcolor.A, 0);
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public override EDxType GetDxType()
        {
            return EDxType.GridCell;
        }
    }
}
