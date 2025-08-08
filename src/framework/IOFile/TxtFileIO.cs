using System;

namespace framework.IOFile;

public static class TxtFileIO
{
    public static bool HasFile(string fileName)
    {
        return FileIO.HasFile($"{fileName}.txt");
    }

    public static string Read(string fileName)
    {
        return FileIO.Read($"{fileName}.txt");
    }

    public static void Create(string fileName, string content)
    {
        FileIO.Create($"{fileName}.txt", content);
    }

    public static void Update(string fileName, string content)
    {
        FileIO.Update($"{fileName}.txt", content);
    }

    public static void Delete(string fileName)
    {
        FileIO.Delete($"{fileName}.txt");
    }
}