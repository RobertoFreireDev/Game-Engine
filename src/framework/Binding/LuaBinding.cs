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
    private static Lua _lua;
    private string _scriptName = "main";
    private static SfxPlayer _player;

    public LuaBinding(string script)
    {
        _lua = new Lua();
        _player = new SfxPlayer();
        _lua.UseTraceback = true;

        // Config
        _lua.RegisterFunction("_title", this, GetType().GetMethod("ConfigTitle"));
        _lua.RegisterFunction("_fps30", this, GetType().GetMethod("ConfigFps30"));
        _lua.RegisterFunction("_fps60", this, GetType().GetMethod("ConfigFps60"));
        _lua.RegisterFunction("_crtshader", this, GetType().GetMethod("EnableCRTshader"));
        _lua.RegisterFunction("_texture", this, GetType().GetMethod("LoadTextureFromBase64"));
        _lua.RegisterFunction("_reboot", this, GetType().GetMethod("ResetMainFile"));        

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
        _lua.RegisterFunction("_rect", this, GetType().GetMethod("DrawRect"));
        _lua.RegisterFunction("_rectfill", this, GetType().GetMethod("DrawRectFill"));
        _lua.RegisterFunction("_rect2", this, GetType().GetMethod("DrawRect2"));
        _lua.RegisterFunction("_rectfill2", this, GetType().GetMethod("DrawRectFill2"));
        _lua.RegisterFunction("_circ", this, GetType().GetMethod("DrawCirc"));      
        _lua.RegisterFunction("_circfill", this, GetType().GetMethod("DrawCircFill"));
        _lua.RegisterFunction("_circ2", this, GetType().GetMethod("DrawCirc2"));
        _lua.RegisterFunction("_circfill2", this, GetType().GetMethod("DrawCircFill2"));
        _lua.RegisterFunction("_line", this, GetType().GetMethod("DrawLine"));
        _lua.RegisterFunction("_pixel", this, GetType().GetMethod("DrawPixel"));
        _lua.RegisterFunction("_print", this, GetType().GetMethod("Print"));
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
        _lua.RegisterFunction("_gelapsedtime", this, GetType().GetMethod("GetElapsedTime"));

        // Grid
        _lua.RegisterFunction("_ngrid", this, GetType().GetMethod("CreateGrid"));
        _lua.RegisterFunction("_cgrid", this, GetType().GetMethod("CopyGrid"));
        _lua.RegisterFunction("_pgrid", this, GetType().GetMethod("PasteGrid"));
        _lua.RegisterFunction("_sgrid", this, GetType().GetMethod("SetGrid"));
        _lua.RegisterFunction("_ugrid", this, GetType().GetMethod("UndoGrid"));
        _lua.RegisterFunction("_rgrid", this, GetType().GetMethod("RedoGrid"));
        _lua.RegisterFunction("_bgrid", this, GetType().GetMethod("PaintBucket"));
        _lua.RegisterFunction("_ggrid", this, GetType().GetMethod("GetGrid"));
        _lua.RegisterFunction("_spixel", this, GetType().GetMethod("SetPixel"));
        _lua.RegisterFunction("_slinegrid", this, GetType().GetMethod("SetLine"));
        _lua.RegisterFunction("_srectgrid", this, GetType().GetMethod("SetRect"));
        _lua.RegisterFunction("_scircgrid", this, GetType().GetMethod("SetCirc"));
        _lua.RegisterFunction("_gpixel", this, GetType().GetMethod("GetPixel"));
        _lua.RegisterFunction("_dgrid", this, GetType().GetMethod("DrawCustomGrid"));

        // Map
        _lua.RegisterFunction("_stilemap", this, GetType().GetMethod("SetTileInMap"));
        _lua.RegisterFunction("_cmap", this, GetType().GetMethod("CreateMap"));
        _lua.RegisterFunction("_gmap", this, GetType().GetMethod("GetMap"));
        _lua.RegisterFunction("_smap", this, GetType().GetMethod("SetMap"));
        _lua.RegisterFunction("_drawmap", this, GetType().GetMethod("DrawMap"));

        try
        {
            _lua.DoString(script, _scriptName);
        }
        catch (Exception ex)
        {
            LuaError.SetError(ex);
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
            LuaError.SetError(ex);
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
            LuaError.SetError(ex);
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
            LuaError.SetError(ex);
        }
    }

    #region TextureFunctions
    public static void LoadTextureFromBase64(int index, int tileWidth, int tileHeight, string spriteBase64)
    {
        GameImage.LoadTexture(index, spriteBase64, tileWidth, tileHeight);
    }

    public static void DrawTexture(int index, int i, int x, int y, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        GameImage.DrawCustomSprite(index, i, x, y, Color.White, w, h, flipX, flipY);
    }
   
    public static void DrawTextureWithColor(int index, int i, int x, int y, int colorIndex = 0, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        GameImage.DrawCustomSprite(index, i, x, y, ColorUtils.GetColor(colorIndex, transparency), w, h, flipX, flipY);
    }

    public static void DrawTextureWithEffect(int index, int i, int x, int y, double time, string parameters = "00000000", int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)
    {
        if (string.IsNullOrWhiteSpace(parameters) || parameters.Length < 8)
        {
            return;
        }
        GFW.SpriteBatch.End();
        var color = colorIndex < 0 ? new Vector4(1, 1, 1, 1) : ColorUtils.GetColor(colorIndex, transparency).ToVector4();
        GFW.CustomEffect.Parameters["Time"].SetValue((float)time);
        GFW.CustomEffect.Parameters["DistortX"].SetValue(SubstringToInt(parameters, 0, 1) * 0.01f);
        GFW.CustomEffect.Parameters["DistortY"].SetValue(SubstringToInt(parameters, 1, 1) * 0.01f);
        GFW.CustomEffect.Parameters["WaveFreq"].SetValue(SubstringToInt(parameters, 2, 1) * 10f);
        GFW.CustomEffect.Parameters["WaveSpeed"].SetValue(SubstringToInt(parameters, 3, 1) * 1f);
        GFW.CustomEffect.Parameters["ScrollX"].SetValue(SubstringToInt(parameters, 4, 1) * 0.02f);
        GFW.CustomEffect.Parameters["ScrollY"].SetValue(SubstringToInt(parameters, 5, 1) * 0.02f);
        GFW.CustomEffect.Parameters["OutlineColor"].SetValue(color);
        GFW.CustomEffect.Parameters["OutlineThickness"].SetValue(SubstringToInt(parameters, 6, 1) * 0.001f);
        GFW.CustomEffect.Parameters["TintColor"].SetValue(color);
        GFW.CustomEffect.Parameters["NoiseAmount"].SetValue(SubstringToInt(parameters, 7, 1) * 0.05f);

        GFW.SpriteBatch.Begin(effect: GFW.CustomEffect, samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
        GameImage.DrawCustomSprite(index, i, x, y, Color.White, w, h, flipX, flipY);
        GFW.SpriteBatch.End();
        GFW.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewMatrix());
    }
    #endregion

    #region MapFunctions
    public static void CreateMap(int index, int columns, int rows, int size)
    {
        MapGrid.Create(index, columns, rows, size);
    }

    public static void SetTileInMap(int index, int x, int y, int tileIndex = 0)
    {
        MapGrid.SetTile(index, x, y, tileIndex);
    }
    
    public static void DrawMap(
        int index, int mapX, int mapY,
        int x, int y,   
        int width, int height)
    {
        MapGrid.DrawMap(index, mapX, mapY, x, y, width, height, Color.White);
    }

    public static string GetMap(int index)
    {
        return MapGrid.GetMap(index);
    }

    public static void SetMap(int index, string grid)
    {
        if (string.IsNullOrWhiteSpace(grid))
        {
            return;
        }

        MapGrid.SetMap(index, grid);
    }
    #endregion

    #region GridFunctions
    public static void CreateGrid(int gridIndex, int columns, int rows, int size, bool enableUndoRedo = false)
    {
        GameGrid.Create(gridIndex, columns, rows, size, enableUndoRedo);
    }

    public static void UndoGrid(int gridIndex)
    {
        GameGrid.Undo(gridIndex);
    }

    public static void RedoGrid(int gridIndex)
    {
        GameGrid.Redo(gridIndex);
    }

    public static void CopyGrid(int gridIndex, int x, int y, int w, int h)
    {
        GameGrid.Copy(gridIndex, x, y, w, h);
    }

    public static void PasteGrid(int gridIndex, int x, int y, int w, int h)
    {
        GameGrid.Paste(gridIndex, x, y, w, h);
    }

    public static void SetGrid(int gridIndex, string grid)
    {
        if (string.IsNullOrWhiteSpace(grid))
        {
            return;
        }

        GameGrid.SetGameGrid(gridIndex, grid);
    }

    public static string GetGrid(int gridIndex)
    {
        return GameGrid.GetGameGrid(gridIndex);
    }

    public static void SetPixel(int gridIndex, int x, int y, int colorIndex = -1)
    {
        GameGrid.SetPixel(gridIndex, x, y,colorIndex);
    }

    public static void PaintBucket(int gridIndex, int sx, int sy, int x, int y, int w, int h, int colorIndex = -1)
    {
        GameGrid.PaintBucket(gridIndex, sx, sy, x, y, w, h, colorIndex);
    }

    public static void SetLine(int gridIndex, int x0, int y0, int x1, int y1, int colorIndex = -1)
    {
        GameGrid.SetLine(gridIndex, x0, y0, x1, y1, colorIndex);
    }

    public static void SetRect(int gridIndex, int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)
    {
        GameGrid.SetRect(gridIndex, x0, y0, x1, y1, colorIndex, fill);
    }

    public static void SetCirc(int gridIndex, int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)
    {
        GameGrid.SetCirc(gridIndex, x0, y0, x1, y1, colorIndex, fill);
    }

    public static int GetPixel(int gridIndex, int x, int y)
    {
        return GameGrid.GetPixel(gridIndex, x, y);
    }

    public static void DrawCustomGrid(
        int gridIndex, int n, int x, int y, int scale, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1,
        bool flipX = false, bool flipY = false)
    {
        GameGrid.DrawCustomGrid(gridIndex, n, x,y, scale, colorIndex < 0 ? Color.White : ColorUtils.GetColor(colorIndex, transparency), w,h,flipX,flipY);
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

    public static void DrawRect(int ox, int oy, int x, int y, int width, int height, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawRectBorder(ox, oy, x, y, width, height, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawRectFill(int ox, int oy, int x, int y, int width, int height, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawRectFill(ox, oy, x, y, width, height, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawRect2(int ox, int oy, int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        var (rx0, ry0, rx1, ry1, w, h) = Shapes.AdjustRect(x0, y0, x1, y1);
        Shapes.DrawRectBorder(ox, oy, rx0, ry0, w, h, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawRectFill2(int ox, int oy, int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        var (rx0, ry0, rx1, ry1, w, h) = Shapes.AdjustRect(x0, y0, x1, y1);
        Shapes.DrawRectFill(ox, oy, rx0, ry0, w, h, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawCirc(int ox, int oy, int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawCirc(ox, oy, x0, y0, x1, y1, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawCircFill(int ox, int oy, int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawCircFill(ox, oy, x0, y0, x1, y1, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawCirc2(int x, int y, int r, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        CircleToRect(x, y, r, out int ox, out int oy, out int x0, out int y0, out int x1, out int y1);
        Shapes.DrawCirc(ox, oy, x0, y0, x1, y1, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawCircFill2(int x, int y, int r, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        CircleToRect(x, y, r, out int ox, out int oy, out int x0, out int y0, out int x1, out int y1);
        Shapes.DrawCircFill(ox, oy, x0, y0, x1, y1, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    private static void CircleToRect(int x, int y, int r, out int ox, out int oy, out int x0, out int y0, out int x1, out int y1)
    {
        ox = 0;
        oy = 0;
        x0 = x - r;
        y0 = y - r;
        x1 = x + r;
        y1 = y + r;
    }

    public static void DrawLine(int ox, int oy, int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawLine(ox, oy, x0, y0, x1, y1, scale, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void DrawPixel(int x, int y, int colorIndex = 0, int transparency = 10)
    {
        Shapes.DrawPixel(x, y, ColorUtils.GetColor(colorIndex, transparency));
    }

    public static void Print(string text, int x, int y, int colorIndex = 0, bool wraptext = false, int wrapLimit = 0)
    {
        Font.DrawText(text, new Vector2(x, y), ColorUtils.GetColor(colorIndex), wraptext, wrapLimit);
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

    public static void ResetMainFile()
    {
        GFW.LoadMainFile();
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

    public static void ReadSfx(string sfxfilename)
    {
        if (!HasFile(sfxfilename))
        {
            return;
        }
        var content = TxtFileIO.Read(sfxfilename);
        _player.ConvertStringToData(content);
    }

    public static void CreateOrUpdateSfx(string sfxfilename)
    {
        var content = _player.ConvertDataToString();
        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }
        TxtFileIO.CreateOrUpdate(sfxfilename, content);
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

    public static double GetElapsedTime()
    {
        return TimeUtils.ElapsedTime;
    }
    #endregion

    #region Utils
    public static int SubstringToInt(string source, int start, int length)
    {
        return int.Parse(source.AsSpan(start, length));
    }
    #endregion
}