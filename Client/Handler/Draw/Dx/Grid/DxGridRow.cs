using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx.Grid
{
    public class DxGridRow : DxBase
    {
        #region Public Fields

        public Color BackColor;
        public List<DxGridCell> Cells = new List<DxGridCell>();
        public Font Font;
        public DxGrid Grid;
        public bool RelativePos;
        public float Scale;
        public string Text;
        public AlignmentX TextAlignment;
        public Color TextColor;
        public float TextHeight;
        public bool UseColorForWholeRow = true;
        public float Y;

        #endregion Public Fields

        #region Private Fields

        private readonly float? _height;

        #endregion Private Fields

        #region Public Constructors

        public DxGridRow(DxHandler dxHandler, IModAPI modAPI, DxGrid grid, float? height, Color backColor, Color? textColor = null, string text = null, float scale = 0.4f,
            Font font = Font.ChaletLondon, AlignmentX textAlignment = AlignmentX.Left, bool isHeader = false, bool relative = true, int frontPriority = 0)
            : base(dxHandler, modAPI, frontPriority, false)
        {
            this.Grid = grid;
            this._height = height;
            BackColor = backColor;
            TextColor = textColor ?? Color.White;
            this.Text = text;
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

        #endregion Public Constructors

        #region Public Properties

        public float Height => _height ?? Grid.RowHeight;

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

        private void DrawBackground()
        {
            if (Grid.Alignment == AlignmentX.Left)
                ModAPI.Graphics.DrawRect(Grid.X - Grid.Width / 2, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A);
            else if (Grid.Alignment == AlignmentX.Center)
                ModAPI.Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A);
            else
                ModAPI.Graphics.DrawRect(Grid.X, Y, Grid.Width, Height, BackColor.R, BackColor.G, BackColor.B, BackColor.A);
        }

        private void DrawText()
        {
            if (TextAlignment == AlignmentX.Left)
                ModAPI.Graphics.DrawText(Text, GetAbsoluteX(Grid.X - Grid.Width / 2, true, true),
                    GetAbsoluteY(Y, RelativePos, true) - GetAbsoluteY(ModAPI.Ui.GetTextScaleHeight(Scale, Font) / 2, true, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
            else if (TextAlignment == AlignmentX.Center)
                ModAPI.Graphics.DrawText(Text, GetAbsoluteX(Grid.X, true, true), GetAbsoluteY(Y, RelativePos, true) - GetAbsoluteY(ModAPI.Ui.GetTextScaleHeight(Scale, Font) / 2, true, true) - 5,
                    Font, Scale, TextColor, TextAlignment, false, false, 999);
            else if (TextAlignment == AlignmentX.Right)
                ModAPI.Graphics.DrawText(Text, GetAbsoluteX(Grid.X + Grid.Width / 2, true, true), GetAbsoluteY(Y, RelativePos, true) - GetAbsoluteY(ModAPI.Ui.GetTextScaleHeight(Scale, Font) / 2,
                    true, true) - 5, Font, Scale, TextColor, TextAlignment, false, false, 999);
        }

        #endregion Private Methods
    }
}
