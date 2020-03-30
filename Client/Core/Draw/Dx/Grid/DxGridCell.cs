using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    internal class DxGridCell : Dx
    {
        private string text;
        public DxGridRow Row;
        private DxGridColumn column;
        private Color? backColor;
        private Color? textColor;
        private float? scale;
        private Font? font;
        private UIResText.Alignment? alignment;

        private float? textHeight;

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

        public DxGridCell(string text, DxGridRow row, DxGridColumn column, Color? backColor = null, Color? textColor = null, float? scale = null, Font? font = null, 
            UIResText.Alignment? alignment = null, int frontPriority = 0) : base(frontPriority, false)
        {
            this.text = text;
            this.Row = row;
            this.column = column;
            this.backColor = backColor;
            this.textColor = textColor;
            this.scale = scale;
            this.font = font;
            this.alignment = alignment;

            if (scale.HasValue && font.HasValue)
                textHeight = Ui.GetTextScaleHeight(scale.Value, (int)font.Value);

            row.AddCell(this);
        }

        public override void Draw()
        {
            int y = GetAbsoluteY(GetRelativeY(Row.Y, Row.RelativePos), true, true) - 5;
            y -= (int)(textHeight ?? Row.TextHeight) / 2;
            UIResText.Alignment align = alignment ?? Row.TextAlignment;
            if (align == UIResText.Alignment.Left)
            {
                int x = GetAbsoluteX(column.X, column.RelativePos, true);
                UIResText.Draw(text, x, y, font ?? Row.Font, scale ?? Row.Scale, textColor ?? Row.TextColor, align, false, false, 0);
            }
            else if (align == UIResText.Alignment.Centered)
            {
                int x = GetAbsoluteX(column.X + column.Width / 2, column.RelativePos, true);
                UIResText.Draw(text, x, y, font ?? Row.Font, scale ?? Row.Scale, textColor ?? Row.TextColor, align, false, false, 0);
            }
            else
            {
                int x = GetAbsoluteX(column.X + column.Width, column.RelativePos, true);
                UIResText.Draw(text, x, y, font ?? Row.Font, scale ?? Row.Scale, textColor ?? Row.TextColor, align, false, false, 0);
            }
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
