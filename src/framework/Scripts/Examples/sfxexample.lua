local sfxexample={
    title = "Sfx example"
}

function sfxexample:init()

end

function sfxexample:update() 

end

function sfxexample:draw()

end

return sfxexample

--[[
    🔊 SFX Functions

    | Function                | Lua Alias |
    | ------------------------------------------------------------------------------------------ | -------------- |
    | `SetSfxSpeed(int index, int speed = 1)`                                                    | `_spdsfx`      |
    | `PlaySfx(int index, int speed = 1, int channel = -1, int offset = 0)`                      | `_playsfx`     |
]]