local mapeditor={}
local map_x, map_y = 15, 5
local sprites_x, sprites_y = 15,135

function mapeditor:init()
end

function mapeditor:update()
end

function mapeditor:draw()
    _rectfill(10,0,310,180,11)
    _rectfill(map_x - 1, map_y - 1,30*10 + 2,110 + 2, 0)
    _csprc(1,0,map_x,map_y,3,2,30,11)
    
    _rectfill(sprites_x - 1, sprites_y - 1,30*10 + 2,4*10 + 2, 0)
    _csprc(1,0,sprites_x,sprites_y,3,2,30,4)
    _cgridc(0,sprites_x,sprites_y,1,-1,10,30,4,false,false)
end

return mapeditor