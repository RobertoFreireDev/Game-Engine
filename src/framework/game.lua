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
   _lsimg(0,_ggrid64(0,10,0,20,20))
   _lsimg(1,_ggrid64(0,30,0,20,20))
   _lsimg(2,_ggrid64(0,50,0,20,20))
   _lsimg(3,_ggrid64(0,70,0,20,20))
   _lsimg(4,_ggrid64(0,90,0,20,20))
   _lsimg(5,_ggrid64(0,110,0,20,20))
   _lsimg(6,_ggrid64(0,180,0,40,40))
   _lsimg(7,_ggrid64(0,220,0,40,40))
   _lsimg(8,_ggrid64(0,260,0,40,40))
   _limg(2,10,10,_ggrid64(0,0,0,30*10-1,4*8*10-1))
   _stimer(0)
end

function game:update()
    self.timer = _gtimer(0)
end

function game:draw()
    local color = 6
    _dsimgfx(3, 20, 20, self.timer,"00001000020000", color, 10, false, false)
    _dsimgfx(2, 20, 20, self.timer,"00002000020000", color, 10, false, false)
    _dsimgfx(1, 20, 20, self.timer,"00003000020000", color, 10, false, false)
    _dsimgfx(0, 20, 20, self.timer,"00004000020000", color, 10, false, false)
    _dsimgfx(4, 40, 20, self.timer,"03330000010000", color, 10, false, false)    
    _dsimgfx(5, 60, 20, self.timer,"33110000010000", color, 10, false, false)
    _dimg(2, 14, 80, 20, -1, 10, 4, 4)
    _rectfill(80, 20, 40, 40, 26, 5)
    _dsimgfx(8, 80, 20, self.timer,"33112200020000", color, 10, false, false)
    _dsimgfx(6, 80, 20, self.timer,"33110000010000", color, 10, false, false)
    _dsimgfx(7, 80, 20, self.timer,"33110000010000", color, 10, false, false)
    _dmap(0, 0, 8, 20, 80, 8, 4)
    _dgrid(0,63,30,90,1)
    _dgrid(0,62,30,90,1)
end

return game