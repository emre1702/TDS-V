using RAGE.NUI;
using System.Linq;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    class DxGridColumn : Dx
    {
        public float X { get; set; }
        public float Width;
        public bool RelativePos;
        public bool RelativeWidth;

        public DxGridColumn(float width, DxGrid grid, bool relativePos = true, bool relativeWidth = true) : base(false)
        {
            Width = relativeWidth ? width * grid.Width : width;
            RelativePos = relativePos;
            RelativeWidth = relativeWidth;

            X = grid.X + grid.Columns.Sum(c => c.RelativeWidth ? c.Width : c.Width*grid.Width);
            if (grid.Alignment == UIResText.Alignment.Centered)
                X -= grid.Width / 2;
            else if (grid.Alignment == UIResText.Alignment.Right)
                X -= grid.Width;
            grid.Columns.Add(this);
        }

        public override EDxType GetDxType()
        {
            return EDxType.GridColumn;
        }
    }
}
