using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;

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
        float redHue;
        float greenHue;
        float blueHue;


        public DropLine(Random rand,int min,int max)
        {
            symbolLine = new List<char>();
            symbolTextAreas = new List<RawRectangleF>();
            symbolFadingLevel = new List<float>();
            kerning = 36;
            currentSymbol = 0;
            fadedSymbol = 0;
            redHue = 0.0f;
            greenHue = 1.0f;
            blueHue = 0.0f;

            int left = rand.Next(0, 1900);
            int top = rand.Next(-100, 800);

            dLineLength = rand.Next(min, max);
            startTimeOffset = rand.Next(0, 8);
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
                    br.Color = new RawColor4(1.0f, 1.0f, 1.0f, 1.0f * symbolFadingLevel.ElementAt(i));
                }
                else
                {
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
