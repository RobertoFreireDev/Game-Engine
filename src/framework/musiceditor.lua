local musiceditor={
    firsttime = true,
}

function musiceditor:create()
end

function musiceditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function musiceditor:update()  
end

function musiceditor:draw() 
    _print("Music",10,2,1)    
end

return musiceditor