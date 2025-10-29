--[[
    Use this to test each and every function
    Filter and validate each and every parameter of lua functions
    Use this category to show case all game engine features and capabilities
    Test every thing!. Example: Pause game, restart game, initial variables, edge cases for every function
    - Test everything together:
    - camera, movement, draw
    Edge cases: 
    - Close to limits of index for 1d and 2d arrays
    - Beyong the lLimits of index for 1d and 2d arrays
    - Big negative and positive numbers 
]]

local game={
}

function game:init()
    _setnotesfx(0, 0, "48205")
    _setnotesfx(0, 1, "65206")
    _setnotesfx(0, 2, "50208")
    _setnotesfx(0, 3, "71209")
end

function game:update()

    if _btnp(_keys.A) then
        _playsfx(0,8)
    end
end

function game:draw()
end

return game