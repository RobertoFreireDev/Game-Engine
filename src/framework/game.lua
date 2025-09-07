--[[
    Use this to test each and every function
    Filter and validate each and every parameter of lua functions
    Use this category to show case all game engine features and capabilities
    Test every thing!. Example: Pause game, restart game, initial variables, edge cases for every function
    - Test everything together:
    - camera, movement, draw
    - multiplse sfx and music
    Edge cases: 
    - Close to limits of index for 1d and 2d arrays
    - Beyong the lLimits of index for 1d and 2d arrays
    - Big negative and positive numbers 
]]

local game={
    timer = 0
}

function game:init()
   _limg(2,10,10,_ggrid64(0))
   _lsimg(3,"iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAMAAACdt4HsAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAAUElEQVRYhe3VsQkAIBQDUbELiIKt7j+n/AXSpJO7NvDaNIU1bZs0bQW4vYBhAgAAAAAA+BFIzzW+9xg4NunaClimAroJAAAAAADgRyA917AH9ry8jfwre4oAAAAASUVORK5CYII=")
   _stimer(0)
end

function game:update()
    self.timer = _gtimer(0)
end

function game:draw()
    local color = 6
    local index = 3

    _dsimgfx(index,  20,  20, self.timer,"44110000011000", color, 4, false, false)
    _dsimgfx(index,  20, 100, self.timer,"44110000020100", color, 4, false, false)
    _dsimgfx(index, 100,  20, self.timer,"00009900010010", color, 4, false, false)
    _dsimgfx(index, 100, 100, self.timer,"00009900020001", color, 4, false, false)
end

return game