local sfxeditor={
    firsttime = true,
    sfxIndex = 1,
    sounds = {}
}

function new_sfx()
    local sfx = {
        speed = 8,
        notes = {},
        octaves = {},
        waves = {},
        vol = {},
        notepos = { x = 20, y = 80, r = 12 },
        octpos = { x = 20, y = 105, r = 3 },
        wavepos = { x = 20, y = 135, r = 4 },
        volpos = { x = 20, y = 175, r = 6 },
        step = { x = 12, y = 5 } 
    }

    function sfx:load()
        self.notes = {}
        self.octaves = {}
        self.waves = {}
        self.vol = {}
        for i=1,24 do
            add(self.notes,new_verticalbar(self.notepos.x + (i-1)*self.step.x,self.notepos.y,self.notepos.r,self.step.x,self.step.y))
            add(self.octaves,new_verticalbar(self.octpos.x + (i-1)*self.step.x,self.octpos.y,self.octpos.r,self.step.x,self.step.y))
            add(self.waves,new_verticalbar(self.wavepos.x + (i-1)*self.step.x,self.wavepos.y,self.wavepos.r,self.step.x,self.step.y))
            add(self.vol,new_verticalbar(self.volpos.x + (i-1)*self.step.x,self.volpos.y,self.volpos.r,self.step.x,self.step.y))
        end

        local str = _getsfx(sfxeditor.sfxIndex-1)
        local len = #str - 2
        local count = 1
        for i = 1, len, 5 do
            local note = (tonumber(str:sub(i, i+1)) or 0) - 36
            local octave = flr(note/12)
            local pitch = note % 12
            local wave  = tonumber(str:sub(i+2, i+2))
            local vol   = tonumber(str:sub(i+3, i+4))
            self.notes[count].value = pitch 
            self.octaves[count].value = octave < 0 and 0 or octave
            self.waves[count].value = wave
            self.vol[count].value = flr(vol/2)
            count = count + 1
        end
    end

    function sfx:update()
        if _mouseclick(0) then        
            self:updatenote()
        end

        self:changespeed()
    end

    function sfx:updatenote()
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

            local w = self.waves[i]:update(_mousepos())
            if w >= 0 then
                self:setnote(
                    i,
                    self.notes[i].value,
                    self.octaves[i].value,
                    w,
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

    function sfx:changespeed()
        if _btnp(_keys.E) then
            self.speed = min(self.speed + 1,32)
        end
        if _btnp(_keys.Q) then
            self.speed = max(1,self.speed - 1)
        end
    end

    function sfx:setnote(i,n,o,w,v)
        v = v == 5 and "10" or "0"..tostring(v*2)
        _setnotesfx(sfxeditor.sfxIndex-1, i-1, tostring(36+n + o*12)..tostring(w)..v)
    end

    function sfx:draw()
        _rectfill(0,0,320,180,11)
        _print("Spd: "..self.speed.." Sfx: "..sfxeditor.sfxIndex-1, self.notepos.x, 2, 1)
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

    function sfx:drawSection(label, pos)
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

    return sfx
end

function sfxeditor:create()
    self.sounds = {}
    for i = 1, 64 do
        add(self.sounds,new_sfx())
    end
    self.sounds[self.sfxIndex]:load()
end

function sfxeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function sfxeditor:update()
    self.sounds[self.sfxIndex]:update()

    if _btnp(_keys.Space) then
        _playsfx(self.sfxIndex-1, self.sounds[self.sfxIndex].speed)    
    end

    if _btnp(_keys.W) then
        self.sfxIndex = max(1,self.sfxIndex-1)
        self.sounds[self.sfxIndex]:load()
    end
    if _btnp(_keys.S) then
        self.sfxIndex = min(self.sfxIndex+1,64)
        self.sounds[self.sfxIndex]:load()
    end
end

function sfxeditor:draw()
    _rectfill(0,0,320,180,11)
    self.sounds[self.sfxIndex]:draw()
end

return sfxeditor