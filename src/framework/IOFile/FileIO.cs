﻿using blackbox.Graphics;
using System;
using System.IO;
using System.Linq;

namespace blackbox.IOFile;

public static class FileIO
{
    public static bool HasFile(string fileName, string extension)
    {
        try
        {
            if (!ValidateFileName(fileName))
            {
                return false;
            }

            fileName += $".{extension}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (File.Exists(path))
            {
                return true;
            }

        }
        catch (Exception e)
        {
            LuaError.SetError("Error finding file: " + e.Message);
        }

        return false;
    }

    public static string Read(string fileName, string extension)
    {
        try
        {
            if (!ValidateFileName(fileName))
            {
                return string.Empty;
            }

            fileName += $".{extension}";

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

    public static void Create(string fileName, string extension, string content)
    {
        try
        {
            if (!ValidateFileName(fileName))
            {
                return;
            }

            fileName += $".{extension}";

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

    public static void Update(string fileName, string extension, string content)
    {
        try
        {
            if (!ValidateFileName(fileName))
            {
                return;
            }

            fileName += $".{extension}";

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

    public static void CreateOrUpdate(string fileName, string extension, string content)
    {
        try
        {
            if (!ValidateFileName(fileName))
            {
                return;
            }

            fileName += $".{extension}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            CreateOrUpdateFile(path, content);
        }
        catch (Exception e)
        {
            LuaError.SetError("Error updating file: " + e.Message);
        }
    }

    public static void Delete(string fileName, string extension)
    {
        try
        {
            if (!ValidateFileName(fileName))
            {
                return;
            }

            fileName += $".{extension}";

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

    public static bool ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Length > 16)
        {
            LuaError.SetError("File name should have 1 to 16 letters");
            return false;
        }

        if (!fileName.All(char.IsLetter))
        {
            LuaError.SetError("File name should have only letters");
            return false;
        }

        return true;
    }
}
