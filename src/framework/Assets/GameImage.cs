using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Assets;

public static class GameImage
{
    public static Texture2D[] GameTexture = new Texture2D[256];

    public static int Columns;
    public static int Rows;
    public static int TileWidth;
    public static int TileHeight;
    public static int Total;

    public static void LoadTexture(int index, string spriteBase64, int width, int height)
    {
        GameTexture[index] = TextureUtils.Convert64ToTexture(spriteBase64);
        TileWidth = width;
        TileHeight = height;
        Columns = GameTexture[index].Width / TileWidth;
        Rows = GameTexture[index].Height / TileHeight;
        Total = Columns * Rows;
    }

    public static void DrawCustomSprite(
        int index, int n, int x, int y, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        if (n < 0 || n >= GameImage.Total)
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
            GameTexture[index],
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
