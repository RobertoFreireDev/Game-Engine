local mapeditor={}
local gridIndex = 0
local origin_x, origin_y = 15, 15

function mapeditor:init()
end

function mapeditor:update()
end

function mapeditor:draw()
    _rectfill(10,10,310,170,11)
    _rectfill(origin_x - 1, origin_y - 1,290 + 2,100 + 2, 0)    
    _csprc(1,0,origin_x,origin_y,3,2,29,10)
    _cgridc(gridIndex,0,origin_x,origin_y,1,-1,10,1,1,false,false)
    _print("Map Editor", 12, 2, 11)
end

return mapeditor