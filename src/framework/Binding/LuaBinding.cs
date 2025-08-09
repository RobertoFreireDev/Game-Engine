using framework.Assets;
using framework.Graphics;
using framework.Input;
using framework.IOFile;
using framework.Sfx;
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
        _lua.RegisterFunction("title", this, GetType().GetMethod("ConfigTitle"));
        _lua.RegisterFunction("fps30", this, GetType().GetMethod("ConfigFps30"));
        _lua.RegisterFunction("fps60", this, GetType().GetMethod("ConfigFps60"));
        _lua.RegisterFunction("texture", this, GetType().GetMethod("LoadTextureFromBase64"));

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
        _lua.RegisterFunction("bckgdclr", this, GetType().GetMethod("ConfigBackGroundColor"));
        _lua.RegisterFunction("pal", this, GetType().GetMethod("Pal"));
        _lua.RegisterFunction("rect", this, GetType().GetMethod("Rect"));
        _lua.RegisterFunction("rectfill", this, GetType().GetMethod("RectFill"));
        _lua.RegisterFunction("print", this, GetType().GetMethod("Print"));
        _lua.RegisterFunction("spr", this, GetType().GetMethod("DrawTexture"));

        // Status
        _lua.RegisterFunction("sysfps", this, GetType().GetMethod("GetFps"));

        // File        
        _lua.RegisterFunction("iohasfile", this, GetType().GetMethod("HasFile"));
        _lua.RegisterFunction("ioread", this, GetType().GetMethod("ReadFile"));
        _lua.RegisterFunction("iocreate", this, GetType().GetMethod("CreateFile"));
        _lua.RegisterFunction("ioupdate", this, GetType().GetMethod("UpdateFile"));
        _lua.RegisterFunction("iodelete", this, GetType().GetMethod("DeleteFile"));

        //Sfx
        _lua.RegisterFunction("sfx", this, GetType().GetMethod("PlaySfx"));

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

    #region TextureFunctions
    public static void LoadTextureFromBase64(string spriteBase64, int tileWidth, int tileHeight)
    {
        try
        {
            GameImage.LoadTexture(spriteBase64, tileWidth, tileHeight);
        }
        catch (Exception ex)
        {
            LuaError.SetError(ex.Message);
        }
    }

    public static void DrawTexture(int i, int x, int y, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        Sprites.DrawSprite(i, x, y, w, h, flipX, flipY);
    }
    #endregion

    #region InitFunctions
    public static void ConfigTitle(string text)
    {
        GFW.UpdateTitle(text);
    }

    public static void ConfigFps30()
    {
        GFW.UpdateFPS(30);
    }

    public static void ConfigFps60()
    {
        GFW.UpdateFPS(60);
    }
    
    public static void ConfigBackGroundColor(int colorIndex)
    {
        GFW.BackgroundColor = colorIndex;
    }
    #endregion

    #region InputFunctions
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
        MouseInput.UpdateCursor(i);
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

    public static void Rect(int x, int y, int width, int height, int colorIndex = 0)
    {
        Shapes.DrawRectBorder(new Rectangle(x, y, width, height), ColorUtils.GetColor(colorIndex));
    }

    public static void RectFill(int x, int y, int width, int height, int colorIndex = 0)
    {
        Shapes.DrawRectFill(new Rectangle(x, y, width, height), ColorUtils.GetColor(colorIndex));
    }

    public static void Print(string text, int x, int y, int colorIndex = 0, bool wraptext = false, int wrapLimit = 0)
    {
        Font.DrawText(text, new Vector2(x, y), ColorUtils.GetColor(colorIndex), wraptext, wrapLimit);
    }
    #endregion

    #region SystemFunctions
    public static int GetFps()
    {
        return FPSUtils.FPS;
    }
    #endregion

    #region IOFileFunctions
    public static bool HasFile(string fileName)
    {
        return TxtFileIO.HasFile(fileName);
    }

    public static string ReadFile(string fileName)
    {
        return TxtFileIO.Read(fileName);
    }

    public static void CreateFile(string fileName, string content)
    {
        TxtFileIO.Create(fileName, content);
    }

    public static void UpdateFile(string fileName, string content)
    {
        TxtFileIO.Update(fileName, content);
    }

    public static void DeleteFile(string fileName)
    {
        TxtFileIO.Delete(fileName);
    }
    #endregion

    #region SfxFunctions
    public static void PlaySfx(int n)
    {
        Random rand = new Random();
        Waveform[] values = (Waveform[]) Enum.GetValues(typeof(Waveform));
        var player = new SfxPlayer();

        var beep = new SfxData();
        for (int i = 0; i < 32; i++)
        {
            int randomIndex = rand.Next(2) + 2;
            beep.Notes[i] = new Note
            {
                Pitch = 60 + (i % 4),
                Wave = values[randomIndex],
                Volume = 0.5f
            };
        }
        beep.Speed = 0.1f;
        player.Sfx(beep);
    }
    #endregion
}