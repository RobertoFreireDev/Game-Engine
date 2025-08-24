using blackbox.Utils;
using Microsoft.Xna.Framework;

namespace blackbox.Assets;

public static class MapGrid
{
    public static int[,] Data;
    public static int Columns;
    public static int Rows;
    public static int Size;
    public static int Total;

    public static void Create(int columns, int rows, int size)
    {
        Columns = columns;
        Rows = rows;
        Size = size;
        Total = Columns * Rows;
        Data = new int[Rows, Columns];
    }

    public static void SetMap(string grid)
    {
        Data = ArrayUtils.StringToIntArray(grid);
    }

    public static string GetMap()
    {
        return ArrayUtils.IntArrayToString(Data);
    }

    public static void SetTile(int x, int y, int tileIndex)
    {
        if (InvalidGridPos(x, y))
        {
            return;
        }
        Data[y, x] = tileIndex;
    }

    public static void DrawMap(
        int mapX, int mapY,   // starting tile in map
        int px, int py,       // screen position to draw at
        int width, int height, // how many tiles wide/tall to draw
        Color color)
    {
        for (int y = 0; y < height; y++)
        {
            int mapYIndex = mapY + y;
            if (mapYIndex < 0 || mapYIndex >= Rows) continue;

            for (int x = 0; x < width; x++)
            {
                int mapXIndex = mapX + x;
                if (mapXIndex < 0 || mapXIndex >= Columns) continue;

                int tileIndex = Data[mapYIndex, mapXIndex];
                if (tileIndex <= 0) continue;

                Rectangle source = GameGrid.TileRects[tileIndex];
                Rectangle dest = new Rectangle(
                    px + x * Size,
                    py + y * Size,
                    Size,
                    Size);

                GFW.SpriteBatch.Draw(GameGrid.Texture, dest, source, color);
            }
        }
    }

    public static bool InvalidGridPos(int x, int y)
    {
        return x < 0 || y < 0 || x >= Columns || y >= Rows;
    }
}
