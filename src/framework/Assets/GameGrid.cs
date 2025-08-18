using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace blackbox.Assets;

public class GameGridData
{
    public int[,] GameGrid;
    public Texture2D GameGridTexture;
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
        ClearGrid(0, 0, Size, Size);
    }

    public void ClearGrid(int x, int y, int w, int h)
    {
        int gridWidth = GameGrid.GetLength(0);
        int gridHeight = GameGrid.GetLength(1);

        // Clamp the rectangle to the grid bounds
        int startX = Math.Max(0, x);
        int startY = Math.Max(0, y);
        int endX = Math.Min(gridWidth, x + w);
        int endY = Math.Min(gridHeight, y + h);

        for (int yy = startY; yy < endY; yy++)
        {
            for (int xx = startX; xx < endX; xx++)
            {
                GameGrid[xx, yy] = -1;
            }
        }

        UpdateTexture2d();
    }

    public void SetPixel(int x, int y, int colorIndex = -1)
    {
        GameGrid[y, x] = colorIndex;
        UpdateTexture2d();
    }

    public void UpdateTexture2d()
    {
        GameGridTexture = TextureUtils.IntArrayToTexture2D(GameGrid);
    }
}

public static class GameGrid
{
    public static GameGridData[] GameGridData = new GameGridData[Constants.MaxGameGrids];

    public static void CreateGrid(int index)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GameGridData[index] = new GameGridData();
    }

    public static void ClearGrid(int index, int x, int y, int w, int h)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GameGridData[index].ClearGrid(x, y, w, h);
    }

    public static bool IsValidIndex(int index)
    {
        return index >= 0 && index < Constants.MaxGameGrids;
    }

    public static string GetBase64(int index)
    {
        if (!IsValidIndex(index) || GameGridData[index] is null)
        {
            return string.Empty;
        }

        return TextureUtils.TextureToBase64(GameGridData[index].GameGridTexture);
    }

    public static void SetPixel(int index, int x, int y, int colorIndex = -1)
    {
        if (!IsValidIndex(index) || GameGridData[index] is null ||
            x < 0 || y < 0 || x >= GameGridData[index].Columns * GameGridData[index].Size || y >= GameGridData[index].Rows * GameGridData[index].Size)
        {
            return;
        }

        GameGridData[index].SetPixel(x, y, colorIndex);
    }

    public static int GetPixel(int index, int x, int y)
    {
        if (!IsValidIndex(index) || GameGridData[index] is null ||
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
        if (!IsValidIndex(index))
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
            GameGridData[index].GameGridTexture,
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