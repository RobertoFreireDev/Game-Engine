using blackbox.Binding;
using blackbox.Utils;

namespace blackbox.Graphics;

public static class LuaError
{
    private static string _message;
    private static bool _error = false;

    public static bool HasError()
    {
        return _error;
    }

    public static void SetError(string message)
    {
        _error = true;
        _message = message;
    }

    public static void Draw()
    {
        LuaBinding.RectFill(
            ScreenUtils.BaseBox.X,
            ScreenUtils.BaseBox.Y,
            ScreenUtils.BaseBox.Width,
            ScreenUtils.BaseBox.Height,
            0);
        LuaBinding.Print(_message, 2, 2, 1, true);
    }
}
