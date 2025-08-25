local mapeditor={
    firsttime = true,
    map_x = 15, 
    map_y = 5, 
    map_pos = {x=0,y=0},
    map_columns = 30,
    map_rows = 12,
    sprites_x = 15, 
    sprites_y = 135,
    sprites_w = 30,
    sprites_h = 4,
    sprites_cell= 10,
    spriteNumber = 0,
    pageNumber = 0,
    maxPage = 7,
}

function mapeditor:create()
end

function mapeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function mapeditor:update()   
    if _mouseclick(0) then
        local mousepos = _mousepos()
        local gridpos = screen_to_grid(mousepos,self.map_x, self.map_y, self.map_columns, self.map_rows, self.sprites_cell)
        if gridpos.x and gridpos.y then
            _stilemap(self.map_pos.x + gridpos.x,self.map_pos.y + gridpos.y,self.spriteNumber)
        else
            local spritespos = screen_to_grid(mousepos,self.sprites_x, self.sprites_y, self.sprites_w, self.sprites_h, self.sprites_cell)
            self.spriteNumber = updateSpriteNumber(spritespos,self.spriteNumber,self.pageNumber,self.sprites_w,self.sprites_h)
        end
    end

    self.pageNumber = movepage(0,self.pageNumber,self.maxPage)
    self.map_pos = movearrows(0,0,320-self.map_columns,180-self.map_rows,self.map_pos)
end

function mapeditor:draw()
    _rectfill(10,0,310,180,11)
    _rectfill(self.map_x - 1, self.map_y - 1,self.map_columns*10 + 2,self.map_rows*10 + 2, 0)
    _csprc(1,0,self.map_x,self.map_y,3,2,self.map_columns,self.map_rows)
    _drawmap(self.map_pos.x, self.map_pos.y, self.map_x,self.map_y, self.map_columns, self.map_rows);
    _print("("..self.map_pos.x..","..self.map_pos.y..")",80,self.sprites_y - 8,12)

    drawPageSpriteNumbers(self.spriteNumber,self.pageNumber,self.sprites_x,self.sprites_y)

    _rectfill(self.sprites_x - 1, self.sprites_y - 1,self.sprites_w*10 + 2,self.sprites_h*10 + 2, 0)
    _csprc(1,0,self.sprites_x,self.sprites_y,3,2,self.sprites_w,self.sprites_h)
    _cgridc(self.pageNumber*self.sprites_w*self.sprites_h,self.sprites_x,self.sprites_y,1,-1,10,self.sprites_w,self.sprites_h,false,false)
    drawSelectedRec(self.spriteNumber, self.pageNumber, self.sprites_w, self.sprites_h, self.sprites_x, self.sprites_y, self.sprites_cell)
end

return mapeditor