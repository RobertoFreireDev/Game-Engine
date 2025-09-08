require("Sfx")

local sfxeditor={
    firsttime = true,
    sounds = {}
}

function sfxeditor:create()
    local s = newSfx(2)
    s:addNote(64, 1, 10)
    s:addNote(36, 0, 00)
    s:addNote(64, 1, 10)
    s:addNote(65, 1, 10)
    s:addNote(67, 1, 10)
    s:addNote(36, 1, 10)
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
        _configsfx(0,self.sounds[1].speed,self.sounds[1]:toString());
        _playsfx(0,self.sounds[1].speed)    
    end
end

function sfxeditor:draw()
    _print("Sfx",10,2,1)
end

return sfxeditor