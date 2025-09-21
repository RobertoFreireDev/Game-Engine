local sfxeditor={
    firsttime = true,
    sfxIndex = 0,
    notes = {},
    notepos = { x = 40, y = 75 }
}

function sfxeditor:create()
    for i=1,24 do
        add(self.notes,new_verticalbar(self.notepos.x + (i-1)*10,self.notepos.y,13,10,5))
    end
end

function sfxeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function sfxeditor:update()   
    if _btnp(_keys.Space) then
        _playsfx(0,8)    
    end

    if _btnp(_keys.Q) then
        
    end

    if _mouseclick(0) then
        for i = 1, #self.notes do
            local v = self.notes[i]:update(_mousepos())
            if v == 0 then
                _setnotesfx(self.sfxIndex, i-1, "36100") -- empty note
            elseif v > 0 then
                _setnotesfx(self.sfxIndex, i-1, tostring(35+v).."110")
            end
        end
    end
end

function sfxeditor:draw()
    _rectfill(0,0,320,180,11)
    _rectfill(self.notepos.x-2,self.notepos.y-2 - 12*5,24*10+4,12*5+4,12)
    foreach(self.notes, function(n)
        n:draw()
    end)
end

return sfxeditor