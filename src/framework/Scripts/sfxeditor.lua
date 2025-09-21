local sfxeditor={
    firsttime = true,
    sfxIndex = 0,
}

function sfxeditor:create()
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
        _setnotesfx(self.sfxIndex, 2, "64110")
    end
end

function sfxeditor:draw()
    _print("Sfx",10,2,1)
    local sound = _getsfx(self.sfxIndex)
    _print(sound,10,12,1)
end

return sfxeditor