using Microsoft.Xna.Framework.Input;

namespace framework.Input;

public static class KeyboardInput
{
    public static bool IsAltF4Pressed()
    {
        return Pressed(Keys.LeftAlt) && Pressed(Keys.F4);
    }

    public static bool IsF2Released()
    {
        return Released(Keys.F2);
    }

    private static bool JustPressed(Keys key)
    {
        return InputStateManager.CurrentKeyboardState()[key] == KeyState.Down && InputStateManager.PreviousKeyboardState()[key] == KeyState.Up;
    }

    private static bool Released(Keys key)
    {
        return InputStateManager.CurrentKeyboardState()[key] == KeyState.Up && InputStateManager.PreviousKeyboardState()[key] == KeyState.Down;
    }

    private static bool Pressed(Keys currentKey)
    {
        return InputStateManager.CurrentKeyboardState()[currentKey] == KeyState.Down;
    }
}
