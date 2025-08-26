local spriteeditor = {
    firsttime = true,
    collorButtons = {},
    paintbuttons = {},
    selectedcolor = 0,
    cell = 12,
    grid_w = 10,
    grid_h = 10,
    sprite_x = 275,
    sprites_x = 15,
    sprites_y = 135,
    origin_x = 100,
    origin_y = 5,
    pixelbutton = new_button(0,11,12,10,275,105,0,0,10,10),
    eraserbutton = new_button(0,10,12,10,275+10,105,0,0,10,10),
    sprites_w = 30,4,10,
    sprites_h = 4,
    sprites_cell = 10,
    spriteNumber = 1,
    pageNumber = 0,
    zoom = 1,
    maxZoom = 4
}

function spriteeditor:create()
    local x,y,size=self.sprite_x,self.origin_y,10
    for i=0,31 do
	    local row = flr(i/4)
	    local col = i%4
	    local px = x + col*size
	    local py = y + row*size
        local cbtn = new_colorbutton(i,px,py,0,0,size,size)
        cbtn.clicked = function(o)
            if self.paintbuttonselected == self.eraserbutton then
                return
            end
            self.selectedcolor = o.c  
        end 
        add(self.collorButtons,cbtn)
    end    
    self.eraserbutton.clicked = function(o) self.paintbuttonselected = o self.selectedcolor = -1 end    
    self.pixelbutton.clicked = function(o) 
        self.paintbuttonselected = o
        if self.selectedcolor == -1 then
            self.selectedcolor = 0
        end
    end
    add(self.paintbuttons,self.eraserbutton)
    add(self.paintbuttons,self.pixelbutton)
end

function spriteeditor:init()
    if self.firsttime then
        self:create()
        self.paintbuttonselected = self.pixelbutton
        self.firsttime = false
    end
end

function spriteeditor:update()
    foreach(self.collorButtons, function(o)
        o:update()
    end)
    foreach(self.paintbuttons, function(o)
        o:update()
        o.b.c = self.paintbuttonselected == o and 13 or 12
    end)

    self.pageNumber = movepage(0,self.pageNumber,const.maxPage)
    self.zoom = mousescroll(1,self.zoom,self.maxZoom)

    if _mouseclick(0) then
        local mousepos = _mousepos()
        local gridpos = screen_to_grid(mousepos,self.origin_x, self.origin_y, self.grid_w*self.zoom, self.grid_h*self.zoom, self.cell/self.zoom)
        if gridpos.x and gridpos.y then            
            _spixel(
                (self.spriteNumber  % self.sprites_w) * self.sprites_cell + gridpos.x,
                flr(self.spriteNumber / self.sprites_w) * self.sprites_cell + gridpos.y,
                self.selectedcolor)
        else
            local spritespos = screen_to_grid(mousepos,self.sprites_x, self.sprites_y, self.sprites_w, self.sprites_h, self.sprites_cell)
            local sn = updateSpriteNumber(spritespos,self.spriteNumber,self.pageNumber,self.sprites_w,self.sprites_h)
            if sn > 0 then
                self.spriteNumber = sn
            end
        end
    end
end

function spriteeditor:draw()
    _rectfill(10,0,310,180,11)
    _rectfill(self.sprite_x-1,self.origin_y-1,42,82,0)
    _rectfill(self.sprite_x-1,89-1,42,12,0)
    _csprc(1,0,self.sprite_x,89,3,2,4,1)
    _rectfill(self.sprite_x,89,40,10,self.selectedcolor)
    foreach(self.collorButtons, function(o)
        o:draw()
    end)
    foreach(self.paintbuttons, function(o)
        o:draw()
    end)

    _rectfill(self.origin_x - 1, self.origin_y - 1,self.grid_w * self.cell + 2,self.grid_h * self.cell + 2, 0)    
    _csprc(1,0,self.origin_x,self.origin_y,3,2,self.cell,self.cell)
    _cgridc(self.spriteNumber,self.origin_x,self.origin_y,self.cell/self.zoom,-1,10,self.zoom,self.zoom,false,false)    

    drawPageSpriteNumbers(self.spriteNumber,self.pageNumber,self.sprites_x,self.sprites_y)
    
    _rectfill(self.sprites_x - 1, self.sprites_y - 1,self.sprites_w*self.sprites_cell + 2,self.sprites_h*self.sprites_cell + 2, 0)
    _csprc(1,0,self.sprites_x,self.sprites_y,3,2,self.sprites_w,self.sprites_h)     
    _cgridc(self.pageNumber*self.sprites_w*self.sprites_h,self.sprites_x,self.sprites_y,1,-1,10,self.sprites_w,self.sprites_h,false,false)
    drawSelectedRec(self.spriteNumber, self.pageNumber, self.sprites_w, self.sprites_h, self.sprites_x, self.sprites_y, self.sprites_cell)
end

return spriteeditor