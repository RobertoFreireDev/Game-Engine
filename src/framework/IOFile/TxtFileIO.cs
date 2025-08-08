namespace framework.IOFile;

public static class TxtFileIO
{
    public static string Read(string fileName)
    {
        return FileIO.Read($"{fileName}.txt");
    }

    public static bool Create(string fileName, string content)
    {
        return FileIO.Create($"{fileName}.txt", content);
    }
}