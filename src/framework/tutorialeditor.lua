local tutorialeditor={
    firsttime = true,
}

--[[
    Alt F4 -> Exit
    F2 -> ToggleFullScreen

    Sprite Editor
    control Z  -> undo
    control Y  -> redo
    control -> draw full circle/rect
   _mouse scroll -> zoom in sprite editor

    Explain All LuaBinding Functions
]]

function tutorialeditor:create()
end

function tutorialeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function tutorialeditor:update()   
    
end

function tutorialeditor:draw()
   _print("Tutorial",10,2,1)
end

return tutorialeditor