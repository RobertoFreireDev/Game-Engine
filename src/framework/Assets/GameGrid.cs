using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace blackbox.Assets;

public class GameGridData
{
    public int[,] GameGrid;
    public int Columns;
    public int Rows;
    public int Size;
    public int Total;

    public GameGridData()
    {
        Size = Constants.GameGridSize;
        Columns = Constants.GameGridWidth;
        Rows = Constants.GameGridHeight;
        Total = Columns * Rows;
        GameGrid = new int[Columns * Size, Rows * Size];
    }
}

public static class GameGrid
{
    public static GameGridData[] GameGridData = new GameGridData[Constants.MaxGameGrids];


    public static void CreateGrid(int index)
    {
        if (index < 0 || index >= Constants.MaxGameGrids)
        {
            return;
        }

        GameGridData[index] = new GameGridData();
        ClearGrid(index,0,0, GameGridData[index].Size, GameGridData[index].Size);
    }

    public static void ClearGrid(int index, int x, int y, int w, int h)
    {
        if (index < 0 || index >= Constants.MaxGameGrids)
        {
            return;
        }

        var gridData = GameGridData[index];
        if (gridData == null) return;

        int gridWidth = gridData.GameGrid.GetLength(0);
        int gridHeight = gridData.GameGrid.GetLength(1);

        // Clamp the rectangle to the grid bounds
        int startX = Math.Max(0, x);
        int startY = Math.Max(0, y);
        int endX = Math.Min(gridWidth, x + w);
        int endY = Math.Min(gridHeight, y + h);

        for (int yy = startY; yy < endY; yy++)
        {
            for (int xx = startX; xx < endX; xx++)
            {
                gridData.GameGrid[xx, yy] = -1;
            }
        }
    }

    public static Texture2D GetTexture2d(int index)
    {
        if (index < 0 || index >= Constants.MaxGameGrids || GameGridData[index] is null)
        {
            return null;
        }

        return TextureUtils.IntArrayToTexture2D(GameGridData[index].GameGrid);
    }

    public static string GetBase64(int index)
    {
        if (index < 0 || index >= Constants.MaxGameGrids || GameGridData[index] is null)
        {
            return string.Empty;
        }

        return TextureUtils.TextureToBase64(GetTexture2d(index));
    }

    public static void SetPixel(int index, int x, int y, int colorIndex = -1)
    {
        if (index < 0 || index >= Constants.MaxGameGrids || GameGridData[index] is null ||
            x < 0 || y < 0 || x >= GameGridData[index].Columns * GameGridData[index].Size || y >= GameGridData[index].Rows * GameGridData[index].Size)
        {
            return;
        }

        GameGridData[index].GameGrid[y, x] = colorIndex;
    }

    public static int GetPixel(int index, int x, int y)
    {
        if (index < 0 || index >= Constants.MaxGameGrids || GameGridData[index] is null ||
            x < 0 || y < 0 || x >= GameGridData[index].Columns * GameGridData[index].Size || y >= GameGridData[index].Rows * GameGridData[index].Size)
        {
            return -1;
        }

        return GameGridData[index].GameGrid[y, x];
    }

    public static void DrawCustomGrid(
        int index, int n, int x, int y, int scale, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        var texture = GetTexture2d(index);

        if (texture is null)
        {
            return;
        }
        var size = GameGridData[index].Size;
        var source = new Rectangle(
            (n % GameGridData[index].Columns) * size,
            (n / GameGridData[index].Columns) * size,
        w * size,
            h * size);
        var destination = new Rectangle(x, y, w * size * scale, h * size * scale);

        SpriteEffects effects = SpriteEffects.None;
        if (flipX) effects |= SpriteEffects.FlipHorizontally;
        if (flipY) effects |= SpriteEffects.FlipVertically;

        GFW.SpriteBatch.Draw(
            texture,
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