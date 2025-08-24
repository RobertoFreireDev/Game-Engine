using System.IO;
using System;
using blackbox.Graphics;

namespace framework.Utils;

public static class FileUtils
{
    public static string GetFileContent(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
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
}
