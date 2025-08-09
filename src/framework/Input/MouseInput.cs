using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace framework.Input;

internal static class MouseInput
{
    private static int offsetX = 10;
    public static int Current_Cursor = 0;    

    public static void UpdateCursor(int cursor)
    {
        if (cursor < 0 || cursor > 47)
        {
            return;
        }

        Current_Cursor = cursor;
    }

    public static Vector2 MousePosition()
    {
        return new Vector2(InputStateManager.CurrentMouseState().Position.X - offsetX, InputStateManager.CurrentMouseState().Position.Y);
    }

    public static Vector2 MouseVirtualPosition()
    {
        return new Vector2(
                (int)((-ScreenUtils.BoxToDraw.X + InputStateManager.CurrentMouseState().Position.X) / ScreenUtils.ScaleX),
                (int)((-ScreenUtils.BoxToDraw.Y + InputStateManager.CurrentMouseState().Position.Y) / ScreenUtils.ScaleY)
            );
    }

    public static bool ScrollUp()
    {
        return InputStateManager.CurrentMouseState().ScrollWheelValue >
            InputStateManager.PreviousMouseState().ScrollWheelValue;
    }

    public static bool ScrollDown()
    {
        return InputStateManager.CurrentMouseState().ScrollWheelValue <
             InputStateManager.PreviousMouseState().ScrollWheelValue;
    }

    public static bool LeftButton_JustPressed()
    {
        return JustPressed(InputStateManager.CurrentMouseState().LeftButton, InputStateManager.PreviousMouseState().LeftButton);
    }

    public static bool LeftButton_Released()
    {
        return Released(InputStateManager.CurrentMouseState().LeftButton, InputStateManager.PreviousMouseState().LeftButton);
    }

    public static bool LeftButton_Pressed()
    {
        return Pressed(InputStateManager.CurrentMouseState().LeftButton);
    }

    public static bool RightButton_JustPressed()
    {
        return JustPressed(InputStateManager.CurrentMouseState().RightButton, InputStateManager.PreviousMouseState().RightButton);
    }

    public static bool RightButton_Released()
    {
        return Released(InputStateManager.CurrentMouseState().RightButton, InputStateManager.PreviousMouseState().RightButton);
    }

    public static bool RightButton_Pressed()
    {
        return Pressed(InputStateManager.CurrentMouseState().RightButton);
    }

    private static bool JustPressed(ButtonState currentButtonState, ButtonState previousButtonState)
    {
        return currentButtonState == ButtonState.Pressed && previousButtonState == ButtonState.Released;
    }

    private static bool Released(ButtonState currentButtonState, ButtonState previousButtonState)
    {
        return currentButtonState == ButtonState.Released && previousButtonState == ButtonState.Pressed;
    }

    private static bool Pressed(ButtonState currentButtonState)
    {
        return currentButtonState == ButtonState.Pressed;
    }
}