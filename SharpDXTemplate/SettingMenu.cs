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
        public bool isVisible;
        SolidColorBrush backgroundGolor;
        RawRectangleF settingMenuSize;
        int menuWidth;
        int menuHeight;
        int screenWidth;
        int screenHeight;
        int topLeftX;
        int topLeftY;
        int activeControl;
        List<SDXMenuControl> menuControls;
        //SDXMenuControl numberOfDropsCtl;
        TextFormat settingTextFormat;


        public SettingMenu(RenderTarget D2DRT,TextFormat tf ,int Width, int Height,FallingAnimState faState)
        {
            isVisible = false;
            menuWidth = 500;
            menuHeight = 500;
            screenWidth = Width;
            screenHeight = Height;
            backgroundGolor = new SolidColorBrush(D2DRT, new RawColor4(0.5f, 0.5f, 0.5f, 1.0f));
            topLeftX = screenWidth / 2 - menuWidth / 2;
            topLeftY = screenHeight / 2 - menuHeight / 2;
            settingMenuSize = new RawRectangleF(topLeftX, topLeftY , screenWidth / 2 + menuWidth / 2, screenHeight / 2 + menuHeight / 2);

            menuControls = new List<SDXMenuControl>();

            menuControls.Add(new SDXMenuControl(D2DRT, settingTextFormat, "Number of Active Symbol Drops: ", topLeftX + 10, topLeftY + 10, 250, 36, faState.numberOfDrops));
            menuControls[0].isActive = true;
            menuControls.Add(new SDXMenuControl(D2DRT, settingTextFormat, "Font Size: ", topLeftX + 10, topLeftY + 10 + 36, 250, 36, faState.fontSize));
            menuControls.Add(new SDXMenuControl(D2DRT, settingTextFormat, "Red: ", topLeftX + 10, topLeftY + 10 + 72, 250, 36, (int)(faState.redValue)*255));
            menuControls.Add(new SDXMenuControl(D2DRT, settingTextFormat, "Green: ", topLeftX + 10, topLeftY + 10 + 108, 250, 36, (int)(faState.greenValue)*255));
            menuControls.Add(new SDXMenuControl(D2DRT, settingTextFormat, "Blue: ", topLeftX + 10, topLeftY + 10 + 144, 250, 36, (int)(faState.blueValue)*255));
        }

        public void ShowSettingsMenu(RenderTarget D2DRT, TextFormat soManyTextFormats)
        {
            D2DRT.FillRectangle(settingMenuSize, backgroundGolor);
            foreach(SDXMenuControl s in menuControls)
                s.DrawControl(D2DRT,soManyTextFormats);
        }


        public void HandleGamePadInputs(int gamePadButtonValue, FallingAnimState faState)
        {

            if (gamePadButtonValue == 6)
            {
                isVisible = !isVisible;
                faState.isSettingMenuVisible = isVisible;
            }

            
            if (gamePadButtonValue == -1)
            {
                switch(activeControl)
                {
                    case 0:
                        {
                            if (faState.numberOfDrops > 10)
                            {
                                faState.numberOfDrops -= 1;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            if(faState.fontSize > 8)
                            {
                                faState.fontSize--;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                    case 2:
                        {
                            if (faState.redValue > (0.0f + 1.0f / 255f))
                            {
                                faState.redValue -= 1.0f / 255f;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                    case 3:
                        {
                            if (faState.greenValue > (0.0f + 1.0f / 255f))
                            {
                                faState.greenValue -= 1.0f / 255f;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                    case 4:
                        {
                            if (faState.blueValue > (0.0f + 1.0f / 255f))
                            {
                                faState.blueValue -= 1.0f / 255f;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                }
            }
            if (gamePadButtonValue == 1)
            {
                switch (activeControl)
                {
                    case 0:
                        {
                            if (faState.numberOfDrops < 250)
                            {
                                faState.numberOfDrops += 1;
                                menuControls[activeControl].value += 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            if (faState.fontSize < 48)
                            {
                                faState.fontSize++;
                                menuControls[activeControl].value++;
                            }
                        }
                        break;
                    case 2:
                        {
                            if (faState.redValue < (1.0f - 1.0f / 255f))
                            {
                                faState.redValue += 1.0f / 255f;
                                menuControls[activeControl].value += 1;
                            }
                        }
                        break;
                    case 3:
                        {
                            if (faState.greenValue < (1.0f - 1.0f / 255f))
                            {
                                faState.greenValue += 1.0f / 255f;
                                menuControls[activeControl].value += 1;
                            }
                        }
                        break;
                    case 4:
                        {
                            if (faState.blueValue < (1.0f - 1.0f / 255f))
                            {
                                faState.blueValue += 1.0f / 255f;
                                menuControls[activeControl].value += 1;
                            }
                        }
                        break;
                }
            }

            if (gamePadButtonValue == 3)
            {
                if (activeControl > 0)
                {
                    menuControls[activeControl].isActive = false;
                    activeControl--;
                    menuControls[activeControl].isActive = true;
                }
            }
            if (gamePadButtonValue == -3)
            {
                if (activeControl < menuControls.Count-1)
                {
                    menuControls[activeControl].isActive = false;
                    activeControl++;
                    menuControls[activeControl].isActive = true;
                }
            }

        }
    }
}