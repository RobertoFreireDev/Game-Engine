using framework.Graphics;
using framework.Utils;
using Microsoft.Xna.Framework;
using NLua;
using System;

namespace framework.Binding;

public class LuaBinding
{
    private Lua Lua = new Lua();
    private string _scriptName = "gescript";
    private bool _error = false;

    public LuaBinding(string script)
    {
        Lua.UseTraceback = true;
        // Config functions
        Lua.RegisterFunction("inittitle", this, GetType().GetMethod("ConfigTitle"));
        Lua.RegisterFunction("initbckgdclr", this, GetType().GetMethod("ConfigBackGroundColor"));
        Lua.RegisterFunction("initfps30", this, GetType().GetMethod("ConfigFps30"));
        Lua.RegisterFunction("initfps60", this, GetType().GetMethod("ConfigFps60"));

        // Draw functions
        Lua.RegisterFunction("pal", this, GetType().GetMethod("Pal"));
        Lua.RegisterFunction("rect", this, GetType().GetMethod("Rect"));
        Lua.RegisterFunction("rectfill", this, GetType().GetMethod("RectFill"));
        Lua.RegisterFunction("print", this, GetType().GetMethod("Print"));

        // Status functions
        Lua.RegisterFunction("sysfps", this, GetType().GetMethod("GetFps"));

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
                _error = true;
                LuaError.Message = ex.Message;
            }
        }
    }

    public void Update()
    {

        var updateFunc = Lua.GetFunction("_update");
        if (updateFunc != null && !_error)
        {
            try
            {
                updateFunc.Call();
            }
            catch (Exception ex)
            {
                _error = true;
                LuaError.Message = ex.Message;
            }
        }
    }

    public void Draw()
    {
        var drawFunc = Lua.GetFunction("_draw");

        if (_error)
        {
            LuaError.Draw();
        }
        else if (drawFunc != null)
        {
            try
            {
                drawFunc.Call();
            }
            catch (Exception ex)
            {
                _error = true;
                LuaError.Message = ex.Message;
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
    
    public static void ConfigBackGroundColor(int colorIndex)
    {
        GFW.BackgroundColor = colorIndex;
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

    public static void PrintMultiple(string text, int x, int y, int lineHeight, int color = 0)
    {
        Font.DrawTextMultiLine(text, x, y, lineHeight, ColorUtils.GetColor(color));
    }

    public static void Print(string text, int x, int y, int color = 0)
    {
        Font.DrawText(text, new Vector2(x, y), ColorUtils.GetColor(color));
    }
    #endregion

    #region System info
    public static int GetFps()
    {
        return FPSUtils.FPS;
    }
    #endregion
}