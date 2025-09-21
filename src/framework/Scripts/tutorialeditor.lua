local tutorialeditor={
    firsttime = true,
    tutorialpages = {},
    currenttutorialpage = 1,
}

--[[
    -> Sprite Editor
    control A,W,S,D  -> translate sprite 
    control Z  -> undo
    control Y  -> redo
    control C  -> copy
    control V  -> paste
    control -> draw full circle/rect
    _mouse scroll -> zoom in sprite editor

    -> Map Editor
    control A,W,S,D  -> move view on map
    control mouse -> draw sprites as rectangle

    -> Sfx editor   

    -> Music editor
]]

function drawtutorialtext(text,x,y)
    _print(text,20 + x,20 + y,12,true)
end

function drawfunctiontext(o,px,ln,x,y)
    local txt = ""
    for i = px, px + ln do
        local item = o[i]
        if item then
            if item.t then
                txt = txt.."[c01]"..item.t.."\n[c12]"
            else
                txt = txt.."[c03]"..item.l.." -> [c04]"..item.c.."\n[c12]"
            end
        end
    end
    drawtutorialtext(txt,x,y)
end

function tutorialeditor:create()
    local firstpage = { category = "BlackBox" }
    function firstpage:draw()
        drawtutorialtext("[c04]Use 'Q' and 'E' to navigate pages[c12]",0,0)
        drawtutorialtext("[c03]BlackBox[c12] starts as a blank canvas.\nUse Lua scripts to build custom tools, editors and games.",0,20)
        drawtutorialtext("- [c03]NLua[c12] --version 1.7.5\n- [c03]MonoGame.Framework.DesktopGL[c12] --version 3.8.4\n- Mix of [c03]ANB16[c12] and [c03]Sweetie 16[c12] palettes. Ref: lospec.com ",0,50)
    end

    local hotkeystutorial = { category = "Global hotkeys" }
    function hotkeystutorial:draw()
        drawtutorialtext("BlackBox:\n- [c03]main.lua[c12] file: Entry point.\n- Exit: [c04]Alt + F4[c12].\n- Toggle Fullscreen: [c04]F2[c12].\nmain.lua:\n- Toggle Menu: [c04]F3[c12].\n- Restart: [c04]Esc[c12].\n- Save: [c04]Ctrl + R[c12].\n- Arrow keys [c04]A,W,D,S[c12] if page allows",0,0)
    end

    local luafunctions = { category = "Lua functions", px = 1, len = 20}
    function luafunctions:init()
        -- Existing Config
        self.func = {}
        add(self.func,{ t = "-- Configurations --"})
        add(self.func,{ l = "_title", c = "void ConfigTitle(string text)"})
        add(self.func,{ l = "_fps30", c = "void ConfigFps30()"})
        add(self.func,{ l = "_fps60", c = "void ConfigFps60()"})
        add(self.func,{ l = "_reboot", c = "void ResetMainFile()"})
        add(self.func,{ t = ""})

        -- Texture
        add(self.func,{ t = "-- Texture --"})
        add(self.func,{ l = "_limg", c = "void LoadTextureFromBase64(int index, int tileWidth, int tileHeight, string spriteBase64)"})
        add(self.func,{ l = "_dimg", c = "void DrawTexture(int index, int i, int x, int y, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)"})
        add(self.func,{ l = "_lsimg", c = "void LoadSingleImageFromBase64(int index, string spriteBase64)"})
        add(self.func,{ l = "_dsimg", c = "void DrawSingleImage(int index, int x, int y, int colorIndex = -1, int transparency = 10, bool flipX = false, bool flipY = false)"})
        add(self.func,{ l = "_dsimgfx", c = "void DrawSingleImageWithEffect(int index, int x, int y, double time, string parameters, int colorIndex = -1, int transparency = 10, bool flipX = false, bool flipY = false)"})
        add(self.func,{ t = ""})

        -- Input
        add(self.func,{ t = "-- Input Mouse --"})
        add(self.func,{ l = "_mouseshow", c = "void ShowHideMouse(bool show)"})
        add(self.func,{ l = "_mousepos", c = "LuaTable GetMousePos()"}) -- {x,y}
        add(self.func,{ l = "_mouseclick", c = "bool MouseButtonPressed(int i)"}) -- i == 1 right else left
        add(self.func,{ l = "_mouseclickp", c = "bool MouseButtonJustPressed(int i)"}) -- i == 1 right else left
        add(self.func,{ l = "_mouseclickr", c = "bool MouseButtonReleased(int i)"}) -- i == 1 right else left
        add(self.func,{ l = "_mousescroll", c = "bool Scroll(int i)"}) -- i == 1 up else down
        add(self.func,{ l = "_mousecursor", c = "void UpdateCursor(int i)"}) -- i -> 0 pointer i --> 1 hand
        add(self.func,{ t = ""})
        add(self.func,{ t = "-- Input Keyboard --"})
        add(self.func,{ l = "_btn", c = "bool Pressed(int keyNumber)"})
        add(self.func,{ l = "_btnp", c = "bool JustPressed(int keyNumber)"})
        add(self.func,{ l = "_btnr", c = "bool Released(int keyNumber)"})
        add(self.func,{ t = ""})

        -- Draw
        add(self.func,{ t = "-- Draw --"})
        add(self.func,{ l = "_crtshader", c = "void EnableCRTshader(bool value, int inner = 85, int outer = 110)"})
        add(self.func,{ l = "_bckgdclr", c = "void ConfigBackGroundColor(int colorIndex)"}) -- colorIndex 0 until 31
        add(self.func,{ l = "_pal", c = "void Pal(string palette)"}) -- "#000000,#ffffff,#f7aaa8,#697594,#d4689a,#782c96,#e83562,#f2825c,#ffc76e,#88c44d,#3f9e59,#373461,#4854a8,#7199d9,#9e5252,#4d2536,#1a1c2c,#5d275d,#b13e53,#ffa300,#ffec27,#a7f070,#38b764,#257179,#29366f,#3b5dc9,#41a6f6,#73eff7,#f4f4f4,#94b0c2,#566c86,#333c57";
        add(self.func,{ l = "_rect", c = "void DrawRect(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10, int thickness = 1)"})
        add(self.func,{ l = "_rectfill", c = "void DrawRectFill(int x, int y, int width, int height, int colorIndex = 0, int transparency = 10)"})
        add(self.func,{ l = "_circ", c = "void DrawCirc(int x, int y, int r, int colorIndex = 0, int transparency = 10)"})
        add(self.func,{ l = "_circfill", c = "void DrawCircFill(int x, int y, int r, int colorIndex = 0, int transparency = 10)"})
        add(self.func,{ l = "_circ2", c = "void DrawCirc2(int ox, int oy, int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10, int thickness = 1)"})
        add(self.func,{ l = "_circfill2", c = "void DrawCircFill2(int ox, int oy, int x0, int y0, int x1, int y1, int colorIndex = 0, int transparency = 10, int thickness = 1)"})
        add(self.func,{ l = "_line", c = "void DrawLine(int x0, int y0, int x1, int y1, int scale = 1, int colorIndex = 0, int transparency = 10)"})
        add(self.func,{ l = "_pixel", c = "void DrawPixel(int x, int y, int colorIndex = 0, int transparency = 10)"})
        add(self.func,{ l = "_print", c = "void Print(string text, int x, int y, int colorIndex = 0, bool wraptext = false, int wrapLimit = 0)"})
        add(self.func,{ l = "_camera", c = "void Camera(float x = 0.0f, float y = 0.0f)"})        
        add(self.func,{ t = ""})

        -- Status
        add(self.func,{ t = "-- Status --"})
        add(self.func,{ l = "_sysfps", c = "int GetFps()"})        
        add(self.func,{ l = "_isfocused", c = "bool IsFocused()"})
        add(self.func,{ t = ""})

        -- File
        add(self.func,{ t = "-- File --"})
        add(self.func,{ l = "_iohasfile", c = "bool HasFile(string fileName)"})
        add(self.func,{ l = "_ioread", c = "string ReadFile(string fileName)"})
        add(self.func,{ l = "_iocreate", c = " void CreateFile(string fileName, string content)"})
        add(self.func,{ l = "_ioupdate", c = "void UpdateFile(string fileName, string content)"})
        add(self.func,{ l = "_iocreateorupdate", c = "void CreateOrUpdateFile(string fileName, string content)"})
        add(self.func,{ l = "_iodelete", c = "void DeleteFile(string fileName)"})
        add(self.func,{ t = ""})

        -- Sfx
        add(self.func,{ t = "-- Sfx --"})
        add(self.func,{ l = "_loadsfx", c = "void ReadSfx(string sfxfilename)"})
        add(self.func,{ l = "_savesfx", c = "void CreateOrUpdateSfx(string sfxfilename)"})
        add(self.func,{ l = "_getsfx", c = "string GetSfx(int index)"})        
        add(self.func,{ l = "_setnotesfx", c = "void SetNoteSfx(int index, int noteIndex, string note)"})
        add(self.func,{ l = "_playsfx", c = "void PlaySfx(int index, int speed = 1, int channel = -1, int offset = 0)"})
        add(self.func,{ l = "_stopsfx", c = "void StopSfx(int index)"})
        add(self.func,{ l = "_validfx", c = "bool ValidSfx(string sound)"})
        add(self.func,{ t = ""})

        -- Time
        add(self.func,{ t = "-- Time --"})
        add(self.func,{ l = "_stimer", c = "void StartTimer(int i = 0)"})
        add(self.func,{ l = "_gtimer", c = "double GetTimer(int i = 0, int d = 4)"})
        add(self.func,{ l = "_pgame", c = "void PauseGame(bool value)"})
        add(self.func,{ l = "_gtime", c = "string GetDateTime(int i = 0)"})
        add(self.func,{ l = "_gdeltatime", c = "double GetDeltaTime()"}) 
        add(self.func,{ l = "_gelapsedtime", c = "double GetElapsedTime()"})
        add(self.func,{ t = ""})

        -- Grid
        add(self.func,{ t = "-- Grid --"})
        add(self.func,{ l = "_ngrid", c = "void NewGrid(int gridIndex, int columns, int rows, int size, bool enableUndoRedo = false)"})
        add(self.func,{ l = "_ggrid", c = "string GetGrid(int gridIndex)"})
        add(self.func,{ l = "_ggrid64", c = "string GetGridAsBase64(int gridIndex, int x, int y, int w, int h)"})
        add(self.func,{ l = "_cgrid", c = "void CopyGrid(int gridIndex, int x, int y, int w, int h)"})
        add(self.func,{ l = "_pgrid", c = "void PasteGrid(int gridIndex, int x, int y, int w, int h)"})
        add(self.func,{ l = "_mgrid", c = "void MoveGrid(int gridIndex, int x, int y, int w, int h, int deltaX, int deltaY)"})
        add(self.func,{ l = "_sgrid", c = "void SetGrid(int gridIndex, string grid)"})
        add(self.func,{ l = "_ugrid", c = "void UndoGrid(int gridIndex)"})
        add(self.func,{ l = "_rgrid", c = "void RedoGrid(int gridIndex)"})
        add(self.func,{ l = "_bgrid", c = "void PaintBucket(int gridIndex, int sx, int sy, int x, int y, int w, int h, int colorIndex = -1)"})
        add(self.func,{ l = "_gpixelgrid", c = "int GetPixel(int gridIndex, int x, int y)"})
        add(self.func,{ l = "_spixelgrid", c = "void SetPixel(int gridIndex, int x, int y, int colorIndex = -1)"})
        add(self.func,{ l = "_slinegrid", c = "void SetLine(int gridIndex, int x0, int y0, int x1, int y1, int colorIndex = -1)"})
        add(self.func,{ l = "_srectgrid", c = "void SetRect(int gridIndex, int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)"})
        add(self.func,{ l = "_scircgrid", c = "void SetCirc(int gridIndex, int x0, int y0, int x1, int y1, int colorIndex = -1, bool fill = false)"})
        add(self.func,{ l = "_dgrid", c = "void DrawCustomGrid(int gridIndex, int n, int x, int y, int scale, int colorIndex = -1, int transparency = 10, int w = 1, int h = 1, bool flipX = false, bool flipY = false)"})
        add(self.func,{ t = ""})

        -- Map
        add(self.func,{ t = "-- Map --"})
        add(self.func,{ l = "_lmap", c = "void SetMap(int index, string grid)"})
        add(self.func,{ l = "_cmap", c = "void CreateMap(int index, int columns, int rows, int size)"})
        add(self.func,{ l = "_gmap", c = "string GetMap(int index)"}) 
        add(self.func,{ l = "_smap", c = "void SetTileInMap(int index, int x, int y, int tileIndex = 0)"})
        add(self.func,{ l = "_bmap", c = "void UpdateTileInMap(int index, int mapX, int mapY,int width, int height, int tileIndex = 0)"})
        add(self.func,{ l = "_dmap", c = "void DrawMap(int index, int mapX, int mapY, int x, int y, int width, int height, int colorIndex = -1, int transparency = 10)"})
        add(self.func,{ t = ""})
    end

    luafunctions:init();

    function luafunctions:update()
        if _btn(_keys.W) then     
            self.px = self.px - 1
        end
        if _btn(_keys.S) then
            self.px = self.px + 1
        end
        self.px = clamp(1,self.px,#self.func)
    end

    function luafunctions:draw()
        drawfunctiontext(self.func,self.px,self.len,0,0)
    end

    add(tutorialeditor.tutorialpages,firstpage)
    add(tutorialeditor.tutorialpages,hotkeystutorial)
    add(tutorialeditor.tutorialpages,luafunctions)
end

function tutorialeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function tutorialeditor:update()
    if _btnp(_keys.Q) then     
        self.currenttutorialpage = self.currenttutorialpage - 1
    end
    if _btnp(_keys.E) then
        self.currenttutorialpage = self.currenttutorialpage + 1
    end

    self.currenttutorialpage = clamp(1, self.currenttutorialpage, #self.tutorialpages)
    local page=self.tutorialpages[self.currenttutorialpage]
    if page.update then
        page:update()
    end
end

function tutorialeditor:draw()
   local page=self.tutorialpages[self.currenttutorialpage]
   if page.draw then
        _print("PAG#: "..self.currenttutorialpage.." ".."("..page.category..")",12,2, 13)
        page:draw()
    end
end

return tutorialeditor