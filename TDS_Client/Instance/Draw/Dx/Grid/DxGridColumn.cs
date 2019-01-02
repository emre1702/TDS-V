using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    class DxGridColumn : Dx
    {
        public float X { get; set; }
        public float Width;
        public bool RelativePos;

        public DxGridColumn(float width, bool relative = true) : base(false)
        {
            Width = width;
            RelativePos = relative;
        }

        public override EDxType GetDxType()
        {
            return EDxType.GridColumn;
        }
    }
}
