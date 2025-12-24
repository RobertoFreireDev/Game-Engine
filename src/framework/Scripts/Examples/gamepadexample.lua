local gamepadexample={
    title = "GamePad example"
}

function gamepadexample:init()

end

function gamepadexample:update() 

end

function gamepadexample:draw()

end

return gamepadexample

--[[
    🖱️ Input Functions

    | Function                        | Lua Alias      |
    | ------------------------------- | -------------- |
    | `GamePadJustPressed(int buttonNumber, int playerIndex = 0)`       | `_gmpdp`         |
    | `GamePadReleased(int buttonNumber, int playerIndex = 0)`       | `_gmpdr`         |
    | `GamePadPressed(int buttonNumber, int playerIndex = 0)`       | `_gmpd`         |
]]