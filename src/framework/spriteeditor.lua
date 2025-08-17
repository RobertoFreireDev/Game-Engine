spriteeditor={}
collorButtons = {}
selectedcolor = 0

-- config
grid_w, grid_h = 10, 10
cell = 10             -- size of each cell in pixels
origin_x, origin_y = 100, 20  -- top-left of grid on screen
pixels = {}

function spriteeditor:init()
    collorButtons = {}
    local x,y,size=270,20,10
    for i=0,15 do
		local row = flr(i/4)
		local col = i%4
		local px = x + col*size
		local py = y + row*size
        local cbtn = new_colorbutton(i,px,py,0,0,size,size)
        cbtn.clicked = function(o) selectedcolor = o.c  end 
        add(collorButtons,cbtn)
	end

    for y=1,grid_h do
        pixels[y] = {}
            for x=1,grid_w do
            pixels[y][x] = nil
        end
    end
end

function spriteeditor:update()
    foreach(collorButtons, function(o)
        o:update()
    end)

    local mousepos = screen_to_grid(_mousepos())
    if _mouseclick(0) and mousepos.x and mousepos.y then
        pixels[mousepos.y][mousepos.x] = selectedcolor
    end
end

function spriteeditor:draw()
    _rectfill(10,10,310,170,11)
    _rectfill(270-1,20-1,42,42,0)
    _rectfill(270-1,64-1,42,12,0)
    _rectfill(270,64,40,10,selectedcolor)
    foreach(collorButtons, function(o)
        o:draw()
    end)

    _rectfill(origin_x - 1, origin_y - 1,grid_w * cell + 2,grid_h * cell + 2, 0)    
    _cspr(1,0,origin_x,origin_y)

    for y=1,grid_h do
        for x=1,grid_w do
            local px = origin_x + (x-1)*cell
            local py = origin_y + (y-1)*cell
            if pixels[y][x] ~= nil then
                _rectfill(px, py,cell,cell, pixels[y][x])
            end
        end
    end
end

function screen_to_grid(p)
    local gx = flr((p.x - origin_x) / cell) + 1
    local gy = flr((p.y - origin_y) / cell) + 1

    if gx < 1 or gx > grid_w or gy < 1 or gy > grid_h then
        return { x=nil, y=nil}
    end

    return { x=gx, y=gy}
end

return spriteeditor