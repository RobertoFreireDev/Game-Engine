using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace framework.Utils;

public static class TextureUtils
{
    public static Dictionary<int, Texture2D> GetTextures(
        GraphicsDevice graphicsDevice,
        Texture2D texture,
        int columns,
        int width,
        int height)
    {
        var textures = new Dictionary<int, Texture2D>();

        int rows = texture.Height / height;
        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var subTexture = new Texture2D(texture.GraphicsDevice, width, height);

                Color[] data = new Color[width * height];
                texture.GetData(0, new Rectangle(x * width, y * height, width, height), data, 0, data.Length);
                subTexture.SetData(data);
                textures.Add(index, subTexture);
                index++;
            }
        }
        return textures;
    }

    public static Texture2D Convert64ToTexture(GraphicsDevice graphicsDevice, string imageBase64)
    {
        byte[] imageBytes = Convert.FromBase64String(imageBase64);
        using var ms = new MemoryStream(imageBytes);
        return Texture2D.FromStream(graphicsDevice, ms);
    }
}