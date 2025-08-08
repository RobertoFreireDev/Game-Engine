using Microsoft.Xna.Framework;

namespace framework.Utils;

public static class FPSUtils
{
    private static int _frameCounter = 0;
    private static double _elapsedTime = 0;
    private static int _fps = 0;
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
}
