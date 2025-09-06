local tutorialeditor={
    firsttime = true,
}

--[[
    Split tutorial into categories
    Each category have pages

    Categoy -> Game Engine
    Explain this tutorial buttons. Example: buttons to navigate through pages
    explain monogame, structure and overall idea
    explain main file
    explain restart using esc
    explain save using control R
    Alt F4 -> Exit
    F2 -> ToggleFullScreen

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