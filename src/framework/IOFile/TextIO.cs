namespace framework.IOFile;

public static class TextIO
{
    // extension .txt
    public static string Read(string filename)
    {
        return string.Empty;
    }

    public static bool Create(string filename)
    {
        // Validate file already exists
        return true;
    }

    public static bool Update(string filename)
    {
        // Validate file doesn't exist
        return true;
    }

    public static bool Delete(string filename)
    {
        // Validate file doesn't exist
        return true;
    }
}