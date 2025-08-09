using framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace framework.Graphics;

public static class Mouse
{
    public static void DrawMouse(this SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(GFW.MouseTextures[MouseInput.Current_Cursor], MouseInput.MousePosition(), Color.White);
    }
}