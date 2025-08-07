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
            "iVBORw0KGgoAAAANSUhEUgAAAF8AAAAjCAYAAADyrNZPAAAAAXNSR0IArs4c6QAAAolJREFUaIHtWtFyxCAIjJn+/y/bl9px6C4sqO1dJ/t0IQKLItBMr+vBn6Fd13X13vu3oLU2fvfeu32265istdZm/YzuCZkaRzXe6h58C+eX/QsXAJJbXSaL7GTtrciQvwpn1Z59vq3SwJwBsyIyliGk6qv2VBmKR8FKzFFcdPMZ2hfYc0Y25Krfqr1x/RU/SFf1rXCZ8TF+sFOzWTPkqI55yJQwZV1mM70s9XocW6f6YGtWbuKDB/8M1SnBNqBMQ2K6kb2Mbia2qozF7CHdcFXYJjfqnCW40jRR82f6J4EasJJE7ub/RmNQswVxqWRbhov9I4mtY/zY3g39mxmbnUekVw4oM7bNXFS9FaDbim4XS1K0d/O621vICFWyjWUR86GMdhnOVS4qEB/1QF4Ku8vJgwcPXgnbPsUqMltrV3W9KcSLA+lnY6vqznHc8wI0KkUd29ONRjQ0OSDZTNxyUWDjQFMM8unJon1hccwofdW0MtYgleAyfk+PvUoMQ2Y3WuU348dXzWh+ReuqI9VYw0qRqp/dcDZqsudoVK1wfvDg9XEqW499WDuNPsGTzXL2PGTIB/ONnrN+b6a8KmNNeCfsRIUmrPkd+h1B2Wiv5yG/410q81mjQg0pY9cik4UVWxmbymEOW9m4w0/KyAE6dTQlWVl0La3NSKbA01NsssSyN73C7x4GqwYYVsqOmvlKLUf6GV7RLWfv2CEhm8exUv9P946s3118nn9fcBA11W1OWD2OJhv0fIIf8xPJVrmdjM39d8Hdp60Egg5dnbIyshWOu5D67LqCqBl5ftg6y8+LY5ePt0U185Eum2xQqVT9VjkvwQtkxd4u4qjme7bfpea//3U6CLUsVvEJpOFiEXN3SSoAAAAASUVORK5CYII="
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
