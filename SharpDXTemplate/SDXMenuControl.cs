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
        int xLocation;
        int yLocation;
        int width;
        int height;
        string label;
        RawColor4 labelColor;
        RawColor4 activeColor;
        public int value;
        TextFormat tFormat;
        RawRectangleF labelRect;
        SolidColorBrush labelSCBrush;
        SolidColorBrush activeSCBrush;
        RawRectangleF valueBox;
        public bool isActive;

        public SDXMenuControl(RenderTarget D2DRT, TextFormat tf,string l, int x, int y, int width, int height,int v)
        {
            label = l;
            xLocation = x;
            yLocation = y;
            tFormat = tf;
            value = v;
            labelRect = new RawRectangleF(x, y, x+width, y+height);
            labelColor = new RawColor4(0f,0f,1f,1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);
            activeColor= new RawColor4(1f, 0f, 0f, 1f);
            activeSCBrush = new SolidColorBrush(D2DRT, activeColor);
            isActive = false;
            valueBox = new RawRectangleF(x + width + 15, y, x + width + 15 + 60, y + 20);
        }

        public void DrawControl(RenderTarget D2DRT, TextFormat tFormat)
        {
            D2DRT.DrawText(label, tFormat, labelRect, labelSCBrush);
            if (isActive)
            {
                D2DRT.DrawRectangle(valueBox, activeSCBrush);
            }
            else
            {
                D2DRT.DrawRectangle(valueBox, labelSCBrush);
            }
            D2DRT.DrawText(value.ToString(), tFormat, valueBox, labelSCBrush);
        }
    }
}
