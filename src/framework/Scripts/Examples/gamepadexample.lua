local gamepadexample={
    title = "GamePad example"
}

function newPlayer(x,y,i)
    local player = {
        x = x,
        y = y,
        i = i,
        c = (i + 1)*2,
        fc = (i + 1)*3,
        sc = (i + 1)*2,
        pr = true,
        r = 4,
        speed = 1.0
    }

    function player:update()
        local dx,dy=0,0
        if _gmpd(_gamepadbuttons.DPadLeft,self.i) or _gmpd(_gamepadbuttons.RightThumbstickLeft,self.i) or _gmpd(_gamepadbuttons.LeftThumbstickLeft,self.i) then
            dx = dx - self.speed
        end
        if _gmpd(_gamepadbuttons.DPadRight,self.i) or _gmpd(_gamepadbuttons.RightThumbstickRight,self.i) or _gmpd(_gamepadbuttons.LeftThumbstickRight,self.i) then
            dx = dx + self.speed
        end
        if _gmpd(_gamepadbuttons.DPadUp,self.i) or _gmpd(_gamepadbuttons.RightThumbstickUp,self.i) or _gmpd(_gamepadbuttons.LeftThumbstickUp,self.i) then
            dy = dy - self.speed
        end
        if _gmpd(_gamepadbuttons.DPadDown,self.i) or _gmpd(_gamepadbuttons.RightThumbstickDown,self.i) or _gmpd(_gamepadbuttons.LeftThumbstickDown,self.i) then
            dy = dy + self.speed
        end

        self.x = self.x + dx
        self.y = self.y + dy

        if _gmpd(_gamepadbuttons.LeftShoulder,self.i) or _gmpd(_gamepadbuttons.RightShoulder,self.i) or _gmpdp(_gamepadbuttons.A,self.i) or _gmpdp(_gamepadbuttons.B,self.i) or _gmpdr(_gamepadbuttons.X,self.i) or _gmpdr(_gamepadbuttons.Y,self.i) then
            self.c = self.pr and self.fc or self.sc
            self.pr = not self.pr
        end
    end

    function player:draw()    
        _circ(player.x, player.y, player.r, player.c, 10)
    end

    return player
end

players = {}

function gamepadexample:init()
    players = {}
    add(players,newPlayer(100,100,0))
    add(players,newPlayer(150,150,1))
end

function gamepadexample:update() 
    for i = 1, #players do
        players[i]:update()
    end
end

function gamepadexample:draw()
    for i = 1, #players do
        players[i]:draw()
    end
end

return gamepadexample