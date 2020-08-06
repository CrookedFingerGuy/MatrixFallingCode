using System;
using System.IO;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.DirectWrite;

using SharpDX.DirectInput;
using SharpDX.XInput;


using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;

namespace MatrixFallingCode
{
    public class RForm : RenderForm
    {
        int width;
        int height;
        SwapChainDescription desc;
        Device device;
        SwapChain swapChain;
        SharpDX.Direct2D1.Factory d2dFactory;
        SharpDX.DirectWrite.Factory fact;
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
        string path;

        UserInputProcessor userInputProcessor;
        Random rng;
        TextFormat menuTextFormat;
        TextFormat symbolTextFormat;
        FallingAnimState FAState;
        SettingMenu settings;

        Stopwatch gameInputTimer;

        public RForm(string text) : base(text)
        {
            path = Directory.GetCurrentDirectory();

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
            fact = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated);
            factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(this.Handle, WindowAssociationFlags.IgnoreAll);
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);
            surface = backBuffer.QueryInterface<Surface>();
            d2dRenderTarget = new RenderTarget(d2dFactory, surface, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.Green);
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();

            userInputProcessor = new UserInputProcessor();
            rng = new Random();
            if (File.Exists(path + @"\FallingAnimState.sta"))
            {
                FAState = FileUtils.ReadFromXmlFile<FallingAnimState>(path + @"\FallingAnimState.sta");
            }
            else
                FAState = new FallingAnimState(rng);
            menuTextFormat = new TextFormat(fact, "Arial", FontWeight.Regular, FontStyle.Normal, 16);
            symbolTextFormat = new TextFormat(fact, "Matrix Code NFI", FontWeight.Regular, FontStyle.Normal, FAState.fontSize);
            settings = new SettingMenu(d2dRenderTarget, menuTextFormat, width, height, FAState);

            gameInputTimer = new Stopwatch();
            gameInputTimer.Start();
        }

        public void rLoop()
        {
            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(Color.Black);

            symbolTextFormat.Dispose();
            symbolTextFormat = new TextFormat(fact, "Matrix Code NFI", FontWeight.Regular, FontStyle.Normal, FAState.fontSize);
            FAState.DrawFAState(d2dRenderTarget, symbolTextFormat, solidColorBrush);
            
            if (gameInputTimer.ElapsedMilliseconds >= 25)
            {
                userInputProcessor.oldPacketNumber = gamePadState.PacketNumber;
                gamePadState = userInputProcessor.GetGamePadState();
                gameInputTimer.Restart();

                if (FAState.isSettingMenuVisible)
                {
                    settings.HandleGamePadInputs(gamePadState,FAState,userInputProcessor.oldPacketNumber,path);
                }
                else
                {
                    FAState.HandleGamePadInputs(gamePadState, settings,userInputProcessor.oldPacketNumber);
                }
                FAState.UpdateDrops(rng);
            }

            if (FAState.CheckSettingsMenuVisiblity())
                settings.ShowSettingsMenu(d2dRenderTarget);

            d2dRenderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
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
