using framework.Graphics;
using framework.Input;
using framework.Utils;
using Microsoft.Xna.Framework;
using NLua;
using System;

namespace framework.Binding;

public class LuaBinding
{
    private static Lua _lua = new Lua();
    private string _scriptName = "game";
    private bool _error = false;

    public LuaBinding(string script)
    {
        _lua.UseTraceback = true;
        // Config
        _lua.RegisterFunction("inittitle", this, GetType().GetMethod("ConfigTitle"));
        _lua.RegisterFunction("initbckgdclr", this, GetType().GetMethod("ConfigBackGroundColor"));
        _lua.RegisterFunction("initfps30", this, GetType().GetMethod("ConfigFps30"));
        _lua.RegisterFunction("initfps60", this, GetType().GetMethod("ConfigFps60"));

        // Input 
        _lua.RegisterFunction("mouse", this, GetType().GetMethod("GetMousePos"));

        // Draw
        _lua.RegisterFunction("pal", this, GetType().GetMethod("Pal"));
        _lua.RegisterFunction("rect", this, GetType().GetMethod("Rect"));
        _lua.RegisterFunction("rectfill", this, GetType().GetMethod("RectFill"));
        _lua.RegisterFunction("print", this, GetType().GetMethod("Print"));

        // Status
        _lua.RegisterFunction("sysfps", this, GetType().GetMethod("GetFps"));

        try
        {
            _lua.DoString(script, _scriptName);
        }
        catch (Exception ex)
        {
            _error = true;
            LuaError.Message = ex.Message;
        }

        if (_error)
        {
            return;
        }

        try
        {
            var initFunc = _lua.GetFunction("_init");
            if (initFunc != null)
            {
                initFunc.Call();
            }
        }
        catch (Exception ex)
        {
            _error = true;
            LuaError.Message = ex.Message;
        }
    }

    public void Update()
    {
        if (_error)
        {
            return;
        }

        try
        {
            var updateFunc = _lua.GetFunction("_update");
            if (updateFunc != null)
            {
                updateFunc.Call();
            }
        }
        catch (Exception ex)
        {
            _error = true;
            LuaError.Message = ex.Message;
        }
    }

    public void Draw()
    {
        if (_error)
        {
            LuaError.Draw();
            return;
        }

        try
        {
            var drawFunc = _lua.GetFunction("_draw");
            if (drawFunc != null)
            {
                drawFunc.Call();
            }
        }
        catch (Exception ex)
        {
            _error = true;
            LuaError.Message = ex.Message;
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

    #region Input
    public static LuaTable GetMousePos()
    {
        var mousepos = MouseInput.MouseVirtualPosition();
        LuaTable table = _lua.DoString("return {}")[0] as LuaTable;
        table["x"] = mousepos.X;
        table["y"] = mousepos.Y;

        return table;
    }
    # endregion

    #region DrawFunctions
    public static void Pal(string palette)
    {
        ColorUtils.SetPalette(palette);
    }

    public static void Rect(int x, int y, int width, int height, int color = 0)
    {
        Shapes.DrawRectBorder(new Rectangle(x, y, width, height), ColorUtils.GetColor(color));
    }

    public static void RectFill(int x, int y, int width, int height, int color = 0)
    {
        Shapes.DrawRectFill(new Rectangle(x, y, width, height), ColorUtils.GetColor(color));
    }

    public static void Print(string text, int x, int y, int color = 0, bool wraptext = false, int wrapLimit = 0)
    {
        Font.DrawText(text, new Vector2(x, y), ColorUtils.GetColor(color), wraptext, wrapLimit);
    }
    #endregion

    #region SystemFunctions
    public static int GetFps()
    {
        return FPSUtils.FPS;
    }
    #endregion
}