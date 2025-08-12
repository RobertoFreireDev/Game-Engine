using blackbox.Utils;

namespace blackbox.IOFile;

public static class TextureFileIO
{
    public static void LoadSpritesAndMap()
    {
        var spritesData = TxtFileIO.Read(Constants.Spritesfilename);
        var spritesMapData = TxtFileIO.Read(Constants.SpriteMapfilename);
        var mapData = TxtFileIO.Read(Constants.Mapfilename);
        // Convert spritesData string to Sprites.SpritesAsSingleRectangle
        // Convert spritesData string to Sprites.SpritesTexture

        // Convert spritesMapData string to Map.SpritesAsSingleRectangle
        // Convert spritesMapData string to local spritesMap Texture
        // Store spritesMap Texture in Map.Sprites = TextureUtils.GetTextures(texture, columns, width, height).ToArray();

        // Convert mapData string to byte[,] data
        // Store data in Map.Data 
    }

    public static void SaveSpritesAndMap()
    {
        string spritesContent = string.Empty; // Get from Sprites.SpritesAsSingleRectangle
        string spriteMapContent = string.Empty; // Get from Map.SpritesAsSingleRectangle
        string mapContent = string.Empty; // Get from Map.Data
        TxtFileIO.CreateOrUpdate(Constants.Spritesfilename, spritesContent);
        TxtFileIO.CreateOrUpdate(Constants.SpriteMapfilename, spriteMapContent);
        TxtFileIO.CreateOrUpdate(Constants.Mapfilename, mapContent);
    }
}
