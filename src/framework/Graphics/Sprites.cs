using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace framework.Graphics;

public static class Sprites
{
    public static void DrawSprite(Texture2D texure, Rectangle rect, Color color)
    {
        GFW.SpriteBatch.Draw(texure, ScreenUtils.ScaleRectangle(rect), color);
    }
}