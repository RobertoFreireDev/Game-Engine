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
        ClearGrid(0, 0, Columns * Size, Rows * Size);
    }

    public static void ClearGrid(int x, int y, int w, int h)
    {
        var (x1, y1, x2, y2) = ClampToBounds(x, y, w, h);

        for (int yy = y1; yy < y2; yy++)
        {
            for (int xx = x1; xx < x2; xx++)
            {
                Data[xx, yy] = -1;
            }
        }

        UpdateTexture2d();
    }

    public static (int x1, int y1, int x2, int y2) ClampToBounds(int x, int y, int w, int h)
    {
        return (
                Math.Max(0, x),
                Math.Max(0, y),
                Math.Min(Columns * Size, x + w),
                Math.Min(Rows * Size, y + h)
            );
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
        // TO DO: Clamp in boundaries

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