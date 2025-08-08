namespace framework.IOFile;

public static class LuaFileIO
{
    public static string Read(string fileName)
    {
        return FileIO.Read($"{fileName}.lua");
    }
}
