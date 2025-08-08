using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace framework.Graphics;

public static class Font
{
    public static char DefaultKey = '?';
    private static string Id = "medium_font";
    private static int Columns = 19;
    private static int CharWidth = 5;
    private static int CharHeight = 7;

    private static List<char> _charIndexes = new List<char>()
    {
        '0','1','2','3','4','5','6','7','8','9',
        'A','B','C','D','E','F','G','H','I','J','K',
        'L','M','N','O','P','Q','R','S','T','U','V',
        'W','X','Y','Z',
        'a','b','c','d','e','f','g','h','i','j','k',
        'l','m','n','o','p','q','r','s','t','u','v',
        'w','x','y','z',
        ',','.',':',';','[',']','{','}',
        '|','#','$','%','(',')','!','?',
        '"','\'','_','+','-','=','*','/',
        '<','>',' ','~','Ꮖ'
    };

    public static Dictionary<char, Texture2D> GetCharacterTextures(GraphicsDevice graphicsDevice)
    {
        Texture2D fontAtlas = GFW.Textures[Id];
        int columns = Columns;
        int charWidth = CharWidth;
        int charHeight = CharHeight;
        var charTextures = new Dictionary<char, Texture2D>();
        foreach (var charIndex in _charIndexes)
        {
            int row = (_charIndexes.IndexOf(charIndex) / columns);
            int column = (_charIndexes.IndexOf(charIndex) % columns);
            int x = column * charWidth;
            int y = row * charHeight;
            var sourceRect = new Rectangle(x, y, charWidth, charHeight);
            var characterTexture = new Texture2D(graphicsDevice, charWidth, charHeight);
            Color[] data = new Color[charWidth * charHeight];
            fontAtlas.GetData(0, sourceRect, data, 0, data.Length);
            characterTexture.SetData(data);
            charTextures[charIndex] = characterTexture;
        }

        return charTextures;
    }

    public static void DrawTextMultiLine(string text, int x, int y, int lineHeight, Color color)
    {
        string[] lines = text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            DrawText(lines[i], new Vector2(x, y + i * lineHeight), color);
        }
    }

    public static void DrawText(string text, Vector2 position, Color color)
    {
        var boxToDraw = ScreenUtils.BoxToDraw;
        var boxToDrawScale = (ScreenUtils.ScaleX + ScreenUtils.ScaleY) / 2;
        var keyBoardKeys = GFW.MediumFontTextures;
        string[] lines = text.Split('\n');
        var copyPos = new Vector2(position.X, position.Y);
        int additionalLines = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            position = new Vector2(copyPos.X, copyPos.Y + (i + additionalLines) * 9);
            foreach (char key in lines[i])
            {
                var charTexture = keyBoardKeys.ContainsKey(key) ? keyBoardKeys[key] : keyBoardKeys[DefaultKey];

                if (position.X >= ScreenUtils.BaseBox.Width - charTexture.Width*4)
                {
                    additionalLines++;
                    position = new Vector2(copyPos.X, copyPos.Y + (i + additionalLines) * 9);
                }

                if (key == '\t')
                {
                    position += new Vector2(charTexture.Width * 4, 0);
                    continue;
                }

                if (key == '\r')
                {
                    continue;
                }

                GFW.SpriteBatch.Draw(
                    charTexture,
                    new Vector2(boxToDraw.X + (int)(position.X * boxToDrawScale), boxToDraw.Y + (int)(position.Y * boxToDrawScale)),
                    null,
                    color,
                    0f,
                    new Vector2(0, 0),
                    boxToDrawScale,
                    SpriteEffects.None,
                    0f);

                position += new Vector2((charTexture.Width - 1), 0);
            }
        }
    }
}
