using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.DirectInput;
using SharpDX.XInput;
using SharpDX.Direct3D9;

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
