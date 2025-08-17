spriteeditor={}
spriteobjects = {}
selectedcolor = 0

function spriteeditor:init()
    spriteobjects = {}
    local x,y,size=270,20,10
    for i=0,15 do
		local row = flr(i/4)
		local col = i%4
		local px = x + col*size
		local py = y + row*size
        local cbtn = new_colorbutton(i,px,py,0,0,size,size)
        cbtn.clicked = function(o) selectedcolor = o.c  end 
        add(spriteobjects,cbtn)
	end
end

function spriteeditor:update()
    foreach(spriteobjects, function(o)
        o:update()
    end)
end

function spriteeditor:draw()
    _rectfill(10,10,310,170,11)
    _rectfill(270-1,20-1,42,42,0)
    _rectfill(270-1,64-1,42,12,0)
    _rectfill(270,64,40,10,selectedcolor)
    _print("Sprite Editor", 12, 2, 11)
    foreach(spriteobjects, function(o)
        o:draw()
    end)
end

return spriteeditor