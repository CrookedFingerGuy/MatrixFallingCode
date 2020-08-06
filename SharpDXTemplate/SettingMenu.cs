using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;

namespace MatrixFallingCode
{
    public class SettingMenu
    {
        int screenWidth;
        int screenHeight;
        int menuWidth;
        int menuHeight;
        int topLeftX;
        int topLeftY;
        public bool isVisible;
        SolidColorBrush backgroundGolor;
        RawRectangleF settingMenuSize;
        List<SDXMenuControl> menuControls;
        int activeControl;
        TextFormat settingsTextFormat;

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
            settingsTextFormat = tf;


            int controlYSpacing = (int)settingsTextFormat.FontSize+20;
            int controlLeftMargin = 10;
            int controlTopMargin = 60;
            menuControls = new List<SDXMenuControl>();
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Number of Active Symbol Drops: ",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing*menuControls.Count + controlTopMargin, 250, controlYSpacing, faState.numberOfDrops));
            menuControls[0].isActive = true;
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Font Size: ",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing * menuControls.Count + controlTopMargin, 250, controlYSpacing, faState.fontSize));
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Red: ",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing * menuControls.Count + controlTopMargin, 250, controlYSpacing, (int)(faState.redValue*255)));
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Green: ",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing * menuControls.Count + controlTopMargin, 250, controlYSpacing, (int)(faState.greenValue*255)));
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Blue: ",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing * menuControls.Count + controlTopMargin, 250, controlYSpacing, (int)(faState.blueValue*255)));
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Minimun Line Length",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing * menuControls.Count + controlTopMargin, 250, controlYSpacing, faState.minDropLength));
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, settingsTextFormat, "Maximun Line Length",
                topLeftX + controlLeftMargin, topLeftY + controlYSpacing * menuControls.Count + controlTopMargin, 250, controlYSpacing, faState.maxDropLength));

            menuControls.Add(new SDXMenuButton(D2DRT, settingsTextFormat, "Reset to Defaults", screenWidth / 2 - 125, topLeftY + 325, 250, 25));
            menuControls.Add(new SDXMenuButton(D2DRT, settingsTextFormat, "Exit", screenWidth / 2 - 25, topLeftY + 360, 50, 25));
            menuControls.Add(new SDXMenuButton(D2DRT, settingsTextFormat, "Close Menu", screenWidth / 2 - 50, topLeftY + 395, 100, 25));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "Settings Menu", topLeftX + 200, topLeftY + 10, 200, 100));
            menuControls.Add(new SDXMenuImage(D2DRT, "DPadLeftRight.png", topLeftX + 25, screenHeight / 2 + menuHeight / 2 - 75, 64, 64));
        }

        public void ShowSettingsMenu(RenderTarget D2DRT)
        {
            D2DRT.FillRectangle(settingMenuSize, backgroundGolor);
            foreach(SDXMenuControl s in menuControls)
                s.DrawControl(D2DRT, settingsTextFormat);
        }

        public void LoadCurrentStateIntoMenu(FallingAnimState faState)
        {
            menuControls[0].value = faState.numberOfDrops;
            menuControls[1].value = faState.fontSize;
            menuControls[2].value = (int)(faState.redValue*255);
            menuControls[3].value = (int)(faState.greenValue*255);
            menuControls[4].value = (int)(faState.blueValue*255);
            menuControls[5].value = faState.minDropLength;
            menuControls[6].value = faState.maxDropLength;
        }

        public void HandleGamePadInputs(State controlerState, FallingAnimState faState,int oldPacketNumber)
        {
            if (controlerState.PacketNumber != oldPacketNumber)
            {
                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Start)
                {
                    isVisible = !isVisible;
                    faState.isSettingMenuVisible = isVisible;
                }

                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
                {
                    if (activeControl > 0)
                    {
                        menuControls[activeControl].isActive = false;
                        activeControl--;
                        menuControls[activeControl].isActive = true;
                    }
                }
                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
                {
                    if (activeControl < menuControls.Count - 1)
                    {
                        menuControls[activeControl].isActive = false;
                        activeControl++;
                        menuControls[activeControl].isActive = true;
                    }
                }
            }


            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
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
                            if (faState.redValue > (1.0f / 256f))
                            {
                                faState.redValue -= 1.0f / 256f;
                                menuControls[activeControl].value -= 1;
                            }
                            else
                            {
                                faState.redValue = 0.0f;
                                menuControls[activeControl].value = 0;
                            }
                        }
                        break;
                    case 3:
                        {
                            if (faState.greenValue > (1.0f / 256f))
                            {
                                faState.greenValue -= 1.0f / 256f;
                                menuControls[activeControl].value -= 1;
                            }
                            else
                            {
                                faState.greenValue = 0.0f;
                                menuControls[activeControl].value = 0;
                            }
                        }
                        break;
                    case 4:
                        {
                            if (faState.blueValue > (1.0f / 256f))
                            {
                                faState.blueValue -= 1.0f / 256f;
                                menuControls[activeControl].value -= 1;
                            }
                            else
                            {
                                faState.blueValue = 0.0f;
                                menuControls[activeControl].value = 0;
                            }
                        }
                        break;
                    case 5:
                        {
                            if (faState.minDropLength > 0)
                            {
                                faState.minDropLength -= 1;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                    case 6:
                        {
                            if (faState.maxDropLength > faState.minDropLength)
                            {
                                faState.maxDropLength -= 1;
                                menuControls[activeControl].value -= 1;
                            }
                        }
                        break;
                }
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
            {
                switch (activeControl)
                {
                    case 0:
                        {
                            if (faState.numberOfDrops < 1000)
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
                            if (faState.redValue < (1.0f - 1.0f / 256f))
                            {
                                faState.redValue += 1.0f / 256f;
                                menuControls[activeControl].value += 1;
                            }
                            else
                            {
                                faState.redValue = 1.0f;
                                menuControls[activeControl].value = 255;
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
                            else
                            {
                                faState.greenValue = 1.0f;
                                menuControls[activeControl].value = 255;
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
                            else
                            {
                                faState.blueValue = 1.0f;
                                menuControls[activeControl].value = 255;
                            }
                        }
                        break;
                    case 5:
                        {
                            if (faState.minDropLength < 50 && faState.minDropLength < faState.maxDropLength)
                            {
                                faState.minDropLength += 1;
                                menuControls[activeControl].value += 1;
                            }
                        }
                        break;
                    case 6:
                        {
                            if (faState.maxDropLength < 50)
                            {
                                faState.maxDropLength += 1;
                                menuControls[activeControl].value += 1;
                            }
                        }
                        break;
                }
            }

            if(controlerState.Gamepad.Buttons==GamepadButtonFlags.A)
            {
                switch(activeControl)
                {
                    case 7:
                        {
                            FADefaults fad;
                            fad = new FADefaults();
                            isVisible = false;
                            faState.screenPaused = fad.screenPaused;
                            faState.isSettingMenuVisible = fad.isSettingMenuVisible;
                            faState.redValue = fad.redValue;
                            faState.greenValue = fad.greenValue;
                            faState.blueValue = fad.blueValue;
                            faState.fontSize = fad.fontSize;
                            faState.numberOfDrops = fad.numberOfDrops;
                            faState.minDropLength = fad.minDropLength;
                            faState.maxDropLength = fad.maxDropLength;
                        }
                        break;
                    case 8:
                        {
                            System.Windows.Forms.Application.Exit();
                        }break;
                    case 9:
                        {
                            isVisible = !isVisible;
                            faState.isSettingMenuVisible = isVisible;
                        }
                        break;
                }
            }
        }
    }
}