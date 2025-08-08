using framework.Graphics;
using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLua;
using System;
using System.IO;

namespace framework.Binding;

public class LuaBinding
{
    private string _scriptName = "gescript";

    public Lua Lua = new Lua();

    public LuaBinding(string script)
    {
        // Config functions
        Lua.RegisterFunction("inittitle", this, GetType().GetMethod("ConfigTitle"));
        Lua.RegisterFunction("initfps30", this, GetType().GetMethod("ConfigFps30"));
        Lua.RegisterFunction("initfps60", this, GetType().GetMethod("ConfigFps60"));

        // Draw functions
        Lua.RegisterFunction("pal", this, GetType().GetMethod("Pal"));
        Lua.RegisterFunction("rect", this, GetType().GetMethod("Rect"));
        Lua.RegisterFunction("rectfill", this, GetType().GetMethod("RectFill"));
        Lua.RegisterFunction("print", this, GetType().GetMethod("Print"));
        Lua.DoString(script, _scriptName);

        var initFunc = Lua.GetFunction("_init");
        if (initFunc != null)
        {
            try
            {
                initFunc.Call();
            }
            catch (Exception ex)
            {
                // Add error screen with exception message
            }
        }
    }

    public void Update()
    {

        var updateFunc = Lua.GetFunction("_update");
        if (updateFunc != null)
        {
            try
            {
                updateFunc.Call();
            }
            catch (Exception ex)
            {
                // Add error screen with exception message
            }
        }
    }

    public void Draw()
    {
        var drawFunc = Lua.GetFunction("_draw");
        if (drawFunc != null)
        {
            try
            {
                drawFunc.Call();
            }
            catch (Exception ex)
            {
                // Add error screen with exception message
            }
        }
    }

    #region ConfigFunctions
    public static void ConfigTitle(string text)
    {
        GFW.Title = text;
    }

    public static void ConfigFps30()
    {
        GFW.FPS = 30;
    }

    public static void ConfigFps60()
    {
        GFW.FPS = 60;
    }    
    #endregion

    #region DrawFunctions
    public static void Pal(string palette)
    {
        ColorUtils.SetPalette(palette);
    }

    public static void Rect(int x, int y, int width, int height, int color = 0)
    {
        Shapes.DrawRectBorder(new Rectangle(0, 0, 320, 180), ColorUtils.GetColor(color));
    }

    public static void RectFill(int x, int y, int width, int height, int color = 0)
    {
        Shapes.DrawRectFill(new Rectangle(0, 0, 320, 180), ColorUtils.GetColor(color));
    }

    public static void Print(string text, int x, int y, int color = 0)
    {
        Font.DrawText(text, new Vector2(x, y), ColorUtils.GetColor(color));
    }
    #endregion
}