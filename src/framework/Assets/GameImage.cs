using blackbox.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Assets;

public static class GameImage
{
    public static Texture2D GameTexture;

    public static int Columns;
    public static int Rows;
    public static int TileWidth;
    public static int TileHeight;
    public static int Total;

    public static void LoadTexture(string spriteBase64, int width, int height)
    {
        GameTexture = TextureUtils.Convert64ToTexture(spriteBase64);
        TileWidth = width;
        TileHeight = height;
        Columns = GameTexture.Width / TileWidth;
        Rows = GameTexture.Height / TileHeight;
        Total = Columns * Rows;
    }
}
