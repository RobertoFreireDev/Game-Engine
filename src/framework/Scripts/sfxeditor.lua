local sfxeditor={
    firsttime = true,
    sfxIndex = 0,
    notes = {},
    notepos = { x = 20, y = 80, r = 12 },
    octaves = {},
    octpos = { x = 20, y = 105, r = 3 },
    waves = {},
    wavepos = { x = 20, y = 135, r = 4 },
    vol = {},
    volpos = { x = 20, y = 175, r = 6 },
    step = { x = 12, y = 5 } 
}

function sfxeditor:create()
    local sound = _getsfx(self.sfxIndex)
    for i=1,24 do
        add(self.notes,new_verticalbar(self.notepos.x + (i-1)*self.step.x,self.notepos.y,self.notepos.r,self.step.x,self.step.y))
        add(self.octaves,new_verticalbar(self.octpos.x + (i-1)*self.step.x,self.octpos.y,self.octpos.r,self.step.x,self.step.y))
        add(self.waves,new_verticalbar(self.wavepos.x + (i-1)*self.step.x,self.wavepos.y,self.wavepos.r,self.step.x,self.step.y))
        add(self.vol,new_verticalbar(self.volpos.x + (i-1)*self.step.x,self.volpos.y,self.volpos.r,self.step.x,self.step.y))
    end
    self:loadsfx(sound)
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
        self:updatenote()
    end
end

function sfxeditor:updatenote()
    for i = 1, #self.notes do        
        local n = self.notes[i]:update(_mousepos())
        if n >= 0 then
            self:setnote(
                i,
                n,
                self.octaves[i].value,
                self.waves[i].value,
                self.vol[i].value)
            return
        end

        local o = self.octaves[i]:update(_mousepos())
        if o >= 0 then
            self:setnote(
                i,
                self.notes[i].value,
                o,
                self.waves[i].value,
                self.vol[i].value)
            return
        end

        local e = self.waves[i]:update(_mousepos())
        if e >= 0 then
            self:setnote(
                i,
                self.notes[i].value,
                self.octaves[i].value,
                e,
                self.vol[i].value)
            return
        end

        local v = self.vol[i]:update(_mousepos())
        if v >= 0 then
            self:setnote(
                i,
                self.notes[i].value,
                self.octaves[i].value,
                self.waves[i].value,
                v)
            return
        end
    end
end

function sfxeditor:setnote(i,n,o,e,v)
    v = v == 5 and "10" or "0"..tostring(v*2)
    _setnotesfx(self.sfxIndex, i-1, tostring(36+n + o*12)..tostring(e+1)..v)
end

function sfxeditor:loadsfx(str)
    local len = #str - 2
    local count = 1
    for i = 1, len, 5 do
        local note = (tonumber(str:sub(i, i+1)) or 0) - 36
        local octave = flr(note/12)
        local pitch = note % 12
        local wave  = tonumber(str:sub(i+2, i+2)) or 0
        local vol   = tonumber(str:sub(i+3, i+4)) or 0
        self.notes[count].value = pitch 
        self.octaves[count].value = octave
        self.waves[count].value = wave 
        self.vol[count].value = flr(vol/2)
        count = count + 1
    end
end

function sfxeditor:draw()
    _rectfill(0,0,320,180,11)
    self:drawSection("Notes", self.notepos)
    self:drawSection("Octaves", self.octpos)
    self:drawSection("Wave", self.wavepos)
    self:drawSection("Volume", self.volpos)

    for i = 1, #self.notes do
        self.notes[i]:draw()
        self.octaves[i]:draw()
        self.waves[i]:draw()
        self.vol[i]:draw()
    end
end

function sfxeditor:drawSection(label, pos)
    _print(label, pos.x, pos.y - pos.r*self.step.y - 8, 12)
    _rectfill(
        pos.x-2,
        pos.y-2 - pos.r*self.step.y,
        24*self.step.x+4,
        pos.r*self.step.y+4,
        3
    )
    _rectfill(
        pos.x-1,
        pos.y-1 - pos.r*self.step.y,
        24*self.step.x+2,
        pos.r*self.step.y+2,
        12
    )
end

return sfxeditor