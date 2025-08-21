local mapeditor={}
local map_x, map_y = 15, 5
local sprites_x, sprites_y = 15,135
local sprites_w,sprites_h,sprites_cell=30,4,10
local spriteNumber = 0
local pageNumber = 0

function mapeditor:init()
end

function mapeditor:update()
    if _mouseclick(0) then
        local mousepos = _mousepos()
        local spritespos = screen_to_grid(mousepos,sprites_x, sprites_y, sprites_w, sprites_h, sprites_cell)
        if spritespos.x and spritespos.y then
            spriteNumber = pageNumber*sprites_w*sprites_h + flr(spritespos.x + spritespos.y * sprites_w)
        end
    end

    if _btnp(_keys.S) then
        pageNumber = clamp(0,pageNumber + 1,6)
    end
    if _btnp(_keys.W) then
        pageNumber = clamp(0,pageNumber - 1,6)
    end
end

function mapeditor:draw()
    _rectfill(10,0,310,180,11)
    _rectfill(map_x - 1, map_y - 1,30*10 + 2,110 + 2, 0)
    _csprc(1,0,map_x,map_y,3,2,30,11)
    
    _print("SPR#:",sprites_x,sprites_y - 8, 12)
    _print(tostring(spriteNumber),sprites_x + 20,sprites_y - 8, 1)
    _print("PAG#:",sprites_x + 40,sprites_y - 8, 12)
    _print(tostring(pageNumber),sprites_x + 60,sprites_y - 8, 1)

    _rectfill(sprites_x - 1, sprites_y - 1,sprites_w*10 + 2,sprites_h*10 + 2, 0)
    _csprc(1,0,sprites_x,sprites_y,3,2,sprites_w,sprites_h)
    _cgridc(pageNumber*sprites_w*sprites_h,sprites_x,sprites_y,1,-1,10,sprites_w,sprites_h,false,false)
    drawSelectedRec(spriteNumber, pageNumber, sprites_w, sprites_h, sprites_x, sprites_y, sprites_cell)
end

return mapeditor