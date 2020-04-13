using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx
{
    internal class DxRectangle : DxBase
    {
        private float _xPos,
            _yPos,
            _sizeX,
            _sizeY;

        private readonly Color _color;
        private AlignmentX _alignmentX;

        public DxRectangle(DxHandler dxHandler, IModAPI modAPI, float x, float y, float width, float height, Color color,
            AlignmentX alignmentX = AlignmentX.Left, AlignmentY alignmentY = AlignmentY.Top,
            bool relativePos = true, int frontPriority = 0) : base(dxHandler, modAPI, frontPriority: frontPriority)
        {
            _xPos = GetRelativeX(x, relativePos);
            _yPos = GetRelativeY(y, relativePos);
            _sizeX = GetRelativeX(width, relativePos);
            _sizeY = GetRelativeY(height, relativePos);

            this._color = color;
            this._alignmentX = alignmentX;

            if (alignmentX == AlignmentX.Left)
                _xPos += _sizeX / 2;
            else if (alignmentX == AlignmentX.Right)
                _xPos -= _sizeX / 2;

            if (alignmentY == AlignmentY.Top)
                _yPos += _sizeY / 2;
            else if (alignmentY == AlignmentY.Bottom)
                _yPos -= _sizeY / 2;
        }

        public void SetAlignment(AlignmentX newalignmentX)
        {
            // convert old back
            if (_alignmentX == AlignmentX.Left)
                _xPos -= _sizeX / 2;
            else if (_alignmentX == AlignmentX.Right)
                _xPos += _sizeX / 2;

            // align new
            if (newalignmentX == AlignmentX.Left)
                _xPos += _sizeX / 2;
            else if (newalignmentX == AlignmentX.Right)
                _xPos -= _sizeX / 2;

            _alignmentX = newalignmentX;
        }

        public void SetWidth(float width, bool relativePos)
        {
            AlignmentX currentalignment = _alignmentX;
            SetAlignment(AlignmentX.Center);
            _sizeX = GetRelativeX(width, relativePos);
            SetAlignment(currentalignment);
        }

        public void SetHeight(float height)
        {
            _yPos -= _sizeY / 2;
            _sizeY = height;
            _yPos += height / 2;
        }

        public override void Draw()
        {
            ModAPI.Graphics.DrawRect(_xPos, _yPos, _sizeX, _sizeY, _color.R, _color.G, _color.B, _color.A);
        }
    }
}
