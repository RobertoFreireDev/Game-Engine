using framework.Graphics;
using System;
using System.IO;

namespace framework.IOFile;

public static class FileIO
{
    public static string ReadFile(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
        catch (FileNotFoundException e)
        {
            LuaError.SetError("File not found: " + e.Message);
        }
        catch (IOException e)
        {
            LuaError.SetError("Error reading file: " + e.Message);
        }

        return string.Empty;
    }

    public static bool CreateFile(string fileName)
    {
        // Validate file already exists
        return true;
    }

    public static bool UpdateFile(string fileName)
    {
        // Validate file doesn't exist
        return true;
    }

    public static bool Delete(string fileName)
    {
        // Validate file doesn't exist
        return true;
    }
}
