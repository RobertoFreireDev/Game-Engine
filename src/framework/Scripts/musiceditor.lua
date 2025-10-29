local musiceditor={
    firsttime = true,
}

function musiceditor:create()
    self.bars = {}
    local yv = 60
    for i = 1, 4 do
        add(self.bars,new_horizontalbar(30,yv,0,4,10,63))
        yv = yv + 20
    end
end

function musiceditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function musiceditor:update()
    if _mouseclick(0) then
        self:handleclick()
    end
    
    if _btnp(_keys.Space) then
        for i = 1, #self.bars do
            _playsfx(self.bars[i].value, 8,i) 
        end
    end
end

function musiceditor:handleclick()
    local mousepos = _mousepos()       
    for i = 1, #self.bars do
        local n = self.bars[i]:update(mousepos)
        if n >= 0 then
            return
        end
    end
end

function musiceditor:draw() 
    for i = 1, #self.bars do
        self.bars[i]:draw()
    end
end

return musiceditor