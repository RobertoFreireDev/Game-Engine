namespace framework.IOFile;

public static class LuaFileIO
{
    private static string extension = "lua";

    public static string Read(string fileName)
    {
        return FileIO.Read(fileName, extension);
    }
}