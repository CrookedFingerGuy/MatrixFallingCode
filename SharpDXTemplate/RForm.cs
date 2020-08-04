using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
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


using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;

namespace MatrixFallingCode
{
    public class RForm : RenderForm
    {
        SwapChainDescription desc;
        Device device;
        SwapChain swapChain;
        SharpDX.Direct2D1.Factory d2dFactory;
        Factory factory;
        Texture2D backBuffer;
        RenderTargetView renderView;
        Surface surface;
        RenderTarget d2dRenderTarget;
        SolidColorBrush solidColorBrush;
        DirectInput directInput;
        Keyboard keyboard;
        KeyboardUpdate[] keyData;
        State gamePadState;
        Random rng;
        int width;
        int height;

        UserInputProcessor userInputProcessor;
        Stopwatch gameInputTimer;
        TextFormat TestTextFormat;
        RawRectangleF TestTextArea;

        FallingAnimState FAState;
        SettingMenu settings;
        TextFormat menuTextFormat;


        //DropLine testDropLine;
        public RForm(string text) : base(text)
        {
            width = 1920;
            height = 1080;

            this.ClientSize = new System.Drawing.Size(width, height);

            desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(this.ClientSize.Width, this.ClientSize.Height, new Rational(144, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = this.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new SharpDX.Direct3D.FeatureLevel[] { SharpDX.Direct3D.FeatureLevel.Level_10_0 }, desc, out device, out swapChain);
            d2dFactory = new SharpDX.Direct2D1.Factory();
            factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(this.Handle, WindowAssociationFlags.IgnoreAll);
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);
            surface = backBuffer.QueryInterface<Surface>();
            d2dRenderTarget = new RenderTarget(d2dFactory, surface, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.White);
            solidColorBrush.Color = Color.Green;
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();
            userInputProcessor = new UserInputProcessor();
            menuTextFormat=new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Arial", FontWeight.Regular, FontStyle.Normal, 16);
            TestTextArea = new SharpDX.Mathematics.Interop.RawRectangleF(10, 10, 400, 400);
            gameInputTimer = new Stopwatch();
            gameInputTimer.Start();
            rng = new Random();

            FAState = new FallingAnimState(rng);
            settings = new SettingMenu(d2dRenderTarget,menuTextFormat, width, height,FAState);

            TestTextFormat = new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Matrix Code NFI", FontWeight.Regular, FontStyle.Normal, FAState.fontSize);
            
        }

        public void rLoop()
        {
            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(Color.Black);
            //d2dRenderTarget.DrawBitmap(Background, 1.0f, BitmapInterpolationMode.Linear);
            //d2dRenderTarget.DrawText("Test", TestTextFormat, TestTextArea, solidColorBrush);
            //userInputProcessor.DisplayGamePadState(d2dRenderTarget, solidColorBrush);

            FAState.DrawFAState(d2dRenderTarget, TestTextFormat, solidColorBrush);
            

            if (gameInputTimer.ElapsedMilliseconds >= 25)
            {
                userInputProcessor.oldPacketNumber = gamePadState.PacketNumber;
                gamePadState = userInputProcessor.GetGamePadState();
                gameInputTimer.Restart();

                if (FAState.isSettingMenuVisible)
                {
                    settings.HandleGamePadInputs(userInputProcessor.CheckGamePadButtons(),FAState);
                }
                else
                {
                    FAState.HandleGamePadInputs(userInputProcessor.CheckGamePadButtons(),settings);
                }
                FAState.UpdateDrops(rng);
            }

            if (FAState.CheckSettingsMenuVisiblity())
                settings.ShowSettingsMenu(d2dRenderTarget,menuTextFormat);

            d2dRenderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
            //Thread.Sleep(100);
        }

        Bitmap LoadBackground(RenderTarget renderTarget, string file)
        {
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }

        ~RForm()
        {
            keyboard.Unacquire();
            keyboard.Dispose();
            directInput.Dispose();
            renderView.Dispose();
            backBuffer.Dispose();
            device.ImmediateContext.ClearState();
            device.ImmediateContext.Flush();
            device.Dispose();
            device.Dispose();
            swapChain.Dispose();
            factory.Dispose();
        }

    }

}
