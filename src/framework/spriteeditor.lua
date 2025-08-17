spriteeditor={}

function spriteeditor:init()
end

function spriteeditor:update()
end

function spriteeditor:draw()
    _rectfill(10,10,310,170,11)
    _print("Sprite Editor", 12, 2, 11) 
    draw_palette(270,20,10)
end

function draw_palette(x,y,size)
    _rectfill(270-1,20-1,42,42,0)
	for i=0,15 do
		local row = flr(i/4)
		local col = i%4
		local px = x + col*size
		local py = y + row*size 
        _rectfill(px,py,size,size,i)
	end
end

return spriteeditor