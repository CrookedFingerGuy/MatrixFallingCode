using System;
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
