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
}

function game:init()
   _loadtexture(2,64,64,"iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAMAAACdt4HsAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAAUElEQVRYhe3VsQkAIBQDUbELiIKt7j+n/AXSpJO7NvDaNIU1bZs0bQW4vYBhAgAAAAAA+BFIzzW+9xg4NunaClimAroJAAAAAADgRyA917AH9ry8jfwre4oAAAAASUVORK5CYII=")
end

function game:update()
end

function game:draw()
    local color = 6
    local index = 2
    _bfx(_gelapsedtime(),"4411000001", color, 4)
        _drawtexture(index, 0, 20, 20, 1, 1, false, false)
    _efx()
    _bfx(_gelapsedtime(),"4411000002", color, 4)
        _drawtexture(index, 0, 20, 100, 1, 1, false, false)
    _efx()
    _bfx(_gelapsedtime(),"0000550001", color, 4)
        _drawtexture(index, 0, 100, 20, 1, 1, false, false)
    _efx()
    _bfx(_gelapsedtime(),"0000550002", color, 4)
        _drawtexture(index, 0, 100, 100, 1, 1, false, false)
    _efx()
end

return game