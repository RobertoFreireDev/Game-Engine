using framework.Graphics;
using framework.Input;
using framework.Utils;
using Microsoft.Xna.Framework;
using NLua;
using System;

namespace framework.Binding;

public class LuaBinding
{
    private Lua Lua = new Lua();
    private string _scriptName = "game";
    private bool _error = false;

    public LuaBinding(string script)
    {
        Lua.UseTraceback = true;
        // Config
        Lua.RegisterFunction("inittitle", this, GetType().GetMethod("ConfigTitle"));
        Lua.RegisterFunction("initbckgdclr", this, GetType().GetMethod("ConfigBackGroundColor"));
        Lua.RegisterFunction("initfps30", this, GetType().GetMethod("ConfigFps30"));
        Lua.RegisterFunction("initfps60", this, GetType().GetMethod("ConfigFps60"));

        // Input 
        Lua.RegisterFunction("mouse", this, GetType().GetMethod("GetMousePos"));

        // Draw
        Lua.RegisterFunction("pal", this, GetType().GetMethod("Pal"));
        Lua.RegisterFunction("rect", this, GetType().GetMethod("Rect"));
        Lua.RegisterFunction("rectfill", this, GetType().GetMethod("RectFill"));
        Lua.RegisterFunction("print", this, GetType().GetMethod("Print"));

        // Status
        Lua.RegisterFunction("sysfps", this, GetType().GetMethod("GetFps"));

        try
        {
            Lua.DoString(script, _scriptName);
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
            var initFunc = Lua.GetFunction("_init");
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
            var updateFunc = Lua.GetFunction("_update");
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
            var drawFunc = Lua.GetFunction("_draw");
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
    public static double[] GetMousePos()
    {
        return ConvertVectorToDouble(MouseInput.MouseVirtualPosition());
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

    #region Utils
    private static double[] ConvertVectorToDouble(Vector2 vector)
    {
        return new double[] { (double)vector.X, (double)vector.Y };
    }
    #endregion
}