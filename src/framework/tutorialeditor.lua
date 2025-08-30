local tutorialeditor={
    firsttime = true,
}

function tutorialeditor:create()
end

function tutorialeditor:init()
    if self.firsttime then
        self:create()
        self.firsttime = false
    end
end

function tutorialeditor:update()   
    
end

function tutorialeditor:draw()
   _print("Tutorial",10,2,1)
end

return tutorialeditor