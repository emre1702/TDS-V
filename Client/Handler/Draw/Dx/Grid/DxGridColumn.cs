using System.Linq;
using TDS.Client.Data.Enums;
using static RAGE.NUI.UIResText;

namespace TDS.Client.Handler.Draw.Dx.Grid
{
    public class DxGridColumn : DxBase
    {
        public bool RelativePos;
        public bool RelativeWidth;
        public float Width;

        public DxGridColumn(DxHandler dxHandler, float width, DxGrid grid, bool relativePos = true, bool relativeWidth = true, int frontPriority = 0)
            : base(dxHandler, frontPriority, false)
        {
            Width = relativeWidth ? width * grid.Width : width;
            RelativePos = relativePos;
            RelativeWidth = relativeWidth;

            X = grid.X + grid.Columns.Sum(c => c.RelativeWidth ? c.Width : c.Width * grid.Width);
            if (grid.Alignment == Alignment.Centered)
                X -= grid.Width / 2;
            else if (grid.Alignment == Alignment.Right)
                X -= grid.Width;
            grid.Columns.Add(this);
        }

        public float X { get; set; }

        public override DxType GetDxType()
        {
            return DxType.GridColumn;
        }
    }
}
