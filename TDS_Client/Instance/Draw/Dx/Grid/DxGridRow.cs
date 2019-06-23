using RAGE.Game;
using RAGE.NUI;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    internal class DxGridRow : Dx
    {
        public string text;
        public float Y;
        private float? height;
        public Color BackColor;
        public Color TextColor;
        public float Scale;
        public Font Font;
        public UIResText.Alignment TextAlignment;
        public bool RelativePos;

        public float Height => height ?? Grid.RowHeight;
        public float TextHeight;

        public DxGrid Grid;

        public List<DxGridCell> Cells = new List<DxGridCell>();

        public bool UseColorForWholeRow = true;

        public DxGridRow(DxGrid grid, float? height, Color backColor, Color? textColor = null, string text = null, float scale = 0.4f, Font font = Font.ChaletLondon, UIResText.Alignment textAlignment = UIResText.Alignment.Left, bool isHeader = false, bool relative = true) : base(false)
        {
            this.Grid = grid;
            this.height = height;
            BackColor = backColor;
            TextColor = textColor ?? backColor.GetContrast();
            this.text = text;
            Scale = scale;
            Font = font;
            TextAlignment = textAlignment;
            RelativePos = relative;

            TextHeight = Ui.GetTextScaleHeight(scale, (int)font);

            if (isHeader)
                grid.SetHeader(this);
            else
                grid.AddRow(this);
        }

        private void DrawBackground()
        {
            if (Grid.Alignment == UIResText.Alignment.Left)
                Graphics.DrawRect(Grid.X - Grid.Width / 2, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
            else if (Grid.Alignment == UIResText.Alignment.Centered)
                Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
            else
                Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
        }

        private void DrawText()
        {
            if (TextAlignment == UIResText.Alignment.Left)
                UIResText.Draw(text, GetAbsoluteX(Grid.X - Grid.Width / 2, true), GetAbsoluteY(Y, RelativePos) - GetAbsoluteY(Ui.GetTextScaleHeight(Scale, (int)Font) / 2, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
            else if (TextAlignment == UIResText.Alignment.Centered)
                UIResText.Draw(text, GetAbsoluteX(Grid.X, true), GetAbsoluteY(Y, RelativePos) - GetAbsoluteY(Ui.GetTextScaleHeight(Scale, (int)Font) / 2, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
            else if (TextAlignment == UIResText.Alignment.Right)
                UIResText.Draw(text, GetAbsoluteX(Grid.X + Grid.Width / 2, true), GetAbsoluteY(Y, RelativePos) - GetAbsoluteY(Ui.GetTextScaleHeight(Scale, (int)Font) / 2, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
        }

        public new float Draw()
        {
            if (UseColorForWholeRow)
                DrawBackground();

            foreach (var cell in Cells)
            {
                if (!UseColorForWholeRow)
                    cell.DrawBackground();
                cell.Draw();
            }
            if (text != null)
                DrawText();

            return Y + Height / 2;
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