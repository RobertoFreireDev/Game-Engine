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

| Function                                                                                                                                                                 | Description                                                                       |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------- |
| `LoadTextureFromBase64(int index, int tileWidth, int tileHeight, string spriteBase64)`                                                                                   | Loads a tiled texture from a Base64 string into memory.                           |
| `DrawTexture(int index, int i, int x, int y, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)`                  | Draws a tile from a texture at a given position with optional color and flipping. |
| `LoadSingleImageFromBase64(int index, string spriteBase64)`                                                                                                              | Loads a single image texture from a Base64 string.                                |
| `DrawSingleImage(int index, int x, int y, int colorIndex = -1, int transparency = 10, bool flipX = false, bool flipY = false)`                                           | Draws a single image at a given position.                                         |
| `DrawSingleImageWithEffect(int index, int x, int y, double time, string parameters, int colorIndex = -1, int transparency = 10, bool flipX = false, bool flipY = false)` | Draws a single image with shader-based distortion and color effects.              |
| `FixLength(string input, int x)`                                                                                                                                         | Ensures a string has a fixed length by padding or trimming.                       |

🗺️ Map Functions

| Function                                                                                                       | Description                               |
| -------------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| `CreateMap(int columns, int rows, int size)`                                                                   | Creates a new map grid.                   |
| `SetTileInMap(int x, int y, int tileIndex = 0)`                                                                | Sets a tile at a specific grid position.  |
| `DrawMap(int mapX, int mapY, int x, int y, int width, int height, int colorIndex = -1, int transparency = 10)` | Draws a portion of the map to the screen. |
| `UpdateTileInMap(int x0, int y0, int x1, int y1, int tileIndex = 0)`                                           | Updates multiple tiles in the map.        |
| `GetMap()`                                                                                                     | Returns the current map data as a string. |
| `SetMap(string grid)`                                                                                          | Loads map data from a string.             |

🧮 Grid Functions
| Function                                                                                                                                             | Description                                     |
| ---------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------- |
| `NewGrid(int columns, int rows, int size, bool enableUndoRedo = false)`                                                                              | Creates a new grid for drawing.                 |
| `UndoGrid()`                                                                                                                                         | Undoes the last grid action.                    |
| `RedoGrid()`                                                                                                                                         | Redoes the last undone grid action.             |
| `CopyGrid(int x, int y, int w, int h)`                                                                                                               | Copies a region of the grid.                    |
| `PasteGrid(int x, int y, int w, int h)`                                                                                                              | Pastes copied grid data into a region.          |
| `MoveGrid(int x, int y, int w, int h, int deltaX, int deltaY)`                                                                                       | Moves a region of the grid.                     |
| `SetGrid(string grid)`                                                                                                                               | Sets the grid state from string data.           |
| `GetGrid()`                                                                                                                                          | Returns the entire grid as a string.            |
| `GetGridAsBase64(int x, int y, int w, int h)`                                                                                                        | Returns a region of the grid encoded in Base64. |
| `SetPixel(int x, int y, int colorIndex = -1)`                                                                                                        | Sets a pixel’s color in the grid.               |
| `PaintBucket(int sx, int sy, int x, int y, int w, int h, int colorIndex = -1)`                                                                       | Flood fills a region with color.                |
| `SetLine(int x0, int y0, int x1, int y1, int colorIndex = -1)`                                                                                       | Draws a line between two grid points.           |
| `SetRect(int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)`                                                                    | Draws a rectangle on the grid.                  |
| `SetCirc(int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)`                                                                    | Draws a circle on the grid.                     |
| `GetPixel(int x, int y)`                                                                                                                             | Returns the color index of a pixel.             |
| `DrawGrid(int n, int x, int y, int scale, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)` | Draws a grid layer or frame to the screen.      |

⚙️ Init Functions

| Function                                                       | Description                             |
| -------------------------------------------------------------- | --------------------------------------- |
| `ConfigTitle(string text)`                                     | Sets the window or game title.          |
| `ConfigFps30()`                                                | Limits the frame rate to 30 FPS.        |
| `ConfigFps60()`                                                | Limits the frame rate to 60 FPS.        |
| `EnableCRTshader(bool value, int inner = 85, int outer = 110)` | Enables or disables a CRT-style shader. |
| `ConfigBackGroundColor(int colorIndex)`                        | Sets the background color.              |

🖱️ Input Functions

| Function                        | Description                                                            |
| ------------------------------- | ---------------------------------------------------------------------- |
| `GetMousePos()`                 | Returns a Lua table with the current mouse coordinates.                |
| `MouseButtonPressed(int i)`     | Returns true if a mouse button is currently pressed (0=left, 1=right). |
| `MouseButtonJustPressed(int i)` | Returns true if a mouse button was just pressed this frame.            |
| `MouseButtonReleased(int i)`    | Returns true if a mouse button was released this frame.                |
| `Scroll(int i)`                 | Checks for scroll input (1=up, else down).                             |
| `UpdateCursor(int i)`           | Updates the cursor style or state.                                     |
| `JustPressed(int keyNumber)`    | Returns true if a key was just pressed.                                |
| `Released(int keyNumber)`       | Returns true if a key was released.                                    |
| `Pressed(int keyNumber)`        | Returns true if a key is currently held down.                          |


🎨 Draw Functions

| Function                                                                                         | Description                                   |
| ------------------------------------------------------------------------------------------------ | --------------------------------------------- |
| `ShowHideMouse(bool show)`                                                                       | Shows or hides the mouse cursor.              |
| `Pal(string palette)`                                                                            | Loads a new color palette.                    |
| `DrawRect(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10, int thickness = 1)`                                                                                  | Draws a rectangle outline.                    |
| `DrawRectFill(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10)`                                                                              | Draws a filled rectangle.                     |
| `DrawCirc(int x, int y, int r, int colorIndex = 0, int transparency = 10)`                                                                                  | Draws a circle outline.                       |
| `DrawCircFill(int x, int y, int r, int colorIndex = 0, int transparency = 10)`                                                                              | Draws a filled circle.                        |
| `DrawCirc2(int ox, int oy, int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10, int thickness = 1)`                                                                                 | Draws a circle using rectangle bounds.        |
| `DrawCircFill2(int ox, int oy, int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10, int thickness = 1)`                                                                             | Draws a filled circle using rectangle bounds. |
| `DrawLine(int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)`                                                                                  | Draws a line between two points.              |
| `DrawPixel(int x, int y, int colorIndex = 0, int transparency = 10)`                                                                                 | Draws a single pixel.                         |
| `Print(string text, int x, int y, int colorIndex = 0, bool wraptext = false, int wrapLimit = 0)` | Renders text on screen.                       |
| `Camera(float x = 0.0f, float y = 0.0f)`                                                         | Moves or resets the camera position.          |


🖥️ System Functions

| Function          | Description                            |
| ----------------- | -------------------------------------- |
| `GetFps()`        | Returns the current frames per second. |
| `IsFocused()`     | Checks if the game window is focused.  |
| `ResetMainFile()` | Reloads the main Lua file.             |


💾 IO File Functions

| Function                                              | Description                                   |
| ----------------------------------------------------- | --------------------------------------------- |
| `HasFile(string fileName)`                            | Checks if a file exists.                      |
| `ReadFile(string fileName)`                           | Reads the contents of a text file.            |
| `CreateFile(string fileName, string content)`         | Creates a new text file.                      |
| `UpdateFile(string fileName, string content)`         | Overwrites the contents of an existing file.  |
| `DeleteFile(string fileName)`                         | Deletes a file.                               |
| `CreateOrUpdateFile(string fileName, string content)` | Creates or updates a file based on existence. |


🔊 SFX Functions

| Function                         | Description                                    |
| -------------------------------- | ---------------------------------------------- |
| `StartTimer(int i = 0)`          | Starts or resets a timer.                      |
| `GetTimer(int i = 0, int d = 4)` | Returns elapsed time from a timer (formatted). |
| `PauseGame(bool value)`          | Pauses or resumes the game.                    |
| `GetDateTime(int i = 0)`         | Returns the current date/time string.          |
| `GetDeltaTime()`                 | Returns the time since the last frame.         |
| `GetElapsedTime()`               | Returns total game runtime.                    |


