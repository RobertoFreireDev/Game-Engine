using framework.Assets;
using framework.Binding;
using framework.Graphics;
using framework.Input;
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
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<char, Texture2D> MediumFontTextures;
        public static Texture2D PixelTexture;
        public static SpriteBatch SpriteBatch;
        public static string Title;
        public static int FPS;
        private LuaBinding game;

        public GFW()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = false;
            IsFixedTimeStep = true;

            var script = @"
                function _init()
                    inittitle(""MY GAME"")
                    initfps60()
                    pal(""#000000,#ffffff,#ffffb0,#7e70ca,#a8734a,#e9b287,#772d26,#b66862,#85d4dc,#c5ffff,#a85fb4,#e99df5,#559e4a,#92df87,#42348b,#bdcc71"")
                end

                function _update()
                end

                function _draw()
                  rectfill(0, 0, 320, 180, 2)
                  rect(0, 0, 320, 180, 1)
                  print(""HELLO WORLD"", 0, 0)
                  print(""HELLO WORLD"", 0, 7)
                end
            ";

            game = new LuaBinding(script);


            Window.Title = Title;
            TargetElapsedTime = FPS != 30 ? TimeSpan.FromSeconds(1.0 / 60.0) : TimeSpan.FromSeconds(1.0 / 30.0);
            _graphics.SynchronizeWithVerticalRetrace = false;
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
            Textures = Images.GetAllImages(GraphicsDevice);
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData(new Color[] { Color.White });
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MediumFontTextures = Font.GetCharacterTextures(GraphicsDevice);
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            game.Draw();
            SpriteBatch.DrawMouse(MouseInput.Context_Menu_mouse);
            Shapes.DrawRectWithHole(GraphicsDevice, ScreenUtils.BaseBox, Color.Black);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
