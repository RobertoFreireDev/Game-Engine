using framework.Assets;
using framework.Binding;
using framework.Graphics;
using framework.Input;
using framework.IOFile;
using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace framework
{
    public class GFW : Game
    {
        public GraphicsDeviceManager _graphics;
        public static Dictionary<string, Texture2D> SystemTextures = new Dictionary<string, Texture2D>();
        public static Dictionary<char, Texture2D> MediumFontTextures;
        public static Dictionary<int, Texture2D> MouseTextures;
        public static Texture2D PixelTexture;
        public static SpriteBatch SpriteBatch;
        public static string Title;
        public static int FPS;
        public static int BackgroundColor;
        private static bool Updated = false;
        private LuaBinding game;

        public GFW()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = false;
            IsFixedTimeStep = true;
            ColorUtils.SetPalette();
        }

        public static void UpdateFPS(int fps)
        {
            FPS = fps;
            Updated = true;
        }

        public static void UpdateTitle(string title)
        {
            Title = title;
            Updated = true;
        }

        private void OnResize(Object sender, EventArgs e)
        {
            if (sender is not GameWindow)
            {
                return;
            }

            var window = (GameWindow)sender;

            if (window.ClientBounds.Width == _graphics.PreferredBackBufferWidth && window.ClientBounds.Height == _graphics.PreferredBackBufferHeight)
            {
                return;
            }

            ScreenUtils.SetResolution(_graphics, GraphicsDevice, window.ClientBounds.Width, window.ClientBounds.Height);
            Window.Position = new Point(window.ClientBounds.X, window.ClientBounds.Y);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ScreenUtils.SetResolution(_graphics, GraphicsDevice);
            Window.Position = new Point(0, 35);
            SystemTextures = Images.GetAllImages(GraphicsDevice);
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData(new Color[] { Color.White });
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MediumFontTextures = Font.GetCharacterTextures(GraphicsDevice, SystemTextures["medium_font"]);
            MouseTextures = TextureUtils.GetTextures(GraphicsDevice, SystemTextures["mouse"], 10, 32, 32);

            // Load game
            GameImage.GraphicsDevice = GraphicsDevice;
            var script = LuaFileIO.Read("game");
            game = new LuaBinding(script);
            _graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.KeyboardInput.IsAltF4Pressed())
                Exit();

            if (Input.KeyboardInput.IsF2Released())
                ScreenUtils.ToggleFullScreen(_graphics, GraphicsDevice);

            ScreenUtils.UpdateIsFocused(IsActive, _graphics.IsFullScreen);
            InputStateManager.Update();

            game.Update();
            FPSUtils.Update(gameTime);

            if (Updated)
            {
                Window.Title = Title;
                TargetElapsedTime = FPS != 30 ? TimeSpan.FromSeconds(1.0 / 60.0) : TimeSpan.FromSeconds(1.0 / 30.0);
                Updated = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            game.Draw();
            SpriteBatch.DrawMouse();
            Shapes.DrawRectWithHole(GraphicsDevice, ScreenUtils.BaseBox, ColorUtils.GetColor(BackgroundColor));
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
