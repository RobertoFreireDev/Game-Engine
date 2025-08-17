using System;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace blackbox.Utils;

public static class ColorUtils
{
    private static Color PrimaryColor;

    private static Color SecondaryColor;

    private static Color TertiaryColor;

    private static Color QuaternaryColor;

    private static Color Color5;

    private static Color Color6;

    private static Color Color7;

    private static Color Color8;

    private static Color Color9;

    private static Color Color10;

    private static Color Color11;

    private static Color Color12;

    private static Color Color13;

    private static Color Color14;

    private static Color Color15;

    private static Color Color16;

    private static string defaultPalette = "#0a080d,#dfe9f5,#f7aaa8,#697594,#d4689a,#782c96,#e83562,#f2825c,#ffc76e,#88c44d,#3f9e59,#373461,#4854a8,#7199d9,#9e5252,#4d2536";

    private static readonly Regex HexColorRegex = new Regex(@"^#[0-9a-fA-F]{6}$", RegexOptions.Compiled);

    public static Color GetColor(int Color, int transparency = 10)
    {
        transparency = Math.Clamp(transparency, 0, 10);
        switch (Color)
        {
            case 0:
                return PrimaryColor * (transparency / 10.0f);
            case 1:
                return SecondaryColor * (transparency / 10.0f);
            case 2:
                return TertiaryColor * (transparency / 10.0f);
            case 3:
                return QuaternaryColor * (transparency / 10.0f);
            case 4:
                return Color5 * (transparency / 10.0f);
            case 5:
                return Color6 * (transparency / 10.0f);
            case 6:
                return Color7 * (transparency / 10.0f);
            case 7:
                return Color8 * (transparency / 10.0f);
            case 8:
                return Color9 * (transparency / 10.0f);
            case 9:
                return Color10 * (transparency / 10.0f);
            case 10:
                return Color11 * (transparency / 10.0f);
            case 11:
                return Color12 * (transparency / 10.0f);
            case 12:
                return Color13 * (transparency / 10.0f);
            case 13:
                return Color14 * (transparency / 10.0f);
            case 14:
                return Color15 * (transparency / 10.0f);
            case 15:
                return Color16 * (transparency / 10.0f);
            default:
                return new Color(1,1,1,1);
        }
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
        PrimaryColor = GetColor(colors[0].Trim());
        SecondaryColor = GetColor(colors[1].Trim());
        TertiaryColor = GetColor(colors[2].Trim());
        QuaternaryColor = GetColor(colors[3].Trim());
        Color5 = GetColor(colors[4].Trim());
        Color6 = GetColor(colors[5].Trim());
        Color7 = GetColor(colors[6].Trim());
        Color8 = GetColor(colors[7].Trim());
        Color9 = GetColor(colors[8].Trim());
        Color10 = GetColor(colors[9].Trim());
        Color11 = GetColor(colors[10].Trim());
        Color12 = GetColor(colors[11].Trim());
        Color13 = GetColor(colors[12].Trim());
        Color14 = GetColor(colors[13].Trim());
        Color15 = GetColor(colors[14].Trim());
        Color16 = GetColor(colors[15].Trim());
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
            return PrimaryColor;
        }
    }

    public static bool ValidateHexColorList(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        string[] colors = input.Split(',');

        if (colors.Length != 16)
            return false;

        foreach (string color in colors)
        {
            if (!HexColorRegex.IsMatch(color.Trim()))
                return false;
        }

        return true;
    }
}