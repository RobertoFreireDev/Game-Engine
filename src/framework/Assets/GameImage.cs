using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace blackbox.Assets;

public class GameImageData
{
    public Texture2D GameTexture;
    public List<Texture2D> GameTextures = new List<Texture2D>();
    private IEnumerator<Texture2D> _textureEnumerator;
    public int Columns;
    public int Rows;
    public int TileWidth;
    public int TileHeight;
    public int Total;
    public int Loaded = 0;

    public GameImageData(Texture2D texture, int width, int height)
    {
        GameTexture = texture;
        TileWidth = width;
        TileHeight = height;
        Columns = GameTexture.Width / TileWidth;
        Rows = GameTexture.Height / TileHeight;
        Total = Columns * Rows;
        _textureEnumerator = TextureUtils.GetTexturesOneAtATime(GameTexture, Columns, TileWidth, TileHeight).GetEnumerator();
    }

    public bool LoadNextTexture()
    {
        if (_textureEnumerator.MoveNext())
        {
            GameTextures.Add(_textureEnumerator.Current);
            Loaded++;
            return true;
        }

        return false;
    }
}

public static class GameImage
{
    public static GameImageData[] GameImageData = new GameImageData[Constants.MaxGameTextures];

    public static void LoadTexture(int index, string spriteBase64, int width, int height)
    {
        if (index < 0 || index >= Constants.MaxGameTextures)
        {
            return;
        }

        GameImageData[index] = new GameImageData(
                TextureUtils.Convert64ToTexture(spriteBase64),
                width,
                height
            );
    }

    public static bool LoadNextSplitedTexture(int index)
    {
        if (index < 0 || index >= Constants.MaxGameTextures || GameImageData[index] is null)
        {
            return false;
        }

        return GameImageData[index].LoadNextTexture();
    }

    public static void DrawCustomSprite(
        int index, int n, int x, int y, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        if (index < 0 || index >= Constants.MaxGameTextures || GameImageData[index] is null || n < 0 || n >= GameImageData[index].Total)
        {
            return;
        }

        if (w == 1 && h == 1)
        {
            DrawSingleSprite(index, n, x, y, color, flipX, flipY);
            return;
        }

        DrawMultipleSprite(index, n, x, y, color, w, h, flipX, flipY);
    }

    private static void DrawSingleSprite(int index, int n, int x, int y, Color color, bool flipX = false, bool flipY = false)
    {
        if (n > GameImageData[index].Loaded || n >= GameImageData[index].GameTextures.Count)
        {
            return;
        }

        SpriteEffects effects = SpriteEffects.None;
        if (flipX) effects |= SpriteEffects.FlipHorizontally;
        if (flipY) effects |= SpriteEffects.FlipVertically;

        GFW.SpriteBatch.Draw(
            GameImageData[index].GameTextures[n],
            new Vector2(x,y),
            color,
            effects
        );
    }

    public static void DrawMultipleSprite(
        int index, int n, int x, int y, Color color, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        if (GameImageData[index] is null || n < 0 || n >= GameImageData[index].Total || index < 0 || index >= Constants.MaxGameTextures)
        {
            return;
        }

        Rectangle source = new Rectangle(
            (n % GameImageData[index].Columns) * GameImageData[index].TileWidth,
            (n / GameImageData[index].Columns) * GameImageData[index].TileHeight,
            w * GameImageData[index].TileWidth,
            h * GameImageData[index].TileHeight);
        Rectangle destination = new Rectangle(x, y, w * GameImageData[index].TileWidth, h * GameImageData[index].TileHeight);
        SpriteEffects effects = SpriteEffects.None;
        if (flipX) effects |= SpriteEffects.FlipHorizontally;
        if (flipY) effects |= SpriteEffects.FlipVertically;

        GFW.SpriteBatch.Draw(
            GameImageData[index].GameTexture,
            destination,
            source,
            color,
            effects
        );
    }
}
