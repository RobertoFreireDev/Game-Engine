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

    public static void Draw(int px, int py, Color color)
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                int tileIndex = Data[y, x];
                if (tileIndex <= 0) continue;

                Rectangle source = GameGrid.TileRects[tileIndex];
                Rectangle dest = new Rectangle(px + x * Size, py + y * Size, Size, Size);

                GFW.SpriteBatch.Draw(GameGrid.Texture, dest, source, color);
            }
        }
    }

    public static bool InvalidGridPos(int x, int y)
    {
        return x < 0 || y < 0 || x >= Columns || y >= Rows;
    }
}
