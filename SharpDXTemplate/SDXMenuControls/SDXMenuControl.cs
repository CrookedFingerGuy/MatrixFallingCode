using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace MatrixFallingCode
{
    public class SDXMenuControl
    {
        protected int xLocation;
        protected int yLocation;
        protected int width;
        protected int height;
        public int value;
        public bool isActive;
        public bool isSelectable;

        public SDXMenuControl(int x, int y, int w, int h)
        {
            xLocation = x;
            yLocation = y;
            width = w;
            height = h;
        }

        public virtual void DrawControl(RenderTarget D2DRT, TextFormat tFormat)
        {
        }
    }
}
