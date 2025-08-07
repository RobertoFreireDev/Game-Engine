using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace framework.Input;

internal static class MouseInput
{
    public const int Context_Menu_mouse = 0;
    public const int Pointer_mouse = 1;
    private static int offsetX = 10;
    public static MouseCursor ContextMenuCursor = MouseCursor.FromTexture2D(GFW.Textures["contextmenu_mouse"], offsetX, 0);
    public static MouseCursor PointerCursor = MouseCursor.FromTexture2D(GFW.Textures["pointer_mouse"], offsetX, 0);

    public static void TryUpdateStatus(int status)
    {
        switch (status)
        {
            case Context_Menu_mouse:
                Mouse.SetCursor(ContextMenuCursor);
                break;
            case Pointer_mouse:
                Mouse.SetCursor(PointerCursor);
                break;
        }
    }

    public static Vector2 MousePosition()
    {
        return new Vector2(InputStateManager.CurrentMouseState().Position.X - offsetX, InputStateManager.CurrentMouseState().Position.Y);
    }

    public static Point MouseVirtualPosition()
    {
        return new Point(
                (int)((-ScreenUtils.BoxToDraw.X + InputStateManager.CurrentMouseState().Position.X) / ScreenUtils.ScaleX),
                (int)((-ScreenUtils.BoxToDraw.Y + InputStateManager.CurrentMouseState().Position.Y) / ScreenUtils.ScaleY)
            );
    }

    public static int GetScrollWheelValue()
    {
        return InputStateManager.CurrentMouseState().ScrollWheelValue;
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