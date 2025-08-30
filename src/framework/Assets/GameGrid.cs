using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace blackbox.Assets;

public class GridData
{
    public int[,] Data;
    public Texture2D Texture;
    public Rectangle[] TileRects;
    public int Columns;
    public int Rows;
    public int Size;
    public int Total;
    public int Margin = 1; // to avoid last column stretching issue

    public void Create(int columns, int rows, int size)
    {
        Columns = columns;
        Rows = rows;
        Size = size;
        Total = Columns * Rows;
        TileRects = new Rectangle[Total];
        for (int i = 0; i < Total; i++)
        {
            int x = (i % Columns) * Size;
            int y = (i / Columns) * Size;
            TileRects[i] = new Rectangle(x, y, Size, Size);
        }
        Data = new int[Rows * Size + Margin, Columns * Size + Margin];
        ClearGrid(0, 0, Rows * Size, Columns * Size);
    }

    public void ClearGrid(int x, int y, int w, int h)
    {
        var (x1, y1, x2, y2) = ClampToBounds(x, y, w, h);

        for (int yy = y1; yy < y2; yy++)
        {
            for (int xx = x1; xx < x2; xx++)
            {
                Data[yy, xx] = -1;
            }
        }

        UpdateTexture2d();
    }

    public (int x1, int y1, int x2, int y2) ClampToBounds(int x, int y, int w, int h)
    {
        return (
                Math.Max(0, x),
                Math.Max(0, y),
                Math.Min(Columns * Size, y + h),
                Math.Min(Rows * Size, x + w)
            );
    }

    public void SetPixel(int x, int y, int colorIndex)
    {
        if (InvalidGridPos(x, y))
        {
            return;
        }
        Data[y, x] = colorIndex;
        UpdateTexture2d();
    }

    public void SetLine(int x0, int y0, int x1, int y1, int colorIndex)
    {
        if (InvalidGridPos(x0, y0) || InvalidGridPos(x1, y1))
        {
            return;
        }

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        int count = 0;
        while (true)
        {
            count++;
            Data[y0,x0] = colorIndex;

            if (x0 == x1 && y0 == y1 || count > 500)
                break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        UpdateTexture2d();
    }

    public bool InvalidGridPos(int x, int y)
    {
        return x < 0 || y < 0 || x >= Columns * Size || y >= Rows * Size;
    }

    public void UpdateTexture2d()
    {
        Texture = TextureUtils.IntArrayToTexture2D(Data);
    }

    public string GetGameGrid()
    {
        return ArrayUtils.IntArrayToString(Data);
    }

    public void SetGameGrid(string gamegrid)
    {
        ArrayUtils.StringToIntArray(Data, gamegrid);
        UpdateTexture2d();
    }

    public int GetPixel(int x, int y)
    {
        if (InvalidGridPos(x, y))
        {
            return -1;
        }

        return Data[y, x];
    }

    public void DrawCustomGrid(
        int n, int x, int y, int scale, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        var source = new Rectangle(
            (n % Columns) * Size,
            (n / Columns) * Size,
            w * Size,
            h * Size);
        var destination = new Rectangle(x, y,
            w * Size * scale,
            h * Size * scale);
        SpriteEffects effects = SpriteEffects.None;
        if (flipX) effects |= SpriteEffects.FlipHorizontally;
        if (flipY) effects |= SpriteEffects.FlipVertically;

        GFW.SpriteBatch.Draw(
            Texture,
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

public static class GameGrid
{
    public static GridData[] GridList = new GridData[Constants.MaxGameGrid];

    public static void Create(int index, int columns, int rows, int size)
    {
        GridList[index] = new GridData();
        GridList[index].Create(columns, rows, size);
    }

    public static void ClearGrid(int index, int x, int y, int w, int h)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].ClearGrid(x, y, w, h);
    }

    private static bool IsValidIndex(int index)
    {
        return index >= 0 && index < Constants.MaxGameGrid && GridList[index] is not null;
    }

    public static void SetPixel(int index, int x, int y, int colorIndex)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].SetPixel(x, y, colorIndex);
    }

    public static void SetLine(int index, int x0, int y0, int x1, int y1, int colorIndex)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].SetLine(x0, y0, x1, y1, colorIndex);
    }

    public static string GetGameGrid(int index)
    {
        if (!IsValidIndex(index))
        {
            return string.Empty;
        }

        return GridList[index].GetGameGrid();
    }

    public static void SetGameGrid(int index, string gamegrid)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].SetGameGrid(gamegrid);
    }

    public static int GetPixel(int index, int x, int y)
    {
        if (!IsValidIndex(index))
        {
            return -1;
        }

        return GridList[index].GetPixel(x, y);
    }

    public static void DrawCustomGrid(
        int index, int n, int x, int y, int scale, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].DrawCustomGrid(n, x, y, scale, color, w, h, flipX, flipY);
    }
}