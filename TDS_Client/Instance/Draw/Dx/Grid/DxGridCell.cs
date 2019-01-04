using RAGE.Game;
using RAGE.NUI;
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
        private UIResText.Alignment? alignment;

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

        public DxGridCell(string text, DxGridRow row, DxGridColumn column, Color? backColor = null, Color? textColor = null, float? scale = null, Font? font = null, UIResText.Alignment? alignment = null) : base(false)
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

        public new void Draw()
        {
            int x = GetAbsoluteX(column.X + column.Width / 2, column.RelativePos);
            int y = GetAbsoluteY(Row.Y + Row.Height / 2, Row.RelativePos);
            UIResText.Draw(text, x, y, font ?? Row.Font, scale ?? Row.Scale, textColor ?? Row.TextColor, this.alignment ?? Row.Alignment, false, false, 0);
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
