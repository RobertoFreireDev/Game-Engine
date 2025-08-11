using blackbox;
using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Graphics;

public static class Map
{
    public static Texture2D[] Sprites = new Texture2D[256];
    public static byte[] Flags = new byte[256];
    // Data contains the index of the Sprite/Flag. 1 byte => 256 values
    public static byte[,] Data = new byte[Constants.ResolutionX*16,Constants.ResolutionY*8];

    public static byte GetMapValue(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Data.GetLength(0) || y >= Data.GetLength(1))
            return 0;

        return Data[x, y];
    }

    public static void SetMapValue(int x, int y, byte v)
    {
        if (x < 0 || y < 0 || x >= Data.GetLength(0) || y >= Data.GetLength(1))
            return;

        Data[x, y] = v;
    }

    public static void DrawMap(int tileSize, Color color, int cel_x, int cel_y, int sx, int sy, int cel_w, int cel_h)
    {
        for (int y = 0; y < cel_h; y++)
        {
            for (int x = 0; x < cel_w; x++)
            {
                int mapX = cel_x + x;
                int mapY = cel_y + y;
                byte tileIndex = GetMapValue(mapX, mapY);
                GFW.SpriteBatch.Draw(
                    Sprites[tileIndex],
                    ScreenUtils.ScaleRectangle(new Rectangle(
                        sx + x * tileSize, 
                        sy + y * tileSize,
                        tileSize,
                        tileSize)), 
                    color);
            }
        }
    }
}
