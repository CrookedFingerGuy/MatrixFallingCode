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
    public class SDXMenuButton:SDXMenuControl
    {
        string label;
        RawColor4 labelColor;
        RawColor4 activeColor;
        SolidColorBrush labelSCBrush;
        SolidColorBrush activeSCBrush;
        RawRectangleF labelRect;
        TextFormat tFormat;

        public SDXMenuButton(RenderTarget D2DRT, TextFormat tf, string l, int x, int y, int width, int height) :base(x,y,width,height)
        {
            label = l;
            tFormat = tf;
            labelRect = new RawRectangleF(x, y, x + width, y + height);
            labelColor = new RawColor4(0f, 0f, 1f, 1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);
            activeColor = new RawColor4(1f, 0f, 0f, 1f);
            activeSCBrush = new SolidColorBrush(D2DRT, activeColor);
            isSelectable = true;
        }

        public override void DrawControl(RenderTarget D2DRT, TextFormat tFormat)
        {
            tFormat.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            D2DRT.DrawText(label, tFormat, labelRect, labelSCBrush);
            if (isActive)
            {
                D2DRT.DrawRectangle(labelRect, activeSCBrush);
            }
            else
            {
                D2DRT.DrawRectangle(labelRect, labelSCBrush);
            }            
        }
    }
}
