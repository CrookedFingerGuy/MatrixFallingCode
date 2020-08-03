using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
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
using System.Runtime.InteropServices.WindowsRuntime;

namespace MatrixFallingCode
{
    public class DropLine
    {
        List<char> symbolLine;
        List<RawRectangleF> symbolTextAreas;
        List<float> symbolFadingLevel;
        int dLineLength;
        int kerning;
        int currentSymbol;
        int fadedSymbol;
        int startTimeOffset;
        bool displayFinished;
        float redHue;
        float greenHue;
        float blueHue;


        public DropLine(Random rand)
        {
            symbolLine = new List<char>();
            symbolTextAreas = new List<RawRectangleF>();
            symbolFadingLevel = new List<float>();

            dLineLength = rand.Next(4, 16);
            kerning = 36;
            currentSymbol = 0;
            fadedSymbol = 0;
            displayFinished = false;
            redHue = 0.0f;
            greenHue = 1.0f;
            blueHue = 0.0f;


            startTimeOffset = rand.Next(0, 8);


            int left = rand.Next(0, 1900);
            int top = rand.Next(-100, 800);
            symbolLine.Add(Convert.ToChar(rand.Next(33, 127)));
            symbolTextAreas.Add(new RawRectangleF(left, top, left + kerning, top + kerning));
            symbolFadingLevel.Add(1f);

            for (int i = 1; i < dLineLength; i++)
            {
                symbolLine.Add(Convert.ToChar(rand.Next(33, 127)));
                symbolTextAreas.Add(new RawRectangleF(left, top + kerning * i, left + kerning, top + kerning * i + kerning));
                symbolFadingLevel.Add(1f);
            }
        }

        public void DrawDropLine(RenderTarget d2dRT, TextFormat tf, SolidColorBrush br,float changedRedValue,float changedGreenValue, float changedBlueValue)
        {
            redHue = changedRedValue;
            greenHue = changedGreenValue;
            blueHue = changedBlueValue;

            for (int i = 0; i < currentSymbol; i++)
            {
                if (i == currentSymbol - 1)
                {
                    //this code is what caused the unmanaged memory leak
                    //br = new SolidColorBrush(d2dRT, new RawColor4(1.0f, 1.0f, 1.0f, 1.0f * symbolFadingLevel.ElementAt(i)));
                    br.Color = new RawColor4(1.0f, 1.0f, 1.0f, 1.0f * symbolFadingLevel.ElementAt(i));
                }
                else
                {
                    //this code is what caused the unmanaged memory leak
                    //br = new SolidColorBrush(d2dRT, new RawColor4(0.0f, 1.0f, 0.0f, 1.0f * symbolFadingLevel.ElementAt(i)));
                    br.Color = new RawColor4(redHue, greenHue, blueHue, 1.0f * symbolFadingLevel.ElementAt(i));
                }

                d2dRT.DrawText(symbolLine.ElementAt(i).ToString(), tf, symbolTextAreas.ElementAt(i), br);
            }
        }

        public bool SymbolIterator(Random rand)
        {
            if (currentSymbol == 0 && startTimeOffset > 0)
                startTimeOffset--;
            else if (currentSymbol < dLineLength)
            {
                currentSymbol++;
                for (int i = 0; i < currentSymbol; i++)
                {
                    symbolFadingLevel[i] *= (float)rand.NextDouble(0.5, 1.25f);
                }
            }
            else
            {
                return SymbolFader();
            }
            return false;
        }

        public bool SymbolFader()
        {
            if (fadedSymbol < dLineLength)
            {
                symbolFadingLevel[fadedSymbol] = 0.0f;
                fadedSymbol++;
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
