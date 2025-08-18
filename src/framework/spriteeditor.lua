spriteeditor,collorButtons,paintbuttons={},{},{}
selectedcolor = 0
gridIndex, cell, grid_w, grid_h = 0, 10 , 10, 10
origin_x, origin_y = 100, 20  -- top-left of grid on screen
eraserbutton = new_button(0,10,12,10,20,20,0,0,10,10)
pixelbutton = new_button(0,11,12,10,30,20,0,0,10,10)

function createSpriteEditor()
    local x,y,size=270,20,10
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
    _creategrid(gridIndex)
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

    local mousepos = screen_to_grid(_mousepos())
    if _mouseclick(0) and mousepos.x and mousepos.y then
        _spixel(gridIndex,mousepos.x,mousepos.y,selectedcolor)
    end
end

function spriteeditor:draw()
    _rectfill(10,10,310,170,11)
    _rectfill(270-1,20-1,42,42,0)
    _rectfill(270-1,64-1,42,12,0)
    _cspr(1,0,270,64,4,1)
    _rectfill(270,64,40,10,selectedcolor)
    foreach(collorButtons, function(o)
        o:draw()
    end)
    foreach(paintbuttons, function(o)
        o:draw()
    end)

    _rectfill(origin_x - 1, origin_y - 1,grid_w * cell + 2,grid_h * cell + 2, 0)    
    _cspr(1,0,origin_x,origin_y,10,10)
    _cgridc(gridIndex,0,origin_x,origin_y,cell,-1,10,1,1,false,false)
end

function screen_to_grid(p)
    local gx = flr((p.x - origin_x) / cell)
    local gy = flr((p.y - origin_y) / cell)

    if gx < 0 or gx > grid_w or gy < 0 or gy > grid_h then
        return { x=nil, y=nil}
    end

    return { x=gx, y=gy}
end

return spriteeditor