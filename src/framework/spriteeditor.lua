local spriteeditor,collorButtons,paintbuttons={},{},{}
local selectedcolor = 0
local cell, grid_w, grid_h = 12 , 10, 10
local sprite_x = 275
local sprites_x, sprites_y = 15,135
local origin_x, origin_y = 100, 5
local pixelbutton = new_button(0,11,12,10,sprite_x,65,0,0,10,10)
local eraserbutton = new_button(0,10,12,10,sprite_x+10,65,0,0,10,10)
local sprites_w,sprites_h,sprites_cell=30,4,10
local spriteNumber = 0
local pageNumber = 0

function createSpriteEditor()
    local x,y,size=sprite_x,origin_y,10
    for i=0,15 do
	    local row = flr(i/4)
	    local col = i%4
	    local px = x + col*size
	    local py = y + row*size
        local cbtn = new_colorbutton(i,px,py,0,0,size,size)
        cbtn.clicked = function(o)
            if paintbuttonselected == eraserbutton then
                return
            end
            selectedcolor = o.c  
        end 
        add(collorButtons,cbtn)
    end    
    eraserbutton.clicked = function(o) paintbuttonselected = o selectedcolor = -1 end    
    pixelbutton.clicked = function(o) 
        paintbuttonselected = o
        if selectedcolor == -1 then
            selectedcolor = 0
        end
    end
    add(paintbuttons,eraserbutton)
    add(paintbuttons,pixelbutton)
end

createSpriteEditor()
paintbuttonselected = pixelbutton

function spriteeditor:init()
end

function spriteeditor:update()
    foreach(collorButtons, function(o)
        o:update()
    end)
    foreach(paintbuttons, function(o)
        o:update()
        o.b.c = paintbuttonselected == o and 13 or 12
    end)

    pageNumber = movepage(pageNumber)

    if _mouseclick(0) then
        local mousepos = _mousepos()
        local gridpos = screen_to_grid(mousepos,origin_x, origin_y, grid_w, grid_h, cell)
        if spriteNumber > 0  and gridpos.x and gridpos.y then            
            _spixel(
                (spriteNumber  % sprites_w) * sprites_cell + gridpos.x,
                flr(spriteNumber / sprites_w) * sprites_cell + gridpos.y,
                selectedcolor)
        else
            local spritespos = screen_to_grid(mousepos,sprites_x, sprites_y, sprites_w, sprites_h, sprites_cell)
            spriteNumber = updateSpriteNumber(spritespos,spriteNumber,pageNumber,sprites_w,sprites_h)
        end
    end
end

function spriteeditor:draw()
    _rectfill(10,0,310,180,11)
    _rectfill(sprite_x-1,origin_y-1,42,42,0)
    _rectfill(sprite_x-1,49-1,42,12,0)
    _csprc(1,0,sprite_x,49,3,2,4,1)
    _rectfill(sprite_x,49,40,10,selectedcolor)
    foreach(collorButtons, function(o)
        o:draw()
    end)
    foreach(paintbuttons, function(o)
        o:draw()
    end)

    _rectfill(origin_x - 1, origin_y - 1,grid_w * cell + 2,grid_h * cell + 2, 0)    
    _csprc(1,0,origin_x,origin_y,3,2,cell,cell)
    _cgridc(spriteNumber,origin_x,origin_y,cell,-1,10,1,1,false,false)    

    drawPageSpriteNumbers(spriteNumber,pageNumber,sprites_x,sprites_y)
    
    _rectfill(sprites_x - 1, sprites_y - 1,sprites_w*sprites_cell + 2,sprites_h*sprites_cell + 2, 0)
    _csprc(1,0,sprites_x,sprites_y,3,2,sprites_w,sprites_h)     
    _cgridc(pageNumber*sprites_w*sprites_h,sprites_x,sprites_y,1,-1,10,sprites_w,sprites_h,false,false)
    drawSelectedRec(spriteNumber, pageNumber, sprites_w, sprites_h, sprites_x, sprites_y, sprites_cell)
end

return spriteeditor