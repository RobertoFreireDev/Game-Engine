local sfxeditor={
    firsttime = true,
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
    
end

function sfxeditor:draw()
    _print("Sfx",10,2,1)
end

return sfxeditor