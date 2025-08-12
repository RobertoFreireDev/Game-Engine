using blackbox.Graphics;
using blackbox.Assets;
using blackbox.Binding;
using blackbox.Input;
using blackbox.IOFile;
using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace blackbox
{
    public class GFW : Game
    {
        public GraphicsDeviceManager _graphics;
        public static Dictionary<string, Texture2D> SystemTextures = new Dictionary<string, Texture2D>();
        public static Dictionary<char, Texture2D> MediumFontTextures;
        public static List<Texture2D> MouseTextures;
        public static Texture2D PixelTexture;
        public static SpriteBatch SpriteBatch;
        public static string Title;
        public static int FPS;
        public static bool ShowMouse = true;
        public static bool ApplyCRTshader = false;
        public static int BackgroundColor;
        private static bool Updated = false;
        private LuaBinding game;
        private Effect crtEffect;
        private static float Inner = 0.00f;
        private static float Outer = 0.00f;
        private RenderTarget2D sceneTarget;

        public GFW()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = false;
            IsFixedTimeStep = true;
            ColorUtils.SetPalette();
            Window.Title = "Black Box";
        }

        public static void ShowHideMouse(bool value)
        {
            ShowMouse = value;
        }

        public static void EnableCRTshader(bool value, int inner, int outer)
        {
            ApplyCRTshader = value;
            Inner = Math.Clamp(inner,0,110) * 0.01f;
            Outer = Math.Clamp(outer,0,110) * 0.01f;
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
            GrapUtils.GraphicsDevice = GraphicsDevice;
            Window.Position = new Point(0, 35);
            SystemTextures = Images.GetAllImages();
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData(new Color[] { Color.White });
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MediumFontTextures = Font.GetCharacterTextures(GraphicsDevice, SystemTextures["medium_font"]);
            MouseTextures = TextureUtils.GetTextures(SystemTextures["mouse"], 10, 32, 32);
            // Load game
            var script = LuaFileIO.Read("game");
            game = new LuaBinding(script);
            _graphics.SynchronizeWithVerticalRetrace = true;
            crtEffect = Content.Load<Effect>("CRT");
            sceneTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
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

            if (Updated)
            {
                Window.Title = Title;
                TargetElapsedTime = FPS != 30 ? TimeSpan.FromSeconds(1.0 / 60.0) : TimeSpan.FromSeconds(1.0 / 30.0);
                Updated = false;
            }

            crtEffect.Parameters["Resolution"].SetValue(
                new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth,
                            GraphicsDevice.PresentationParameters.BackBufferHeight));
            crtEffect.Parameters["Inner"].SetValue(Inner);
            crtEffect.Parameters["Outer"].SetValue(Outer);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            TimeUtils.Update(gameTime);
            GraphicsDevice.SetRenderTarget(sceneTarget);
            GraphicsDevice.Clear(ColorUtils.GetColor(BackgroundColor));
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
            game.Draw();
            SpriteBatch.End();

            // STEP 2: Reset back buffer and apply CRT shader
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            // Pass Resolution to the shader (important!)
            crtEffect.Parameters["Resolution"].SetValue(new Vector2(
                sceneTarget.Width,
                sceneTarget.Height
            ));

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
                              null, null, effect: ApplyCRTshader ? crtEffect : null);
            SpriteBatch.Draw(sceneTarget, ScreenUtils.BoxToDraw, Color.White);
            SpriteBatch.End();

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (ShowMouse)
            {
                SpriteBatch.DrawMouse();
            }            
            DrawRectWithHole(ScreenUtils.ScaleRectangle(ScreenUtils.BaseBox), Color.Black);
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawRectWithHole(Rectangle hole, Color color)
        {
            var viewport = GraphicsDevice.Viewport.Bounds;
            SpriteBatch.Draw(PixelTexture, new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Y + hole.Y), color);
            SpriteBatch.Draw(PixelTexture, new Rectangle(viewport.X, hole.Bottom, viewport.Width, viewport.Bottom - hole.Bottom), color);
            SpriteBatch.Draw(PixelTexture, new Rectangle(viewport.X, hole.Y, hole.X - viewport.X, hole.Height), color);
            SpriteBatch.Draw(PixelTexture, new Rectangle(hole.Right, hole.Y, viewport.Right - hole.Right, hole.Height), color);
        }
    }
}
