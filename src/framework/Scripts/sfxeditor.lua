local sfxeditor={
    firsttime = true,
    sfxIndex = 0,
    notes = {},
    notepos = { x = 35, y = 75 },
    octaves = {},
    octpos = { x = 35, y = 120 },
}

function sfxeditor:create()
    local sound = _getsfx(self.sfxIndex)
    for i=1,24 do
        add(self.notes,new_verticalbar(self.notepos.x + (i-1)*10,self.notepos.y,13,10,5,false))
        add(self.octaves,new_verticalbar(self.octpos.x + (i-1)*10,self.octpos.y,3,10,5,true))
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
        local o = self.octaves[i]:update(_mousepos())
        if o >= 0 then
            self:setnote(
                i,
                self.notes[i].value,
                o,
                1,
                10)
            return
        end
        
        local n = self.notes[i]:update(_mousepos())
        if n >= 0 then
            self:setnote(
                i,
                n,
                self.octaves[i].value,
                1,
                10)
            return
        end
    end
end

function sfxeditor:setnote(i,n,o,e,v)
    if n == 0 then
        _setnotesfx(self.sfxIndex, i-1, "36100")
    end
    _setnotesfx(self.sfxIndex, i-1, tostring(35+n + o*12)..tostring(e)..tostring(v))
end

function sfxeditor:loadsfx(str)
    local len = #str - 2
    local count = 1
    for i = 1, len, 5 do
        self.notes[count].value = (tonumber(str:sub(i, i+1)) - 35) or 35
        count = count + 1
    end
end

function sfxeditor:draw()
    _rectfill(0,0,320,180,11)
    _rectfill(self.notepos.x-2,self.notepos.y-2 - 13*5,24*10+4,13*5+4,3)
    _rectfill(self.notepos.x-1,self.notepos.y-1 - 13*5,24*10+2,13*5+2,12)
    _rectfill(self.octpos.x-2,self.octpos.y-2 - 3*5,24*10+4,3*5+4,3)
    _rectfill(self.octpos.x-1,self.octpos.y-1 - 3*5,24*10+2,3*5+2,12)

    for i = 1, #self.notes do
        self.notes[i]:draw()
        self.octaves[i]:draw()
    end
end

return sfxeditor