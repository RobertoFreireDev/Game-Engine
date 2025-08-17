using blackbox.Assets;
using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Graphics;

public static class Sprites
{
    public static byte[,] SpritesAsSingleRectangle = new byte[64 * 10, 4 * 10]; // 64x4 = 256 sprites
    public static Texture2D SpritesTexture;
    public static int Columns = 64;
    public static int Rows = 4;
    public static int TileWidth = 10; // 10x10 pixels each minimum sprite
    public static int TileHeight = 10;
    public static int Total = 256;

    public static void DrawSprite(
        int n, int x, int y, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        if (n < 0 || n >= Total)
        {
            return;
        }

        int scale = (int)(ScreenUtils.ScaleX + ScreenUtils.ScaleY) / 2;
        Rectangle source = new Rectangle(
            (n % Columns) * TileWidth,
            (n / Columns) * TileHeight,
            w * TileWidth,
            h * TileHeight);
        Rectangle destination = new Rectangle(x, y, w * TileWidth, h * TileHeight);
        SpriteEffects effects = SpriteEffects.None;
        if (flipX) effects |= SpriteEffects.FlipHorizontally;
        if (flipY) effects |= SpriteEffects.FlipVertically;

        GFW.SpriteBatch.Draw(
            SpritesTexture,
            destination,
            source,
            color,
            0f,
            Vector2.Zero,
            effects,
            0f
        );
    }
}