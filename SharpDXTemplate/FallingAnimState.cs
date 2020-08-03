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
        int numberOfDrops;
        bool screenPaused;
        float redValue;
        float greenValue;
        float blueValue;

        int updateSpeed;
        int speedCounter;


        public FallingAnimState(Random rng)
        {
            screenPaused = false;
            redValue=0.0f;
            greenValue = 1.0f;
            blueValue = 0.0f;
            numberOfDrops = 250;
            DropLines = new List<DropLine>();
            speedCounter = 0;
            updateSpeed = 10;

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

        public void HandleGamePadInputs(int gamePadButtonValue)
        {
            if (gamePadButtonValue == 2)
            {
                screenPaused = !screenPaused;
            }


            if (gamePadButtonValue == 4)
            {
                if (redValue < 1.0f)
                    redValue += 0.1f;
            }
            if (gamePadButtonValue == -4)
            {
                if (redValue > 0.0f)
                    redValue -= 0.1f;
            }

            if (gamePadButtonValue == 5)
            {
                if (greenValue < 1.0f)
                    greenValue += 0.1f;
            }
            if (gamePadButtonValue == -5)
            {
                if (greenValue > 0.0f)
                    greenValue -= 0.1f;
            }


            if (gamePadButtonValue == 1)
            {
                if (blueValue < 1.0f)
                    blueValue += 0.1f;
            }
            if (gamePadButtonValue == -1)
            {
                if (blueValue > 0.0f)
                    blueValue -= 0.1f;
            }

            if(gamePadButtonValue == 3)
            {
                if (updateSpeed < 40)
                    updateSpeed++;                
            }

            if(gamePadButtonValue == -3)
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

    }
}
