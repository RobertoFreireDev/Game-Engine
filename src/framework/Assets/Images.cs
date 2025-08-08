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
        },
        {
            "medium_font",
            "iVBORw0KGgoAAAANSUhEUgAAAF8AAAAjCAYAAADyrNZPAAAAAXNSR0IArs4c6QAAAoJJREFUaIHtWsuOwyAMhGj//5fZS1MhrwdmbGijFXNqHPwCYztWSzn4GmoppbTW2ptQa71/t9aafbbrEK3WWnt+hXcHjfUj6m90D97E/mV7wZ6WXefRkDxGjiovQ/P0RWxm5dnnCznfR0DP6AlTDGL5WXkszfOHQcbnmV9w8xHqC+hZoRVwyEhvVN59/Rk9Hi+rm7Glx8/9A50ayvteHhtBSWHMOmUzR1E6qnFoHasDrcncxIODf4Zol2ALkFKQEO9MnsKr+BalsY1DD7ngsrBFrnV9r13n8c7kFVD8Ef9OeAWYCaLh5n+iMCgFFzm4yxb7kYTWIfvQ3t38FxJWhBYtc0BK29bbwvJl4N1W73ahIPX2rl93jRYigyLRhqII6WBaO8XmqC0sPHvYA3kUVqeTg4ODJ2HZKJahZcbMHu+oCxn54fGrvkV5ez+ufoHXKs0q9oh31qJ5nQPq1TMF8hPfHKwfPUJTTUtDBZJxTtG7u+1lfChOINyBpjYJf6aakSlftKXqP9XVKSmyhQFqNdHzrFWN2Hxw8HzsitZtg7XdGA2u7GbNBlxFyPcevU+fit4LMWdpn/hK9aaaKP/PahQCs9Gjmufpvd9Jkc9O+bIzEyUKI7IUmcxh3rJUv6cjZU+Bd+pel2Rps2uJHEM0BiM+dsjn8XjfDaptVxlEdAaZtKP027Nc7vErds1uOXqHDsmTuR2Z/P+t3pgtuFGcvy+ImBXakECUj2edjfe8GkivpbEpIWvvKn+HfxdcnZsYo71DZ7usLC1jdxioUK1Uim4SU5jROsbmkW8r9T4e0cj3eJVNRYcetXfp5q+OfDaiFXnWHja/e7Sn5PzT7YiwtSgj6xcaOqWkv64FWwAAAABJRU5ErkJggg=="
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
