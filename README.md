# Black-Box

This engine starts as a blank canvas, giving Lua control over display and logic

Developers can write Lua scripts to build custom tools, editors, or games.

![alt text](image.png)

# Package games for distribution

Visual Studio on terminal/power shell

```
dotnet publish blackbox.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

https://docs.monogame.net/articles/getting_started/packaging_games.html?tabs=windows


🧩 Texture Functions

| Function                                                                                                                                                                 | Lua Alias | Description                                                                       |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | --------- | --------------------------------------------------------------------------------- |
| `LoadTextureFromBase64(int index, int tileWidth, int tileHeight, string spriteBase64)`                                                                                   | `_limg`   | Loads a tiled texture from a Base64 string into memory.                           |
| `DrawTexture(int index, int i, int x, int y, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)`                  | `_dimg`   | Draws a tile from a texture at a given position with optional color and flipping. |
| `LoadSingleImageFromBase64(int index, string spriteBase64)`                                                                                                              | `_lsimg`  | Loads a single image texture from a Base64 string.                                |
| `DrawSingleImage(int index, int x, int y, int colorIndex = -1, int transparency = 10, bool flipX = false, bool flipY = false)`                                           | `_dsimg`  | Draws a single image at a given position.                                         |
| `DrawSingleImageWithEffect(int index, int x, int y, double time, string parameters, int colorIndex = -1, int transparency = 10, bool flipX = false, bool flipY = false)` | `_dsimgfx`| Draws a single image with shader-based distortion and color effects.              |
| `FixLength(string input, int x)`                                                                                                                                         | `—`       | Ensures a string has a fixed length by padding or trimming.                       |


🗺️ Map Functions

| Function                                                                                                       | Lua Alias | Description                               |
| -------------------------------------------------------------------------------------------------------------- | --------- | ----------------------------------------- |
| `CreateMap(int columns, int rows, int size)`                                                                   | `_cmap`   | Creates a new map grid.                   |
| `SetTileInMap(int x, int y, int tileIndex = 0)`                                                                | `_smap`   | Sets a tile at a specific grid position.  |
| `DrawMap(int mapX, int mapY, int x, int y, int width, int height, int colorIndex = -1, int transparency = 10)` | `_dmap`   | Draws a portion of the map to the screen. |
| `UpdateTileInMap(int x0, int y0, int x1, int y1, int tileIndex = 0)`                                           | `_bmap`   | Updates multiple tiles in the map.        |
| `GetMap()`                                                                                                     | `_gmap`   | Returns the current map data as a string. |
| `SetMap(string grid)`                                                                                          | `_lmap`   | Loads map data from a string.             |


🧮 Grid Functions

| Function                                                                                                                                             | Lua Alias     | Description                                     |
| ---------------------------------------------------------------------------------------------------------------------------------------------------- | ------------- | ----------------------------------------------- |
| `NewGrid(int columns, int rows, int size, bool enableUndoRedo = false)`                                                                              | `_ngrid`      | Creates a new grid for drawing.                 |
| `UndoGrid()`                                                                                                                                         | `_ugrid`      | Undoes the last grid action.                    |
| `RedoGrid()`                                                                                                                                         | `_rgrid`      | Redoes the last undone grid action.             |
| `CopyGrid(int x, int y, int w, int h)`                                                                                                               | `_cgrid`      | Copies a region of the grid.                    |
| `PasteGrid(int x, int y, int w, int h)`                                                                                                              | `_pgrid`      | Pastes copied grid data into a region.          |
| `MoveGrid(int x, int y, int w, int h, int deltaX, int deltaY)`                                                                                       | `_mgrid`      | Moves a region of the grid.                     |
| `SetGrid(string grid)`                                                                                                                               | `_sgrid`      | Sets the grid state from string data.           |
| `GetGrid()`                                                                                                                                          | `_ggrid`      | Returns the entire grid as a string.            |
| `GetGridAsBase64(int x, int y, int w, int h)`                                                                                                        | `_ggrid64`    | Returns a region of the grid encoded in Base64. |
| `SetPixel(int x, int y, int colorIndex = -1)`                                                                                                        | `_spixelgrid` | Sets a pixel’s color in the grid.               |
| `PaintBucket(int sx, int sy, int x, int y, int w, int h, int colorIndex = -1)`                                                                       | `_bgrid`      | Flood fills a region with color.                |
| `SetLine(int x0, int y0, int x1, int y1, int colorIndex = -1)`                                                                                       | `_slinegrid`  | Draws a line between two grid points.           |
| `SetRect(int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)`                                                                    | `_srectgrid`  | Draws a rectangle on the grid.                  |
| `SetCirc(int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)`                                                                    | `_scircgrid`  | Draws a circle on the grid.                     |
| `GetPixel(int x, int y)`                                                                                                                             | `_gpixelgrid` | Returns the color index of a pixel.             |
| `DrawGrid(int n, int x, int y, int scale, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)` | `_dgrid`      | Draws a grid layer or frame to the screen.      |


⚙️ Init Functions

| Function                                                       | Lua Alias   | Description                             |
| -------------------------------------------------------------- | ----------- | --------------------------------------- |
| `ConfigTitle(string text)`                                     | `_title`    | Sets the window or game title.          |
| `ConfigFps30()`                                                | `_fps30`    | Limits the frame rate to 30 FPS.        |
| `ConfigFps60()`                                                | `_fps60`    | Limits the frame rate to 60 FPS.        |
| `EnableCRTshader(bool value, int inner = 85, int outer = 110)` | `_crtshader`| Enables or disables a CRT-style shader. |
| `ConfigBackGroundColor(int colorIndex)`                        | `_bckgdclr` | Sets the background color.              |


🖱️ Input Functions

| Function                        | Lua Alias      | Description                                                            |
| ------------------------------- | -------------- | ---------------------------------------------------------------------- |
| `GetMousePos()`                 | `_mousepos`    | Returns a Lua table with the current mouse coordinates.                |
| `MouseButtonPressed(int i)`     | `_mouseclick`  | Returns true if a mouse button is currently pressed (0=left, 1=right). |
| `MouseButtonJustPressed(int i)` | `_mouseclickp` | Returns true if a mouse button was just pressed this frame.            |
| `MouseButtonReleased(int i)`    | `_mouseclickr` | Returns true if a mouse button was released this frame.                |
| `Scroll(int i)`                 | `_mousescroll` | Checks for scroll input (1=up, else down).                             |
| `UpdateCursor(int i)`           | `_mousecursor` | Updates the cursor style or state.                                     |
| `UpdateCursor/ShowHideMouse`    | `_mouseshow`   | Shows or hides the mouse cursor.                                       |
| `JustPressed(int keyNumber)`    | `_btnp`        | Returns true if a key was just pressed.                                |
| `Released(int keyNumber)`       | `_btnr`        | Returns true if a key was released.                                    |
| `Pressed(int keyNumber)`        | `_btn`         | Returns true if a key is currently held down.                          |


🎨 Draw Functions

| Function                                                                                         | Lua Alias     | Description                                   |
| ------------------------------------------------------------------------------------------------ | ------------- | --------------------------------------------- |
| `ShowHideMouse(bool show)`                                                                       | `_mouseshow`  | Shows or hides the mouse cursor.              |
| `Pal(string palette)`                                                                            | `_pal`        | Loads a new color palette.                    |
| `DrawRect(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10, int thickness = 1)`                                                                                  | `_rect`       | Draws a rectangle outline.                    |
| `DrawRectFill(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10)`                                                                              | `_rectfill`   | Draws a filled rectangle.                     |
| `DrawCirc(int x, int y, int r, int colorIndex = 0, int transparency = 10)`                                                                                  | `_circ`       | Draws a circle outline.                       |
| `DrawCircFill(int x, int y, int r, int colorIndex = 0, int transparency = 10)`                                                                              | `_circfill`   | Draws a filled circle.                        |
| `DrawCirc2(int ox, int oy, int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10, int thickness = 1)`                                                                                 | `_circ2`      | Draws a circle using rectangle bounds.        |
| `DrawCircFill2(int ox, int oy, int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10, int thickness = 1)`                                                                             | `_circfill2`  | Draws a filled circle using rectangle bounds. |
| `DrawLine(int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)`                                                                                  | `_line`       | Draws a line between two points.              |
| `DrawPixel(int x, int y, int colorIndex = 0, int transparency = 10)`                                                                                 | `_pixel`      | Draws a single pixel.                         |
| `Print(string text, int x, int y, int colorIndex = 0, bool wraptext = false, int wrapLimit = 0)` | `_print`      | Renders text on screen.                       |
| `Camera(float x = 0.0f, float y = 0.0f)`                                                         | `_camera`     | Moves or resets the camera position.          |


🖥️ System Functions

| Function          | Lua Alias  | Description                            |
| ----------------- | ---------- | -------------------------------------- |
| `GetFps()`        | `_sysfps`  | Returns the current frames per second. |
| `IsFocused()`     | `_isfocused` | Checks if the game window is focused.  |
| `ResetMainFile()` | `_reboot`  | Reloads the main Lua file.             |


💾 IO File Functions

| Function                                              | Lua Alias         | Description                                   |
| ----------------------------------------------------- | ----------------- | --------------------------------------------- |
| `HasFile(string fileName)`                            | `_iohasfile`      | Checks if a file exists.                      |
| `ReadFile(string fileName)`                           | `_ioread`         | Reads the contents of a text file.            |
| `CreateFile(string fileName, string content)`         | `_iocreate`       | Creates a new text file.                      |
| `UpdateFile(string fileName, string content)`         | `_ioupdate`       | Overwrites the contents of an existing file.  |
| `DeleteFile(string fileName)`                         | `_iodelete`       | Deletes a file.                               |
| `CreateOrUpdateFile(string fileName, string content)` | `_iocreateorupdate` | Creates or updates a file based on existence. |


🔊 SFX Functions

| Function                | Lua Alias | Description                            |
| ----------------------- | --------- | -------------------------------------- |
| `PlaySfx(string index)` | `_psfx`   | Plays a sound effect by name or index. |


⏱️ Timer Functions

| Function                         | Lua Alias      | Description                                    |
| -------------------------------- | -------------- | ---------------------------------------------- |
| `StartTimer(int i = 0)`          | `_stimer`      | Starts or resets a timer.                      |
| `GetTimer(int i = 0, int d = 4)` | `_gtimer`      | Returns elapsed time from a timer (formatted). |
| `PauseGame(bool value)`          | `_pgame`       | Pauses or resumes the game.                    |
| `GetDateTime(int i = 0)`         | `_gtime`       | Returns the current date/time string.          |
| `GetDeltaTime()`                 | `_gdeltatime`  | Returns the time since the last frame.         |
| `GetElapsedTime()`               | `_gelapsedtime`| Returns total game runtime.                    |


