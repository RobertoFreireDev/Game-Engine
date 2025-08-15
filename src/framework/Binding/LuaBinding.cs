using blackbox.Graphics;
using blackbox.Assets;
using blackbox.Input;
using blackbox.IOFile;
using blackbox.Sfx;
using blackbox.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLua;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace blackbox.Binding;

public class LuaBinding
{
    private static Lua _lua = new Lua();
    private string _scriptName = "game";
    private static SfxPlayer _player = new SfxPlayer();

    public LuaBinding(string script)
    {
        _lua.UseTraceback = true;

        // Config
        _lua.RegisterFunction("_title", this, GetType().GetMethod("ConfigTitle"));
        _lua.RegisterFunction("_fps30", this, GetType().GetMethod("ConfigFps30"));
        _lua.RegisterFunction("_fps60", this, GetType().GetMethod("ConfigFps60"));
        _lua.RegisterFunction("_crtshader", this, GetType().GetMethod("EnableCRTshader"));
        _lua.RegisterFunction("_texture", this, GetType().GetMethod("LoadTextureFromBase64"));

        // Input
        _lua.RegisterFunction("_mouseshow", this, GetType().GetMethod("ShowHideMouse"));
        _lua.RegisterFunction("_mousepos", this, GetType().GetMethod("GetMousePos"));
        _lua.RegisterFunction("_mouseclick", this, GetType().GetMethod("MouseButtonPressed"));
        _lua.RegisterFunction("_mouseclickp", this, GetType().GetMethod("MouseButtonJustPressed"));
        _lua.RegisterFunction("_mouseclickr", this, GetType().GetMethod("MouseButtonReleased"));
        _lua.RegisterFunction("_mousescroll", this, GetType().GetMethod("Scroll"));
        _lua.RegisterFunction("_mousecursor", this, GetType().GetMethod("UpdateCursor"));

        _lua.RegisterFunction("_btn", this, GetType().GetMethod("Pressed"));
        _lua.RegisterFunction("_btnp", this, GetType().GetMethod("JustPressed"));
        _lua.RegisterFunction("_btnr", this, GetType().GetMethod("Released"));

        _lua.RegisterFunction("_isfocused", this, GetType().GetMethod("IsFocused"));

        // Draw
        _lua.RegisterFunction("_bckgdclr", this, GetType().GetMethod("ConfigBackGroundColor"));
        _lua.RegisterFunction("_pal", this, GetType().GetMethod("Pal"));
        _lua.RegisterFunction("_rect", this, GetType().GetMethod("Rect"));
        _lua.RegisterFunction("_rectfill", this, GetType().GetMethod("RectFill"));
        _lua.RegisterFunction("_circ", this, GetType().GetMethod("Circ"));
        _lua.RegisterFunction("_circfill", this, GetType().GetMethod("CircFill"));
        _lua.RegisterFunction("_line", this, GetType().GetMethod("DrawLine"));
        _lua.RegisterFunction("_pixel", this, GetType().GetMethod("DrawPixel"));
        _lua.RegisterFunction("_print", this, GetType().GetMethod("Print"));
        _lua.RegisterFunction("_spr", this, GetType().GetMethod("DrawSprite"));
        _lua.RegisterFunction("_sprc", this, GetType().GetMethod("DrawSpriteWithColor"));
        _lua.RegisterFunction("_cspr", this, GetType().GetMethod("DrawTexture"));
        _lua.RegisterFunction("_csprc", this, GetType().GetMethod("DrawTextureWithColor"));
        _lua.RegisterFunction("_cspre", this, GetType().GetMethod("DrawTextureWithEffect"));
        _lua.RegisterFunction("_camera", this, GetType().GetMethod("Camera"));

        // Status
        _lua.RegisterFunction("_sysfps", this, GetType().GetMethod("GetFps"));

        // File        
        _lua.RegisterFunction("_iohasfile", this, GetType().GetMethod("HasFile"));
        _lua.RegisterFunction("_ioread", this, GetType().GetMethod("ReadFile"));
        _lua.RegisterFunction("_iocreate", this, GetType().GetMethod("CreateFile"));
        _lua.RegisterFunction("_ioupdate", this, GetType().GetMethod("UpdateFile"));
        _lua.RegisterFunction("_iocreateorupdate", this, GetType().GetMethod("CreateOrUpdateFile"));
        _lua.RegisterFunction("_iodelete", this, GetType().GetMethod("DeleteFile"));
        _lua.RegisterFunction("_loadsfx", this, GetType().GetMethod("ReadSfx"));
        _lua.RegisterFunction("_savesfx", this, GetType().GetMethod("CreateOrUpdateSfx"));

        //Sfx
        _lua.RegisterFunction("_configsfx", this, GetType().GetMethod("ConfigSfx"));
        _lua.RegisterFunction("_playsfx", this, GetType().GetMethod("PlaySfx"));
        _lua.RegisterFunction("_stopsfx", this, GetType().GetMethod("StopSfx"));
        _lua.RegisterFunction("_validfx", this, GetType().GetMethod("ValidSfx"));

        //Time
        _lua.RegisterFunction("_stimer", this, GetType().GetMethod("StartTimer"));
        _lua.RegisterFunction("_gtimer", this, GetType().GetMethod("GetTimer"));
        _lua.RegisterFunction("_pgame", this, GetType().GetMethod("PauseGame"));
        _lua.RegisterFunction("_gtime", this, GetType().GetMethod("GetDateTime"));
        _lua.RegisterFunction("_gdeltatime", this, GetType().GetMethod("GetDeltaTime"));

        //Flags
        _lua.RegisterFunction("_gflag", this, GetType().GetMethod("GetFlag"));
        _lua.RegisterFunction("_gflags", this, GetType().GetMethod("GetAllFlags"));
        _lua.RegisterFunction("_sflag", this, GetType().GetMethod("SetFlag"));
        _lua.RegisterFunction("_sflags", this, GetType().GetMethod("SetAllFlags"));

        //Map
        _lua.RegisterFunction("_mget", this, GetType().GetMethod("GetMapValue"));
        _lua.RegisterFunction("_mset", this, GetType().GetMethod("SetMapValue"));
        _lua.RegisterFunction("_map", this, GetType().GetMethod("DrawMap"));

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
            Camera(0, 0); 
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
    public static void LoadTextureFromBase64(int tileWidth, int tileHeight, string spriteBase64)
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
        Sprites.DrawCustomSprite(i, x, y, Color.White, w, h, flipX, flipY);
    }

    public static void DrawTextureWithColor(int i, int x, int y, int index, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        Sprites.DrawCustomSprite(i, x, y, ColorUtils.GetColor(index, transparency), w, h, flipX, flipY);
    }

    public static void DrawTextureWithEffect(int i, int x, int y, string parameters = "", int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        if (string.IsNullOrWhiteSpace(parameters) || parameters.Length < 4)
        {
            return;
        }
        GFW.SpriteBatch.End();
        GFW.SpriteBatch.Begin(effect: GFW.CustomEffect, samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
        var rectangle = GameImage.GameTexture.Bounds;
        GFW.CustomEffect.Parameters["DistortX"].SetValue(SubstringToInt(parameters, 0, 1) * 0.01f);
        GFW.CustomEffect.Parameters["DistortY"].SetValue(SubstringToInt(parameters, 1, 1) * 0.01f);
        GFW.CustomEffect.Parameters["WaveFreq"].SetValue(SubstringToInt(parameters, 2, 1) * 10f);
        GFW.CustomEffect.Parameters["WaveSpeed"].SetValue(SubstringToInt(parameters, 3, 1) * 1f);
        GFW.CustomEffect.Parameters["Time"].SetValue((float)TimeUtils.ElapsedTime);
        Sprites.DrawCustomSprite(i, x, y, Color.White, w, h, flipX, flipY);
        GFW.SpriteBatch.End();
        GFW.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
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

    public static void EnableCRTshader(bool value, int inner = 85, int outer = 110)
    {
        GFW.EnableCRTshader(value, inner, outer);
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

        return Input.KeyboardInput.JustPressed((Keys)keyNumber);
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
    public static void ShowHideMouse(bool value)
    {
        GFW.ShowHideMouse(value);
    }

    public static void Pal(string palette)
    {
        ColorUtils.SetPalette(palette);
    }

    public static void Rect(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawRectBorder(new Rectangle(x, y, width, height), ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void RectFill(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawRectFill(new Rectangle(x, y, width, height), ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void Circ(int x, int y, int r, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawCirc(x, y, r, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void CircFill(int x, int y, int r, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawCircFill(x, y, r, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawLine(int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawLine(x0, y0, x1, y1, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawPixel(int x, int y, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawPixel(x, y, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void Print(string text, int x, int y, int colorIndex = 0, bool wraptext = false, int wrapLimit = 0)
    {
        Font.DrawText(text, new Vector2(x, y), ColorUtils.GetColor(colorIndex), wraptext, wrapLimit);
    }

    public static void DrawSprite(int i, int x, int y, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        Sprites.DrawSprite(i, x, y, Color.White, w, h, flipX, flipY);
    }

    public static void DrawSpriteWithColor(int i, int x, int y, int index, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        Sprites.DrawSprite(i, x, y, ColorUtils.GetColor(index, transparency), w, h, flipX, flipY);
    }

    public static void Camera(float x = 0.0f, float y = 0.0f)
    {
        Camera2D.Camera(x, y);
    }
    #endregion

    #region SystemFunctions
    public static int GetFps()
    {
        return TimeUtils.FPS;
    }

    public static bool IsFocused()
    {
        return ScreenUtils.IsFocused;
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

    public static void CreateOrUpdateFile(string fileName, string content)
    {
        TxtFileIO.CreateOrUpdate(fileName, content);
    }

    public static void ReadSfx()
    {
        if (!HasFile(Constants.Sfxfilename))
        {
            return;
        }
        var content = TxtFileIO.Read(Constants.Sfxfilename);
        _player.ConvertStringToData(content);
    }

    public static void CreateOrUpdateSfx()
    {
        var content = _player.ConvertDataToString();
        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }
        TxtFileIO.CreateOrUpdate(Constants.Sfxfilename, content);
    }
    #endregion

    #region SfxFunctions
    public static void ConfigSfx(int index, int speed, string sound)
    {
        sound += Math.Clamp(speed, Constants.MinSpeed, Constants.MaxSpeed).ToString("D2");
        _player.SetSfx(index, sound);
    }

    public static bool ValidSfx(string sound)
    {
        return _player.ValidateSoundString(sound);
    }

    public static void PlaySfx(int index, int speed = 1, int channel = -1, int offset = 0)
    {
        _player.PlaySfx(index, speed, channel, offset);
    }

    public static void StopSfx(int index)
    {
        _player.Stop(index);
    }
    #endregion

    #region TimerFunctions
    public static void StartTimer(int i = 0)
    {
        TimeUtils.StartTimer(i);
    }

    public static double GetTimer(int i = 0, int d = 4)
    {
        return TimeUtils.GetTime(i, d);
    }

    public static void PauseGame(bool value)
    {
        GFW.PauseGame(value);
    }

    public static string GetDateTime(int i = 0)
    {
        return TimeUtils.GetDateTime(i);
    }

    public static double GetDeltaTime()
    {
        return TimeUtils.Delta;
    }
    #endregion

    #region MapFunctions
    public static bool GetFlag(int tileIndex, int flag = -1)
    {
        return Map.FGet(tileIndex, flag);
    }

    public static byte GetAllFlags(int tileIndex)
    {
        return Map.FGetByte(tileIndex);
    }

    public static void SetFlag(int tileIndex, int flag, bool value)
    {
        Map.FSet(tileIndex, flag, value);
    }

    public static void SetAllFlags(int tileIndex, byte value)
    {
        Map.FSetByte(tileIndex, value);
    }

    public static int GetMapValue(int x, int y)
    {
        return Map.GetMapValue(x, y);
    }

    public static void SetMapValue(int x, int y, byte v)
    {
        Map.SetMapValue(x, y, v);
    }
    
    public static void DrawMap(int tileSize, int colorIndex, int cel_x, int cel_y, int sx, int sy, int cel_w, int cel_h)
    {
        Map.DrawMap(tileSize, ColorUtils.GetColor(colorIndex), cel_x, cel_y, sx, sy, cel_w, cel_h);
    }
    #endregion

    #region Utils
    public static int SubstringToInt(string source, int start, int length)
    {
        try
        {
            return int.Parse(source.AsSpan(start, length));
        }
        catch
        {
            LuaError.SetError($"Error Parsing string to int: {source}");
        }

        return 0;
    }
    #endregion
}