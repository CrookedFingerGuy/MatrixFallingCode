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

namespace MatrixFallingCode
{
    public class SettingMenu
    {
        bool isVisible;
        SolidColorBrush backgroundGolor;
        RawRectangleF settingMenuSize;
        int menuWidth;
        int menuHeight;
        int screenWidth;
        int screenHeight;
        Geometry menuBackgroung;
        

        public SettingMenu(RenderTarget D2DRT, int Width, int Height)
        {
            isVisible = false;
            menuWidth = 500;
            menuHeight = 500;
            screenWidth = Width;
            screenHeight = Height;
            backgroundGolor = new SolidColorBrush(D2DRT, new RawColor4(0.5f,0.5f,0.5f,1.0f));
            settingMenuSize = new RawRectangleF(screenWidth/2-menuWidth/2,screenHeight/2-menuHeight/2, screenWidth / 2 + menuWidth / 2, screenHeight / 2 + menuHeight / 2);
        }

        public void ShowSettingsMenu(RenderTarget D2DRT)
        {
            D2DRT.FillRectangle(settingMenuSize, backgroundGolor);
        }
    }
}
