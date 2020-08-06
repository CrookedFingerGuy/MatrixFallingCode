using System;
using System.Collections.Generic;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.XInput;

namespace MatrixFallingCode
{
    public class FallingAnimState
    {
        public bool screenPaused;
        public bool isSettingMenuVisible;
        public float redValue;
        public float greenValue;
        public float blueValue;
        public int fontSize;
        public int minDropLength;
        public int maxDropLength;
        int updateSpeed;
        int speedCounter;

        public int numberOfDrops;
        List<DropLine> DropLines;

        public FallingAnimState(Random rng)
        {
            screenPaused = false;
            isSettingMenuVisible = false;
            redValue=0.0f;
            greenValue = 1.0f;
            blueValue = 0.0f;
            speedCounter = 0;
            updateSpeed = 10;
            fontSize = 36;
            minDropLength = 4;
            maxDropLength = 16;

            numberOfDrops = 1000;
            DropLines = new List<DropLine>();
            for (int i = 0; i < numberOfDrops; i++)
            {
                DropLines.Add(new DropLine(rng,minDropLength,maxDropLength));
            }
            numberOfDrops = 250;
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
            //Act one time on a button press
            if (controlerState.PacketNumber != oldPacketNumber)
            {
                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Back)
                {
                    screenPaused = !screenPaused;
                }

                if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Start)
                {
                    isSettingMenuVisible = !isSettingMenuVisible;
                    sMenu.LoadCurrentStateIntoMenu(this);
                    sMenu.isVisible = isSettingMenuVisible;
                }
            }

            //Press and hold will repeat the action every rLoop()
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.RightShoulder)
            {
                if (redValue + 0.1f < 1.0f)
                    redValue += 0.1f;
                else
                    redValue = 1.0f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.LeftShoulder)
            {
                if (redValue - 0.1f > 0.0f)
                    redValue -= 0.1f;
                else
                    redValue = 0.0f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.X)
            {
                if (greenValue + 0.1f < 1.0f)
                    greenValue += 0.1f;
                else
                    greenValue = 1.0f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.Y)
            {
                if (greenValue - 0.1f > 0.0f)
                    greenValue -= 0.1f;
                else
                    greenValue = 0.0f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
            {
                if (blueValue + 0.1f < 1.0f)
                    blueValue += 0.1f;
                else
                    blueValue = 1.0f;
            }
            if (controlerState.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
            {
                if (blueValue - 0.1f > 0.0f)
                    blueValue -= 0.1f;
                else
                    blueValue = 0.0f;
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
                            DropLines[i] = new DropLine(rng,minDropLength,maxDropLength);
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
