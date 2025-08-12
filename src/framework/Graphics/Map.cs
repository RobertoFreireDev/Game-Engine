using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Graphics;

public static class Map
{
    public static byte[,] SpritesAsSingleRectangle = new byte[64*10,4*10]; // 64x4 = 256 sprites
    public static int Columns = 64;
    public static int Rows = 4;
    public static int TileWidth = 10; // 10x10 pixels each minimum sprite
    public static int TileHeight = 10;
    public static int Total = 256;
    public static Texture2D[] Sprites = new Texture2D[Total];
    public static byte[] Flags = new byte[Total];
    // Data contains the index of the Sprite/Flag. 1 byte => 256 values
    public static byte[,] Data = new byte[Constants.ResolutionX*16,Constants.ResolutionY*8];

    public static bool FGet(int tileIndex, int flag = -1)
    {
        if (tileIndex < 0 || tileIndex >= Flags.Length)
            return false;

        if (flag == -1)
        {
            // Return "true" if any flag bit is set
            return Flags[tileIndex] != 0;
        }
        else
        {
            if (flag < 0 || flag > 7) return false;
            return (Flags[tileIndex] & (1 << flag)) != 0;
        }
    }

    public static byte FGetByte(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= Flags.Length)
            return 0;

        return Flags[tileIndex];
    }

    public static void FSet(int tileIndex, int flag, bool value)
    {
        if (tileIndex < 0 || tileIndex >= Flags.Length || flag < 0 || flag > 7)
            return;

        if (value)
            Flags[tileIndex] |= (byte)(1 << flag); // Set bit
        else
            Flags[tileIndex] &= (byte)~(1 << flag); // Clear bit
    }

    public static void FSetByte(int tileIndex, byte value)
    {
        if (tileIndex < 0 || tileIndex >= Flags.Length)
            return;

        Flags[tileIndex] = value;
    }

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
                    new Rectangle(
                        sx + x * tileSize, 
                        sy + y * tileSize,
                        tileSize,
                        tileSize), 
                    color);
            }
        }
    }
}
