using framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace framework.Graphics;

public static class Mouse
{
    public static void DrawMouse(this SpriteBatch spriteBatch)
    {
        if (MouseInput.Current_Cursor == MouseInput.Pointer_mouse)
        {
            spriteBatch.Draw(GFW.Textures["pointer_mouse"], MouseInput.MousePosition(), Color.White);
        }
        else
        {
            spriteBatch.Draw(GFW.Textures["contextmenu_mouse"], MouseInput.MousePosition(), Color.White);
        }
    }
}