﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;

namespace MatrixFallingCode
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            RForm rForm = new RForm("SharpDX Template");

            RenderLoop.Run(rForm, () => rForm.rLoop());

            rForm.Dispose();

        }
    }
}
