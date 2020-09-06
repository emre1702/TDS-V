using RAGE;
using System.Drawing;
using TDS_Client.Data.Enums;
using static RAGE.NUI.UIResText;

namespace TDS_Client.Handler.Draw.Dx
{
    internal class DxRectangle : DxBase
    {
        private readonly Color _color;

        private Alignment _Alignment;

        private float _xPos,
                            _yPos,
            _sizeX,
            _sizeY;

        public DxRectangle(DxHandler dxHandler, float x, float y, float width, float height, Color color,
            Alignment alignmentX = Alignment.Left, AlignmentY alignmentY = AlignmentY.Top,
            bool relativePos = true, int frontPriority = 0) : base(dxHandler, frontPriority: frontPriority)
        {
            _xPos = GetRelativeX(x, relativePos);
            _yPos = GetRelativeY(y, relativePos);
            _sizeX = GetRelativeX(width, relativePos);
            _sizeY = GetRelativeY(height, relativePos);

            this._color = color;
            this._Alignment = alignmentX;

            if (alignmentX == Alignment.Left)
                _xPos += _sizeX / 2;
            else if (alignmentX == Alignment.Right)
                _xPos -= _sizeX / 2;

            if (alignmentY == AlignmentY.Top)
                _yPos += _sizeY / 2;
            else if (alignmentY == AlignmentY.Bottom)
                _yPos -= _sizeY / 2;
        }

        public override void Draw()
        {
            RAGE.Game.Graphics.DrawRect(_xPos, _yPos, _sizeX, _sizeY, _color.R, _color.G, _color.B, _color.A, 0);
        }

        public void SetAlignment(Alignment newAlignment)
        {
            // convert old back
            if (_Alignment == Alignment.Left)
                _xPos -= _sizeX / 2;
            else if (_Alignment == Alignment.Right)
                _xPos += _sizeX / 2;

            // align new
            if (newAlignment == Alignment.Left)
                _xPos += _sizeX / 2;
            else if (newAlignment == Alignment.Right)
                _xPos -= _sizeX / 2;

            _Alignment = newAlignment;
        }

        public void SetHeight(float height)
        {
            _yPos -= _sizeY / 2;
            _sizeY = height;
            _yPos += height / 2;
        }

        public void SetWidth(float width, bool relativePos)
        {
            Alignment currentalignment = _Alignment;
            SetAlignment(Alignment.Centered);
            _sizeX = GetRelativeX(width, relativePos);
            SetAlignment(currentalignment);
        }
    }
}
