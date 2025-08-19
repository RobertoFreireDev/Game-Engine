using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace blackbox.Assets;

public static class GameGrid
{
    public static int[,] Data;
    public static Texture2D Texture;
    public static int Columns = Constants.GameGridWidth;
    public static int Rows = Constants.GameGridHeight;
    public static int Size = Constants.GameGridSize;
    public static int Total = Columns * Rows;

    public static void Create()
    {
        Data = new int[Columns * Size, Rows * Size];
        ClearGrid(0, 0, Size, Size);
    }

    public static void ClearGrid(int x, int y, int w, int h)
    {
        int gridWidth = Data.GetLength(0);
        int gridHeight = Data.GetLength(1);

        // Clamp the rectangle to the grid bounds
        int startX = Math.Max(0, x);
        int startY = Math.Max(0, y);
        int endX = Math.Min(gridWidth, x + w);
        int endY = Math.Min(gridHeight, y + h);

        for (int yy = startY; yy < endY; yy++)
        {
            for (int xx = startX; xx < endX; xx++)
            {
                Data[xx, yy] = -1;
            }
        }

        UpdateTexture2d();
    }

    public static void SetPixel(int x, int y, int colorIndex)
    {
        if (InvalidGridPos(x, y))
        {
            return;
        }
        Data[y, x] = colorIndex;
        UpdateTexture2d();
    }

    public static bool InvalidGridPos(int x, int y)
    {
        return x < 0 || y < 0 || x >= Columns * Size || y >= Rows * Size;
    }

    public static void UpdateTexture2d()
    {
        Texture = TextureUtils.IntArrayToTexture2D(Data);
    }

    public static string GetBase64()
    {
        return TextureUtils.TextureToBase64(Texture);
    }

    public static int GetPixel(int x, int y)
    {
        if (InvalidGridPos(x, y))
        {
            return -1;
        }

        return Data[y, x];
    }

    public static void DrawCustomGrid(
        int n, int x, int y, int scale, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        var source = new Rectangle(
            (n % Columns) * Size,
            (n / Columns) * Size,
            w * Size,
            h * Size);
        var destination = new Rectangle(x, y, w * Size * scale, h * Size * scale);
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