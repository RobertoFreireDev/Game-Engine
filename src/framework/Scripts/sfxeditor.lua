local sfxeditor={
    firsttime = true,
    sounds = {}
}

function sfxeditor:create()
    local s = newSfx(8)
    s:updateNote(1, 64, 1, 10)
    s:updateNote(2, 36, 0, 00)
    s:updateNote(3, 64, 1, 10)
    s:updateNote(4, 65, 1, 10)
    s:updateNote(5, 67, 1, 10)
    s:updateNote(6, 67, 1, 10)
    add(self.sounds,s)
end

function sfxeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function sfxeditor:update()   
    if _btnp(_keys.Space) then
        _setsfx(0,self.sounds[1].speed,self.sounds[1]:toString());
        _playsfx(0,self.sounds[1].speed)    
    end

    if _btnp(_keys.Q) then
        self.sounds[1]:updateNote(2, 64, 1, 10)
    end
end

function sfxeditor:draw()
    _print("Sfx",10,2,1)
end

return sfxeditor