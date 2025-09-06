local tutorialeditor={
    firsttime = true,
    tutorialpages = {},
    currenttutorialpage = 1,
}

--[[
    Categoy -> Explain All LuaBinding Functions
    Use this category to test each function
    Use this category to explain each function
    Also, filter and validate each and every parameter of lua functions

    Categoy -> Sprite Editor
    control A,W,S,D  -> move sprite 
    control Z  -> undo
    control Y  -> redo
    control C  -> copy
    control V  -> paste
    control -> draw full circle/rect
    _mouse scroll -> zoom in sprite editor

    Add show pos x and y to validate shapes and draw logic

    Categoy -> Map Editor
    control A,W,S,D  -> move map

    Categoy -> Sfx editor   

    Categoy -> Music editor

    Categoy -> Game
    Use this category to show case all game engine features and capabilities
    Use this category to test every thing. Example: Pause game, restart game
]]

function drawtutorialtext(text,x,y)
    _print(text,20 + x,20 + y,1,true)
end

function drawfunctiontext(o,x,y)
    local txt = ""
    foreach(o, function(i)
        txt = txt.."Lua - "..i.l.." -> C# - "..i.c.."\n"
    end)
    drawtutorialtext(txt,x,y)
end

function tutorialeditor:create()
    local firstpage = { category = "BlackBox overview" }
    function firstpage:draw()
        drawtutorialtext("Use 'Q' and 'E' to navigate pages",0,0)
        drawtutorialtext("BlackBox game engine uses NLua for Lua scripting integration and MonoGame as the game framework.",0,20)
        drawtutorialtext("- NLua --version 1.7.5\n- MonoGame.Framework.DesktopGL --version 3.8.4\n- Mix of ANB16 and Sweetie 16 palettes. Ref: lospec.com ",0,40)
    end

    local hotkeystutorial = { category = "Global hotkeys" }
    function hotkeystutorial:draw()
        drawtutorialtext("BlackBox:\n- main.lua file: Entry point. In this case, we are using it for both editor and game.\n- Exit: Press Alt + F4 to quit.\n- Toggle Fullscreen: Press F2 to switch between fullscreen/window.\nmain.lua:\n- Restart: Press Esc to restart the game.\n- Save: Press Ctrl + R to save progress.",0,0)
    end

    local luafunctions = { category = "Functions/configurations"}
    function luafunctions:init()
        self.func={}
        add(self.func,{ l = "_title", c = "void ConfigTitle(string text)"})
        add(self.func,{ l = "_fps30", c = "void ConfigFps30()"})
        add(self.func,{ l = "_fps60", c = "void ConfigFps60()"})
        add(self.func,{ l = "_reboot", c = "void ResetMainFile()"})
    end

    luafunctions:init();

    function luafunctions:draw()
        drawfunctiontext(self.func,0,0)
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
        _print("PAG#: "..self.currenttutorialpage.." ".."("..page.category..")",12,2, 12)
        page:draw()
    end
end

return tutorialeditor