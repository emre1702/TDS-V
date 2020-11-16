using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Enums;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS.Client.Handler.Draw.Dx.Grid
{
    public class DxGridRow : DxBase
    {
        public Color BackColor;
        public List<DxGridCell> Cells = new List<DxGridCell>();
        public Font Font;
        public DxGrid Grid;
        public bool RelativePos;
        public float Scale;
        public string Text;
        public Alignment TextAlignment;
        public Color TextColor;
        public float TextHeight;
        public bool UseColorForWholeRow = true;
        public float Y;

        private readonly float? _height;

        public DxGridRow(DxHandler dxHandler, DxGrid grid, float? height, Color backColor, Color? textColor = null, string text = null, float scale = 0.4f,
            Font font = Font.ChaletLondon, Alignment textAlignment = Alignment.Left, bool isHeader = false, bool relative = true, int frontPriority = 0)
            : base(dxHandler, frontPriority, false)
        {
            Grid = grid;
            _height = height;
            BackColor = backColor;
            TextColor = textColor ?? Color.White;
            Text = text;
            Scale = scale;
            Font = font;
            TextAlignment = textAlignment;
            RelativePos = relative;

            TextHeight = GetTextAbsoluteHeight(1, Scale, Font, RelativePos);

            if (isHeader)
                grid.SetHeader(this);
            else
                grid.AddRow(this);
        }

        public float Height => _height ?? Grid.RowHeight;

        public void AddCell(DxGridCell cell, bool setPriority = true)
        {
            Cells.Add(cell);
            if (setPriority)
                cell.FrontPriority = FrontPriority + 1;
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
            if (Text != null)
                DrawText();

            return Y + Height / 2;
        }

        public override DxType GetDxType()
        {
            return DxType.GridRow;
        }

        private void DrawBackground()
        {
            if (Grid.Alignment == Alignment.Left)
                RAGE.Game.Graphics.DrawRect(Grid.X - Grid.Width / 2, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
            else if (Grid.Alignment == Alignment.Centered)
                RAGE.Game.Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
            else
                RAGE.Game.Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A, 0);
        }

        private void DrawText()
        {
            if (TextAlignment == Alignment.Left)
                RAGE.NUI.UIResText.Draw(Text, GetAbsoluteX(Grid.X - Grid.Width / 2, true, true),
                    GetAbsoluteY(Y, RelativePos, true) - GetAbsoluteY(RAGE.Game.Ui.GetTextScaleHeight(Scale, (int)Font) / 2, true, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
            else if (TextAlignment == Alignment.Centered)
                RAGE.NUI.UIResText.Draw(Text, GetAbsoluteX(Grid.X, true, true), GetAbsoluteY(Y, RelativePos, true) - GetAbsoluteY(Ui.GetTextScaleHeight(Scale, (int)Font) / 2, true, true) - 5,
                    Font, Scale, TextColor, TextAlignment, false, false, 999);
            else if (TextAlignment == Alignment.Right)
                RAGE.NUI.UIResText.Draw(Text, GetAbsoluteX(Grid.X + Grid.Width / 2, true, true), GetAbsoluteY(Y, RelativePos, true) - GetAbsoluteY(Ui.GetTextScaleHeight(Scale, (int)Font) / 2,
                    true, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
        }
    }
}
