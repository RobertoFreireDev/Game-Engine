using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace framework.Graphics;

public static class Shapes
{
    public static void DrawPixel(int x, int y, Color color)
    {
        GFW.SpriteBatch.Draw(GFW.PixelTexture, ScreenUtils.ScaleRectangle(new Rectangle(x, y, 1, 1)), color);
    }

    public static void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            GFW.SpriteBatch.Draw(GFW.PixelTexture, ScreenUtils.ScaleRectangle(new Rectangle(x0, y0, 1, 1)), color);

            if (x0 == x1 && y0 == y1)
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
    }

    public static void DrawCirc(int xm, int ym, int r, Color color)
    {
        if (r < 0)
        {
            return;
        }

        if (r == 0)
        {
            GFW.SpriteBatch.Draw(
                GFW.PixelTexture,
                ScreenUtils.ScaleRectangle(new Rectangle(xm, ym, 1, 1)),
                color
            );
            return;
        }

        int x0 = xm - r;
        int y0 = ym - r;
        int x1 = xm + r;
        int y1 = ym + r;

        int rx0 = Math.Min(x0, x1);
        int ry0 = Math.Min(y0, y1);
        int rx1 = Math.Max(x0, x1);
        int ry1 = Math.Max(y0, y1);
        var bounds = ScreenUtils.ScaleRectangle(new Rectangle(rx0, ry0, rx1 - rx0, ry1 - ry0));
        rx0 = bounds.Left;
        ry0 = bounds.Top;
        rx1 = bounds.Right;
        ry1 = bounds.Bottom;

        int xC = (int)Math.Round((rx0 + rx1) / 2.0);
        int yC = (int)Math.Round((ry0 + ry1) / 2.0);

        int evenX = (rx0 + rx1) % 2;
        int evenY = (ry0 + ry1) % 2;

        int rX = rx1 - xC;
        int rY = ry1 - yC;

        List<Point> pixels = new List<Point>();

        for (int x = rx0; x <= xC; x++)
        {
            double angle = Math.Acos((x - xC) / (double)rX);
            int y = (int)Math.Round(rY * Math.Sin(angle) + yC);

            pixels.Add(new Point(x - evenX, y));
            pixels.Add(new Point(x - evenX, 2 * yC - y - evenY));
            pixels.Add(new Point(2 * xC - x, y));
            pixels.Add(new Point(2 * xC - x, 2 * yC - y - evenY));
        }
        for (int y = ry0; y <= yC; y++)
        {
            double angle = Math.Asin((y - yC) / (double)rY);
            int x = (int)Math.Round(rX * Math.Cos(angle) + xC);

            pixels.Add(new Point(x, y - evenY));
            pixels.Add(new Point(2 * xC - x - evenX, y - evenY));
            pixels.Add(new Point(x, 2 * yC - y));
            pixels.Add(new Point(2 * xC - x - evenX, 2 * yC - y));
        }

        foreach (var p in pixels)
        {
            GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(p.X, p.Y, 1, 1), color);
        }
    }

    public static void DrawCircFill(int xm, int ym, int r, Color color)
    {
        if (r < 0)
        {
            return;
        }

        if (r == 0)
        {
            GFW.SpriteBatch.Draw(
                GFW.PixelTexture,
                ScreenUtils.ScaleRectangle(new Rectangle(xm, ym, 1, 1)),
                color
            );
            return;
        }

        int x0 = xm - r;
        int y0 = ym - r;
        int x1 = xm + r;
        int y1 = ym + r;

        int rx0 = Math.Min(x0, x1);
        int ry0 = Math.Min(y0, y1);
        int rx1 = Math.Max(x0, x1);
        int ry1 = Math.Max(y0, y1);
        var bounds = ScreenUtils.ScaleRectangle(new Rectangle(rx0, ry0, rx1 - rx0, ry1 - ry0));
        rx0 = bounds.Left;
        ry0 = bounds.Top;
        rx1 = bounds.Right;
        ry1 = bounds.Bottom;

        int xC = (int)Math.Round((rx0 + rx1) / 2.0);
        int yC = (int)Math.Round((ry0 + ry1) / 2.0);

        int evenX = (rx0 + rx1) % 2;
        int evenY = (ry0 + ry1) % 2;

        int rX = rx1 - xC;
        int rY = ry1 - yC;

        // Dictionary keyed by y, value = list of x positions in that y line
        var linePixels = new Dictionary<int, List<int>>();

        // Helper to add points in linePixels dictionary
        void AddPixel(int x, int y)
        {
            if (!linePixels.TryGetValue(y, out var xs))
            {
                xs = new List<int>();
                linePixels[y] = xs;
            }
            xs.Add(x);
        }

        // Collect points on the circle border (top/bottom half and left/right half)
        for (int x = rx0; x <= xC; x++)
        {
            double angle = Math.Acos((x - xC) / (double)rX);
            int y = (int)Math.Round(rY * Math.Sin(angle) + yC);

            AddPixel(x - evenX, y);
            AddPixel(x - evenX, 2 * yC - y - evenY);
            AddPixel(2 * xC - x, y);
            AddPixel(2 * xC - x, 2 * yC - y - evenY);
        }
        for (int y = ry0; y <= yC; y++)
        {
            double angle = Math.Asin((y - yC) / (double)rY);
            int x = (int)Math.Round(rX * Math.Cos(angle) + xC);

            AddPixel(x, y - evenY);
            AddPixel(2 * xC - x - evenX, y - evenY);
            AddPixel(x, 2 * yC - y);
            AddPixel(2 * xC - x - evenX, 2 * yC - y);
        }

        // For each line, find min and max x, then draw a rectangle spanning from min x to max x at y with height 1
        foreach (var kvp in linePixels)
        {
            int y = kvp.Key;
            List<int> xs = kvp.Value;
            xs.Sort();

            int minX = xs[0];
            int maxX = xs[xs.Count - 1];
            int width = maxX - minX + 1;

            GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(minX, y, width, 1), color);
        }
    }

    public static void DrawRectFill(Rectangle rect, Color color)
    {
        GFW.SpriteBatch.Draw(GFW.PixelTexture, ScreenUtils.ScaleRectangle(rect), color);
    }

    public static void DrawRectBorder(Rectangle rect, Color color, int thickness = 1)
    {
        // Top
        DrawRectFill(new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Bottom
        DrawRectFill(new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        // Left
        DrawRectFill(new Rectangle(rect.X, rect.Y+1, thickness, rect.Height - 2), color);
        // Right
        DrawRectFill(new Rectangle(rect.X + rect.Width - thickness, rect.Y + 1, thickness, rect.Height -2), color);
    }

    public static void DrawRectWithHole(GraphicsDevice graphicsDevice, Rectangle hole, Color color)
    {
        var viewport = graphicsDevice.Viewport.Bounds;
        hole = ScreenUtils.ScaleRectangle(hole);
        GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Y + hole.Y), color);
        GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(viewport.X, hole.Bottom, viewport.Width, viewport.Bottom - hole.Bottom), color);
        GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(viewport.X, hole.Y, hole.X - viewport.X, hole.Height), color);
        GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(hole.Right, hole.Y, viewport.Right - hole.Right, hole.Height), color);
    }
}