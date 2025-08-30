using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace blackbox.Graphics;

public static class Shapes
{
    public static void DrawPixel(int x, int y, Color color)
    {
        GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(x, y, 1, 1), color);
    }

    public static void DrawLine(int ox, int oy, int x0, int y0, int x1, int y1, int scale, Color color)
    {
        scale = Math.Max(scale, 1);
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        int count = 0;

        while (true)
        {
            count++;
            GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(ox + x0*scale, oy + y0 * scale, scale, scale), color);

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
    }

    public static void DrawCirc(int ox, int oy, int x0, int y0, int x1, int y1, int scale, Color color)
    {
        scale = Math.Max(scale, 1);
        int rx0 = Math.Min(x0, x1);
        int ry0 = Math.Min(y0, y1);
        int rx1 = Math.Max(x0, x1);
        int ry1 = Math.Max(y0, y1);        
        var bounds = new Rectangle(rx0, ry0, rx1 - rx0, ry1 - ry0);
        rx0 = bounds.Left;
        ry0 = bounds.Top;
        rx1 = bounds.Right;
        ry1 = bounds.Bottom;

        if (rx1 - rx0 <= 1 && ry1 - ry0 <= 1)
        {
            GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(ox + rx0 * scale, oy + ry0 * scale, (rx1 + 1 - rx0) * scale,(ry1 + 1 - ry0) * scale), color);
            return;
        }

        int xC = (int)Math.Ceiling((rx0 + rx1) / 2.0);
        int yC = (int)Math.Ceiling((ry0 + ry1) / 2.0);

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
            if (p.X < rx0 || p.X > rx1 || p.Y < ry0 || p.Y > ry1)
            {
                continue;
            }

            GFW.SpriteBatch.Draw(GFW.PixelTexture, new Rectangle(ox + p.X * scale, oy + p.Y * scale, scale, scale), color);
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
                new Rectangle(xm, ym, 1, 1),
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
        var bounds = new Rectangle(rx0, ry0, rx1 - rx0, ry1 - ry0);
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
        GFW.SpriteBatch.Draw(GFW.PixelTexture, rect, color);
    }

    public static void DrawRectBorder(Rectangle rect, Color color, int thickness = 1)
    {
        // Top
        DrawRectFill(new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Bottom
        DrawRectFill(new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        // Left
        DrawRectFill(new Rectangle(rect.X, rect.Y + 1, thickness, rect.Height - 2), color);
        // Right
        DrawRectFill(new Rectangle(rect.X + rect.Width - thickness, rect.Y + 1, thickness, rect.Height - 2), color);
    }
}