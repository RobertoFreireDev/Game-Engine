using blackbox.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Graphics;

public static class Mouse
{
    public static void DrawMouse(this SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(GFW.MouseTextures[MouseInput.Current_Cursor], MouseInput.MouseToDrawPosition(), Color.White);
    }
}