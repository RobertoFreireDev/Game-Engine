using framework.Utils;
using Microsoft.Xna.Framework;

namespace framework.Graphics;

public static class Shapes
{
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
        DrawRectFill(new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Right
        DrawRectFill(new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
    }
}