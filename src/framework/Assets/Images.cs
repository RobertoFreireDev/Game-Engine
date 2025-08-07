using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace framework.Assets;

public static class Images
{
    private static readonly Dictionary<string, string> Base64Images = new Dictionary<string, string>
    {
        {
            "pointer_mouse",
            "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAO9JREFUWIXtlU0OwiAQhQfTSGNP0FVp6P1PZGO74gQmusLVKCJjsfzVpG8HKfRj3vBg4CHZCW2Ox3liPuuCdTpyLTuhbclOaBtqrQ4xNtkBdoAQVSGLY1zP/67Aebq8jQfRf2TDUlWCACiZYA2vddu2JFASAJeUUs75bADX+w0AXjZhJSoq01M9OGgPgmznFvh0NPXt0rwXgGuzhtdgdvAaofckAHqNJ15zihBtpwdKVeJrDiz5F0PFLSDDBgMqlRWD6AEgYxTbP8aeK24BCTDOE1NKPYmzA+RScQCvJzfGjbCbD1W8AsUBfsqBFDfiAWIScBhP8ziLAAAAAElFTkSuQmCC"
        },
        {
            "contextmenu_mouse",
            "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAP5JREFUWIXdV9ESgzAIA271/z/Ya/YwnXZFpRXUW+58ayElKVIiGzB97pCIoC1g47rf01v3HaKpAgAIABERRMRFkm4Jcs4e+dskmE6/bObv9m5JTBVg5nUyjUA3TlVAIdLMyOUars2ZUmoyp3sfGMexab2LBFXQjySm2Ld3whACsx/I8P94WQKmlHpJEDOvSVSyhHigSFD2iusJKGSKnP9pwi1o5jSZcBiGIEoXe6BIPJnTVIEzCURkd3a43YQuFZhPq0mUc96VObQCIsfhvUeyat1RJ+ySwGMWnHG7CaMeJtAM+ch/gdUDbk+x6wIzA0BxCwCE5dMAWt6Qm+PZG8T4YVk0+Y4LAAAAAElFTkSuQmCC"
        }
    };

    public static Dictionary<string, Texture2D> GetAllImages(GraphicsDevice graphicsDevice)
    {
        var textures = new Dictionary<string, Texture2D>();

        foreach (var pair in Base64Images)
        {
            string name = pair.Key;
            string base64 = pair.Value;

            byte[] imageBytes = Convert.FromBase64String(base64);
            using var ms = new MemoryStream(imageBytes);
            var texture = Texture2D.FromStream(graphicsDevice, ms);
            textures.Add(name, texture);
        }

        return textures;
    }
}
