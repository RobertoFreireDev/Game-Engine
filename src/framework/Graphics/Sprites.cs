using framework.Assets;
using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace framework.Graphics;

public static class Sprites
{
    public static void DrawSprite(
        int n,int x, int y, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        if (n < 0 || n >= GameImage.Total)
        {
            return;
        }

        int scale = (int)(ScreenUtils.ScaleX + ScreenUtils.ScaleY) / 2;
        Rectangle source = new Rectangle(
            (n % GameImage.Columns) * GameImage.TileWidth,
            (n / GameImage.Columns) * GameImage.TileHeight, 
            w * GameImage.TileWidth, 
            h * GameImage.TileHeight);
        Rectangle destination = new Rectangle(x,y, w * GameImage.TileWidth, h * GameImage.TileHeight);
        SpriteEffects effects = SpriteEffects.None;
        if (flipX) effects |= SpriteEffects.FlipHorizontally;
        if (flipY) effects |= SpriteEffects.FlipVertically;

        GFW.SpriteBatch.Draw(
            GameImage.GameTexture,
            ScreenUtils.ScaleRectangle(destination),
            source,
            Color.White,
            0f,
            Vector2.Zero,
            effects,
            0f
        );
    }
}