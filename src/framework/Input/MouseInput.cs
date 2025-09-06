﻿using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace blackbox.Input;

internal static class MouseInput
{
    public static int Current_Cursor = 0;    

    public static void UpdateCursor(int cursor)
    {
        if (cursor < 0 || cursor > 1)
        {
            return;
        }

        Current_Cursor = cursor;
    }

    public static Vector2 MouseVirtualPosition(int offsetX = 0)
    {
        return new Vector2(
                (int)((-ScreenUtils.BoxToDraw.X + InputStateManager.CurrentMouseState().Position.X - offsetX * ScreenUtils.ScaleX) / ScreenUtils.ScaleX),
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