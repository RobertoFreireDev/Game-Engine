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
        drawtutorialtext("- [c03]NLua[c12] --version 1.7.5\n- [c03]MonoGame.Framework.DesktopGL[c12] --version 3.8.4\n- Mix of [c03]ANB16[c12] and [c03]Sweetie 16[c12] palettes. Ref: lospec.com. \n- Sfx from hunteraudio.itch.io/8bit-sfx-and-music-pack",0,50)
    end

    local hotkeystutorial = { category = "Global hotkeys" }
    function hotkeystutorial:draw()
        drawtutorialtext("BlackBox:\n- [c03]main.lua[c12] file: Entry point.\n- Exit: [c04]Alt + F4[c12].\n- Toggle Fullscreen: [c04]F2[c12].\nmain.lua:\n- Toggle Menu: [c04]F3[c12].\n- Restart: [c04]Esc[c12].\n- Save: [c04]Ctrl + R[c12].\n- Arrow keys [c04]A,W,D,S[c12] if page allows",0,0)
    end

    add(tutorialeditor.tutorialpages,firstpage)
    add(tutorialeditor.tutorialpages,hotkeystutorial)
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