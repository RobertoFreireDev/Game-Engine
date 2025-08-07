using framework.Graphics;
using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLua;
using System.IO;

namespace framework.Binding;

public class LuaBinding
{
    private string _scriptName = "gescript";

    public Lua Lua = new Lua();

    public LuaBinding(string script)
    {
        // Draw functions
        Lua.RegisterFunction("rect", this, GetType().GetMethod("Rect"));
        Lua.RegisterFunction("rectfill", this, GetType().GetMethod("RectFill"));
        Lua.RegisterFunction("print", this, GetType().GetMethod("Print"));
        Lua.DoString(script, _scriptName);

        // Call the _init function in Lua
        var updateFunc = Lua.GetFunction("_init");
        if (updateFunc != null)
        {
            updateFunc.Call();
        }
    }

    public void Update()
    {

        var updateFunc = Lua.GetFunction("_update");
        if (updateFunc != null)
        {
            updateFunc.Call();
        }
    }

    public void Draw()
    {
        var drawFunc = Lua.GetFunction("_draw");
        if (drawFunc != null)
        {
            drawFunc.Call();
        }
    }

    #region DrawFunctions
    public static void Rect(int x, int y, int width, int height, int color)
    {
        Shapes.DrawRectBorder(new Rectangle(0, 0, 320, 180), Color.BurlyWood);
    }

    public static void RectFill(int x, int y, int width, int height, int color)
    {
        Shapes.DrawRectFill(new Rectangle(0, 0, 320, 180), Color.DarkGray);
    }

    public static void Print(string text, int x, int y)
    {
        Font.DrawText(text, new Vector2(x, y), Color.Black);
    }
    #endregion
}