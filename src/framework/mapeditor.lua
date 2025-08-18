local mapeditor={}
local gridIndex = 0
local map_x, map_y = 15, 15
local sprites_x, sprites_y = 15,135

function mapeditor:init()
end

function mapeditor:update()
end

function mapeditor:draw()
    _rectfill(10,10,310,170,11)
    _rectfill(map_x - 1, map_y - 1,300 + 2,100 + 2, 0)
    
    _rectfill(sprites_x - 1, sprites_y - 1,300 + 2,40 + 2, 0)
    _cgridc(gridIndex,0,sprites_x,sprites_y,1,-1,10,30,4,false,false)
    _print("Map Editor", 12, 2, 11)
end

return mapeditor