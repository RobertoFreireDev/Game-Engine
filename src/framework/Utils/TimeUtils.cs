using Microsoft.Xna.Framework;
using System;

namespace blackbox.Utils;

public static class TimeUtils
{
    private static int _frameCounter = 0;
    private static double _elapsedTime = 0;
    private static int _fps = 0;
    private static DateTime[] Times = new DateTime[16];

    public static int FPS => _fps;

    public static void Update(GameTime gameTime)
    {
        _frameCounter++;
        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (_elapsedTime >= 1)
        {
            _fps = _frameCounter;
            _frameCounter = 0;
            _elapsedTime = 0;
        }
    }

    public static void StartTimer(int i)
    {
        if (i < 0 || i >= 16)
        {
            return;
        }

        Times[i] = DateTime.UtcNow;
    }

    public static double GetTime(int i = 0, int d = 2)
    {
        if (i < 0 || i >= 16)
        {
            return 0.0f;
        }

        return Math.Round((DateTime.UtcNow - Times[i]).TotalSeconds, d);
    }

    public static string GetDateTime(int i)
    {
        if (i == 1)
        {
            return DateTime.UtcNow.ToLongDateString();
        }

        if (i == 2)
        {
            return DateTime.UtcNow.ToLongTimeString();
        }

        if (i == 3)
        {
            return DateTime.UtcNow.ToShortDateString();
        }

        if (i == 4)
        {
            return DateTime.UtcNow.ToShortTimeString();
        }

        return DateTime.UtcNow.ToString();
    }
}
