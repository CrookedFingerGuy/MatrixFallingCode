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
    public class FallingAnimState
    {
        List<DropLine> DropLines;
        public int numberOfDrops;
        public bool screenPaused;
        public bool isSettingMenuVisible;
        public float redValue;
        public float greenValue;
        public float blueValue;

        public int fontSize;
        int updateSpeed;
        int speedCounter;


        public FallingAnimState(Random rng)
        {
            screenPaused = false;
            isSettingMenuVisible = false;
            redValue=0.0f;
            greenValue = 1.0f;
            blueValue = 0.0f;
            numberOfDrops = 250;
            DropLines = new List<DropLine>();
            speedCounter = 0;
            updateSpeed = 10;
            fontSize = 36;
            for (int i = 0; i < numberOfDrops; i++)
            {
                DropLines.Add(new DropLine(rng));
            }
        }

        public void DrawFAState(RenderTarget D2DRT,TextFormat tf,SolidColorBrush br)
        {
            for (int i = 0; i < numberOfDrops; i++)
            {
                DropLines[i].DrawDropLine(D2DRT, tf, br, redValue, greenValue, blueValue);
            }
        }

        public void HandleGamePadInputs(State controlerState, SettingMenu sMenu, int oldPacketNumber)
        {

            if (controlerState.PacketNumber != oldPacketNumber)
            {
                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Back)
                {
                    screenPaused = !screenPaused;
                }

                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Start)
                {
                    isSettingMenuVisible = !isSettingMenuVisible;
                    sMenu.isVisible = isSettingMenuVisible;
                }
            }

            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.A)
            {
                if (redValue < 1.0f)
                    redValue += 0.1f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.B)
            {
                if (redValue > 0.0f)
                    redValue -= 0.1f;
            }

            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.X)
            {
                if (greenValue < 1.0f)
                    greenValue += 0.1f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Y)
            {
                if (greenValue > 0.0f)
                    greenValue -= 0.1f;
            }


            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
            {
                if (blueValue < 1.0f)
                    blueValue += 0.1f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
            {
                if (blueValue > 0.0f)
                    blueValue -= 0.1f;
            }

            if(controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
            {
                if (updateSpeed < 40)
                    updateSpeed++;                
            }

            if(controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
            {
                if (updateSpeed > 0)
                    updateSpeed--;
            }
        }

        public void UpdateDrops(Random rng)
        {
            if (speedCounter >= updateSpeed)
            {
                if (!screenPaused)
                {
                    for (int i = 0; i < numberOfDrops; i++)
                    {
                        if (DropLines[i].SymbolIterator(rng))
                        {
                            DropLines[i] = new DropLine(rng);
                        }
                    }
                    speedCounter = 0;
                }
            }
            else
            {
                speedCounter++;
            }
        }

        public bool CheckSettingsMenuVisiblity()
        {
            return isSettingMenuVisible;
        }
    }
}
