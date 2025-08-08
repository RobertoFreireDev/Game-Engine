using framework.Graphics;
using framework.Input;
using framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLua;
using System;

namespace framework.Binding;

public class LuaBinding
{
    private static Lua _lua = new Lua();
    private string _scriptName = "game";

    public LuaBinding(string script)
    {
        _lua.UseTraceback = true;
        // Config
        _lua.RegisterFunction("inittitle", this, GetType().GetMethod("ConfigTitle"));
        _lua.RegisterFunction("initbckgdclr", this, GetType().GetMethod("ConfigBackGroundColor"));
        _lua.RegisterFunction("initfps30", this, GetType().GetMethod("ConfigFps30"));
        _lua.RegisterFunction("initfps60", this, GetType().GetMethod("ConfigFps60"));

        // Input 
        _lua.RegisterFunction("mousepos", this, GetType().GetMethod("GetMousePos"));
        _lua.RegisterFunction("mouseclick", this, GetType().GetMethod("MouseButtonPressed"));
        _lua.RegisterFunction("mouseclickp", this, GetType().GetMethod("MouseButtonJustPressed"));
        _lua.RegisterFunction("mouseclickr", this, GetType().GetMethod("MouseButtonReleased"));
        _lua.RegisterFunction("mousescroll", this, GetType().GetMethod("Scroll"));
        _lua.RegisterFunction("mousecursor", this, GetType().GetMethod("UpdateCursor"));

        _lua.RegisterFunction("btn", this, GetType().GetMethod("Pressed"));
        _lua.RegisterFunction("btnp", this, GetType().GetMethod("JustPressed"));
        _lua.RegisterFunction("btnr", this, GetType().GetMethod("Released"));

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
            LuaError.SetError(ex.Message);
        }
        
        if (LuaError.HasError())
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
            LuaError.SetError(ex.Message);
        }
    }

    public void Update()
    {
        if (LuaError.HasError())
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
            LuaError.SetError(ex.Message);
        }
    }

    public void Draw()
    {
        if (LuaError.HasError())
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
            LuaError.SetError(ex.Message);
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

    public static bool MouseButtonPressed(int i)
    {
        if (i == 1)
        {
            return MouseInput.RightButton_Pressed();
        }
        else
        {
            return MouseInput.LeftButton_Pressed();
        }
    }

    public static bool MouseButtonJustPressed(int i)
    {
        if (i == 1)
        {
            return MouseInput.RightButton_JustPressed();
        }
        else
        {
            return MouseInput.LeftButton_JustPressed();
        }
    }

    public static bool MouseButtonReleased(int i)
    {
        if (i == 1)
        {
            return MouseInput.RightButton_Released();
        }
        else
        {
            return MouseInput.LeftButton_Released();
        }
    }

    public static bool Scroll(int i)
    {
        if (i == 1)
        {
            return MouseInput.ScrollUp();
        }
        else
        {
            return MouseInput.ScrollDown();
        }
    }

    public static void UpdateCursor(int i)
    {
        if (i == 1)
        {
            MouseInput.UpdateCursor(MouseInput.Pointer_mouse);
        }
        else
        {
            MouseInput.UpdateCursor(MouseInput.Context_Menu_mouse);
        }
    }

    public static bool JustPressed(int keyNumber)
    {
        if (!Enum.IsDefined(typeof(Keys), keyNumber))
        {
            return false;
        }

        return Input.KeyboardInput.JustPressed((Keys) keyNumber);
    }

    public static bool Released(int keyNumber)
    {
        if (!Enum.IsDefined(typeof(Keys), keyNumber))
        {
            return false;
        }

        return Input.KeyboardInput.Released((Keys)keyNumber);
    }

    public static bool Pressed(int keyNumber)
    {
        if (!Enum.IsDefined(typeof(Keys), keyNumber))
        {
            return false;
        }

        return Input.KeyboardInput.Pressed((Keys)keyNumber);
    }
    #endregion

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