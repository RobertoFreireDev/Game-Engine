-- Sfx.lua
-- Lua helper for editing SFX strings
-- Each note = "PPWVV" (Pitch(2 digits) Wave(1 digit) Volume(2 digits))
-- Speed = "SS" (2 digits)

function newSfx(speed)
    local o = {
         notes = {},
         speed = speed
    }

    function o:updateNote(index, pitch, wave, volume)
        self.notes[index] = {
            pitch = clamp(36, pitch, 71),
            wave  = clamp(0, wave, 4),
            volume= clamp(0, volume, 10)
        }
    end

    function o:toString()
        local s = ""
        for _, n in ipairs(self.notes) do
            s=s..string.format("%02d%d%02d", n.pitch, n.wave, n.volume)
        end
        return s
    end

    function o.fromString(str)
        local self = Sfx.new()
        self.notes = {}

        local len = #str - 2
        local speed = tonumber(str:sub(len+1, len+2)) or 10
        self.speed = speed

        for i = 1, len, 5 do
            local pitch = tonumber(str:sub(i, i+1)) or 0
            local wave  = tonumber(str:sub(i+2, i+2)) or 0
            local vol   = tonumber(str:sub(i+3, i+4)) or 0
            table.insert(self.notes, {pitch=pitch, wave=wave, volume=vol})
        end
        return self
    end

    return o
end