using framework.Input;
using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace framework
{
    public class GFW : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GFW()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = true;
            IsMouseVisible = true;
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

            ScreenUtils.SetResolution(_graphics, GraphicsDevice, window.ClientBounds.Width, window.ClientBounds.Height, window.ClientBounds.X, window.ClientBounds.Y);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            ScreenUtils.SetResolution(_graphics, GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.KeyboardInput.IsAltF4Pressed())
                Exit();

            if (Input.KeyboardInput.IsF2Released())
                ScreenUtils.ToggleFullScreen(_graphics, GraphicsDevice);

            InputStateManager.Update();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
