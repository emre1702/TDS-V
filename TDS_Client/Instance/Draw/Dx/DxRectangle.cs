using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxRectangle : Dx
    {
        private float xpos, 
            ypos, 
            sizex, 
            sizey;
        private readonly Color color;
        private UIResText.Alignment alignmentX;

        public DxRectangle(float x, float y, float width, float height, Color color, UIResText.Alignment alignmentX = UIResText.Alignment.Left, EAlignmentY alignmentY = EAlignmentY.Top, bool relativePos = true) : base()
        {
            xpos = GetRelativeX(x, relativePos);
            ypos = GetRelativeY(y, relativePos);
            sizex = GetRelativeX(width, relativePos);
            sizey = GetRelativeY(height, relativePos);

            this.color = color;
            this.alignmentX = alignmentX;

            if (alignmentX == UIResText.Alignment.Left)
                xpos += sizex / 2;
            else if (alignmentX == UIResText.Alignment.Right)
                xpos -= sizex / 2;

            if (alignmentY == EAlignmentY.Top)
                ypos += sizey / 2;
            else if (alignmentY == EAlignmentY.Bottom)
                ypos -= sizey / 2;
        }

        public void SetAlignment(UIResText.Alignment newalignmentX)
        {
            // convert old back
            if (alignmentX == UIResText.Alignment.Left)
                xpos -= sizex / 2;
            else if (alignmentX == UIResText.Alignment.Right)
                xpos += sizex / 2;

            // align new
            if (newalignmentX == UIResText.Alignment.Left)
                xpos += sizex / 2;
            else if (newalignmentX == UIResText.Alignment.Right)
                xpos -= sizex / 2;

            alignmentX = newalignmentX;
        }

        public void SetWidth(float width, bool relativePos)
        {
            UIResText.Alignment currentalignment = alignmentX;
            SetAlignment(UIResText.Alignment.Centered);
            sizex = GetRelativeX(width, relativePos);
            SetAlignment(currentalignment);
        }

        public void SetHeight(float height)
        {
            ypos -= sizey / 2;
            sizey = height;
            ypos += height / 2;
        }

        public override void Draw()
        {
            Graphics.DrawRect(xpos, ypos, sizex, sizey, color.R, color.G, color.B, color.A, 0);
        }
    }
}
