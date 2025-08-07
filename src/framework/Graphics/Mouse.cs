using framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace framework.Graphics;

public static class Mouse
{
    public static void DrawMouse(this SpriteBatch spriteBatch, int status = 0)
    {
        var mousePos = MouseInput.MousePosition();
        if (status == MouseInput.Context_Menu_mouse)
        {
            spriteBatch.Draw(GFW.Textures["contextmenu_mouse"], mousePos, Color.White);
        }
        else 
        {
            spriteBatch.Draw(GFW.Textures["pointer_mouse"], mousePos, Color.White);
        }
    }
}
