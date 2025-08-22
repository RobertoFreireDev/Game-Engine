using blackbox.Utils;
using Microsoft.Xna.Framework;

namespace blackbox.Assets;

public static class MapGrid
{
    public static int[,] Data;
    public static Rectangle[] TileRects;
    public static int Columns = Constants.MapGridWidth;
    public static int Rows = Constants.MapGridHeight;
    public static int Size = Constants.MapGridSize;
    public static int Total = Columns * Rows;

    public static void Create()
    {
        Data = new int[Rows, Columns];
        TileRects = new Rectangle[Total];

        for (int i = 0; i < Total; i++)
        {
            int x = (i % Constants.GameGridWidth) * Size;
            int y = (i / Constants.GameGridWidth) * Size;
            TileRects[i] = new Rectangle(x, y, Size, Size);
        }
    }

    public static void SetTile(int x, int y, int tileIndex)
    {
        if (InvalidGridPos(x, y))
        {
            return;
        }
        Data[y, x] = tileIndex;
    }

    public static void Draw(Color color)
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                int tileIndex = Data[y, x];
                if (tileIndex <= 0) continue;

                Rectangle source = TileRects[tileIndex];
                Rectangle dest = new Rectangle(x * Size, y * Size, Size, Size);

                GFW.SpriteBatch.Draw(GameGrid.Texture, dest, source, color);
            }
        }
    }

    public static bool InvalidGridPos(int x, int y)
    {
        return x < 0 || y < 0 || x >= Columns * Size || y >= Rows * Size;
    }
}
