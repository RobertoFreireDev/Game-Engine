using Microsoft.Xna.Framework.Input;

namespace blackbox.Input;

public static class InputStateManager
{
    private static KeyboardState _currentKeyboardState;
    private static KeyboardState _previousKeyboardState;
    private static MouseState _currentMouseState;
    private static MouseState _previousMouseState;

    public static KeyboardState CurrentKeyboardState()
    {
        return _currentKeyboardState;
    }

    public static KeyboardState PreviousKeyboardState()
    {
        return _previousKeyboardState;
    }

    public static MouseState CurrentMouseState()
    {
        return _currentMouseState;
    }

    public static MouseState PreviousMouseState()
    {
        return _previousMouseState;
    }

    public static void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }
}
