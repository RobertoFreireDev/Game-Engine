local sfxeditor={
    firsttime = true,
    sfxIndex = 0,
    notes = {}
}

function sfxeditor:create()
    add(self.notes,new_verticalbar(20,140,13,10,5))
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
    foreach(self.notes, function(n)
        n:draw()
    end)
end

return sfxeditor