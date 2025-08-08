using framework.Graphics;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace framework.IOFile;

public static class FileIO
{
    public static string Read(string fileName)
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
        catch (Exception e)
        {
            LuaError.SetError("Error reading file: " + e.Message);
        }

        return string.Empty;
    }

    public static bool Create(string fileName, string content)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        try
        {
            if (File.Exists(path))
            {
                LuaError.SetError($"File already exists: {path}");
                return false;
            }

            using (FileStream fs = File.Create(path))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                string[] lines = content.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    writer.WriteLine(lines[i]);
                }
            }

            return true;
        }
        catch (Exception e)
        {
            LuaError.SetError("Error creating file: " + e.Message);
            return false;
        }
    }
}
