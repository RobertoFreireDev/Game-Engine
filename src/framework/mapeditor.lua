local mapeditor={}
local map_x, map_y, map_columns, map_rows = 15, 5, 30, 12
local sprites_x, sprites_y = 15,135
local sprites_w,sprites_h,sprites_cell=30,4,10
local spriteNumber = 0
local pageNumber = 0

function mapeditor:init()
end

function mapeditor:update()   
    if _mouseclick(0) then
        local mousepos = _mousepos()
        local gridpos = screen_to_grid(mousepos,map_x, map_y, map_columns, map_rows, sprites_cell)
        if gridpos.x and gridpos.y then
            _stilemap(gridpos.x,gridpos.y,spriteNumber)
        else
            local spritespos = screen_to_grid(mousepos,sprites_x, sprites_y, sprites_w, sprites_h, sprites_cell)
            spriteNumber = updateSpriteNumber(spritespos,spriteNumber,pageNumber,sprites_w,sprites_h)
        end
    end

    pageNumber = movepage(pageNumber)
end

function mapeditor:draw()
    _rectfill(10,0,310,180,11)
    _rectfill(map_x - 1, map_y - 1,map_columns*10 + 2,map_rows*10 + 2, 0)
    _csprc(1,0,map_x,map_y,3,2,map_columns,map_rows)
    _drawmap(map_x,map_y)
    
    drawPageSpriteNumbers(spriteNumber,pageNumber,sprites_x,sprites_y)

    _rectfill(sprites_x - 1, sprites_y - 1,sprites_w*10 + 2,sprites_h*10 + 2, 0)
    _csprc(1,0,sprites_x,sprites_y,3,2,sprites_w,sprites_h)
    _cgridc(pageNumber*sprites_w*sprites_h,sprites_x,sprites_y,1,-1,10,sprites_w,sprites_h,false,false)
    drawSelectedRec(spriteNumber, pageNumber, sprites_w, sprites_h, sprites_x, sprites_y, sprites_cell)
end

return mapeditor