using RAGE.Game;
using System.Drawing;

namespace TDS_Client.Instance.Draw.Dx
{
    class DxRectangle : Dx
    {
        private float xpos, 
            ypos, 
            sizex, 
            sizey;
        private Color color;
        private Alignment alignment;
        private bool relativePos;

        public DxRectangle(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.Left, bool relativePos = true) : base()
        {
            xpos = GetRelativeX(x, relativePos);
            ypos = GetRelativeY(y, relativePos);
            sizex = GetRelativeX(width, relativePos);
            sizey = GetRelativeY(height, relativePos);

            this.color = color;
            this.alignment = alignment;
            this.relativePos = relativePos;

            if (alignment == Alignment.Left)
                xpos += sizex / 2;
            else if (alignment == Alignment.Right)
                xpos -= sizex / 2;
            ypos += sizey / 2;
        }

        public void SetAlignment(Alignment newalignment)
        {
            // convert old back
            if (alignment == Alignment.Left)
                xpos -= sizex / 2;
            else if (alignment == Alignment.Right)
                xpos += sizex / 2;

            // align new
            if (newalignment == Alignment.Left)
                xpos += sizex / 2;
            else if (newalignment == Alignment.Right)
                xpos -= sizex / 2;

            alignment = newalignment;
        }

        public void SetWidth(float width)
        {
            Alignment currentalignment = alignment;
            SetAlignment(Alignment.Center);
            sizex = width;
            SetAlignment(currentalignment);
        }

        public void SetHeight(float height)
        {
            ypos -= sizey / 2;
            sizey = height;
            ypos += height / 2;
        }

        protected override void Draw(int tick)
        {
            Graphics.DrawRect(xpos, ypos, sizex, sizey, color.R, color.G, color.B, color.A, 0);
        }
    }
}
