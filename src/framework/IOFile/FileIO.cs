using framework.Graphics;
using System;
using System.IO;

namespace framework.IOFile;

public static class FileIO
{
    public static string Read(string fileName)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
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

    public static void Create(string fileName, string content)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (File.Exists(path))
            {
                LuaError.SetError($"File already exists: {path}");
            }

            CreateOrUpdateFile(path, content);

        }
        catch (Exception e)
        {
            LuaError.SetError("Error creating file: " + e.Message);
        }
    }

    public static void Update(string fileName, string content)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (!File.Exists(path))
            {
                LuaError.SetError($"File doesn't exist: {path}");
            }

            CreateOrUpdateFile(path, content);
        }
        catch (Exception e)
        {
            LuaError.SetError("Error updating file: " + e.Message);
        }
    }

    public static void Delete(string fileName)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (!File.Exists(path))
            {
                LuaError.SetError($"File not found: {path}");
            }

            File.Delete(path);
        }
        catch (Exception e)
        {
            LuaError.SetError("Error deleting file: " + e.Message);
        }
    }

    private static void CreateOrUpdateFile(string path, string content)
    {
        using (FileStream fs = File.Create(path))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            string[] lines = content.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i]);
            }
        }
    }
}
