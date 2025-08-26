using System;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace blackbox.Utils;

public static class ColorUtils
{
    private const int Qty = 32;

    private static Color[] Colors = new Color[Qty];

    private static string defaultPalette = "#000000,#dfe9f5,#f7aaa8,#697594,#d4689a,#782c96,#e83562,#f2825c,#ffc76e,#88c44d,#3f9e59,#373461,#4854a8,#7199d9,#9e5252,#4d2536,#0a080d,#6f6776,#9a9a97,#c5ccb8,#8b5580,#c38890,#a593a5,#666092,#9a4f50,#c28d75,#7ca1c0,#416aa3,#8d6268,#be955c,#68aca9,#387080,#6e6962,#93a167,#6eaa78,#557064,#9d9f7f,#7e9e99,#5d6872,#433455";

    private static readonly Regex HexColorRegex = new Regex(@"^#[0-9a-fA-F]{6}$", RegexOptions.Compiled);

    public static Color GetColor(int colorIndex, int transparency = 10)
    {
        transparency = Math.Clamp(transparency, -1, 10);

        if (colorIndex < 0 || colorIndex >= 32)
        {
            return new Color(1, 1, 1, 1);
        }

        return Colors[colorIndex] * (transparency / 10.0f);
    }

    public static void SetPalette(string palette = "")
    {
        if (string.IsNullOrWhiteSpace(palette) || !ValidateHexColorList(palette))
        {
            SetColor(defaultPalette);
            return;
        }

        try
        {
            SetColor(palette);
        }
        catch (Exception ex) {
            SetColor(defaultPalette);
        }        
    }

    private static void SetColor(string palette)
    {
        string[] colors = palette.Split(',');

        for (int i=0; i < Qty; i++)
        {
            Colors[i] = GetColor(colors[i].Trim());
        }
    }

    private static Color GetColor(string hexColor)
    {
        try
        {
            hexColor = hexColor.Substring(1);
            int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);
            return new Color(r, g, b);
        }
        catch (Exception ex)
        {
            return Colors[0];
        }
    }

    public static bool ValidateHexColorList(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        string[] colors = input.Split(',');

        if (colors.Length != Qty)
            return false;

        foreach (string color in colors)
        {
            if (!HexColorRegex.IsMatch(color.Trim()))
                return false;
        }

        return true;
    }
}