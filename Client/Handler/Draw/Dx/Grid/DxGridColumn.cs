using System.Linq;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx.Grid
{
    public class DxGridColumn : DxBase
    {
        #region Public Fields

        public bool RelativePos;
        public bool RelativeWidth;
        public float Width;

        #endregion Public Fields

        #region Public Constructors

        public DxGridColumn(DxHandler dxHandler, IModAPI modAPI, float width, DxGrid grid, bool relativePos = true, bool relativeWidth = true, int frontPriority = 0)
            : base(dxHandler, modAPI, frontPriority, false)
        {
            Width = relativeWidth ? width * grid.Width : width;
            RelativePos = relativePos;
            RelativeWidth = relativeWidth;

            X = grid.X + grid.Columns.Sum(c => c.RelativeWidth ? c.Width : c.Width * grid.Width);
            if (grid.Alignment == AlignmentX.Center)
                X -= grid.Width / 2;
            else if (grid.Alignment == AlignmentX.Right)
                X -= grid.Width;
            grid.Columns.Add(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public float X { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override DxType GetDxType()
        {
            return DxType.GridColumn;
        }

        #endregion Public Methods
    }
}
