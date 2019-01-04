using RAGE.Game;
using RAGE.NUI;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    class DxGridRow : Dx
    {
        public string text;
        public float Y;
        private float? height;
        public Color BackColor;
        public Color TextColor;
        public float Scale;
        public Font Font;
        public UIResText.Alignment Alignment;
        public bool RelativePos;

        public float Height => height ?? Grid.RowHeight;

        public DxGrid Grid;

        public List<DxGridCell> Cells = new List<DxGridCell>();

        public bool UseColorForWholeRow = true;

        public DxGridRow(float? height, Color backColor, Color textColor, string text = null, float scale = 1f, Font font = Font.ChaletLondon, UIResText.Alignment alignment = UIResText.Alignment.Left, bool relative = true) : base(false)
        {
            this.height = height;
            BackColor = backColor;
            TextColor = textColor;
            this.text = text;
            Scale = scale;
            Font = font;
            Alignment = alignment;
            RelativePos = relative;
        }

        public float Draw()
        {
            if (UseColorForWholeRow)
            {
                if (Grid.Alignment == UIResText.Alignment.Left)
                    Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
                else
                    Graphics.DrawRect(Grid.X - Grid.Width / 2, Y - Height / 2, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
            }
            foreach (var cell in Cells)
            {
                if (!UseColorForWholeRow)
                    cell.DrawBackground();
                cell.Draw();
            }
            if (text != null)
            {
                if (Grid.Alignment == UIResText.Alignment.Left)
                    UIText.Draw(text, new Point(GetAbsoluteX(Grid.X, RelativePos), GetAbsoluteY(Grid.Y, RelativePos)), Scale, TextColor, Font, false);
                else
                    UIText.Draw(text, new Point(GetAbsoluteX(Grid.X - Grid.Width / 2, RelativePos), GetAbsoluteY(Grid.Y - Height / 2, RelativePos)), Scale, TextColor, Font, true);
            }
            return Y + Height;
        }

        public void DrawAsHeader()
        {
            Y = Grid.Y;
            Draw();
        }

        public void AddCell(DxGridCell cell)
        {
            Cells.Add(cell);
            Color color = cell.BackColor;
            if (color != null && color != BackColor)
            {
                UseColorForWholeRow = false;
            }
        }

        public void CheckUseColorForWholeRow()
        {
            foreach (var cell in Cells)
            {
                Color color = cell.BackColor;
                if (color != null && color != BackColor)
                {
                    UseColorForWholeRow = false;
                    return;
                }
            }
            UseColorForWholeRow = true;
        }

        public override EDxType GetDxType()
        {
            return EDxType.GridRow;
        }
    }
}
