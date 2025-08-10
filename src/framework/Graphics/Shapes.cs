using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace framework.Graphics;

public static class Shapes
{
    public static void DrawCircFill(int x, int y, int r, Color clr)
    {
        if (r < 0)
        {
            return;
        }

        if (r == 0)
        {
            GFW.SpriteBatch.Draw(
                GFW.PixelTexture,
                ScreenUtils.ScaleRectangle(new Rectangle(x, y, 1, 1)),
                clr
            );
            return;
        }

        for (int dy = -r; dy <= r; dy++)
        {
            for (int dx = -r; dx <= r; dx++)
            {
                if (Math.Abs(dx) + Math.Abs(dy) <= r * 1.5f)
                {
                    GFW.SpriteBatch.Draw(
                        GFW.PixelTexture,
                        ScreenUtils.ScaleRectangle(new Rectangle(x + dx, y + dy, 1, 1)),
                        clr
                    );
                }
            }
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