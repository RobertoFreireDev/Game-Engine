using framework.Binding;
using framework.Utils;

namespace framework.Graphics;

public static class LuaError
{
    public static string Message;

    public static void Draw()
    {
        LuaBinding.RectFill(
            ScreenUtils.BaseBox.X,
            ScreenUtils.BaseBox.Y,
            ScreenUtils.BaseBox.Width,
            ScreenUtils.BaseBox.Height,
            0);
        LuaBinding.PrintMultiple(Message, 2, 2, 10, 1);
    }
}
