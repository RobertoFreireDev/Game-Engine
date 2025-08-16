p = {}
p.x = 8
p.y = 8

local left, up, right, down	= 37, 38, 39, 40

function _init()
    _stimer(2)
    _bckgdclr(2)
    _crtshader(true,105,110)
    _mouseshow(true)
    --_texture(32,32,"iVBORw0KGgoAAAANSUhEUgAAAEAAAAAgCAIAAAAt/+nTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEvSURBVFhH7ZbdDcIwDITTLsEoTMMzM/EK07AJLAG15CiNmjSJL+5f4BMSFCHH9p0dOnN/mSPT2/fDkingeznRyz7kuJ17etmHJOUxs7RlIcXGAEQFzEoKKkCldo+3fdiU2fompU/aU5h9tKPhlwAuzn+NKgHLUluAih+I6/NjPwkZj3euEhE9WHR7lJCIJpsBDrST/cOkWh7WTanvKnsiVQCQa8KEWtMy8SeyRikVeObU2fgeCIeKhSpv0FgAxcL8LT0SIKF50zcx1c3dBQg32EI0rcAS1Kga5ScVWM3fc/gJRArIShzdturecISR/QTWsBAdX35LiH5M7GUG4GsUFF3dLaLsfVM1t4X84hitnRNGVqFWAc6p5p8cN2jOQtnZWNBC8FyK0NfUsUL2xpgBjYSHlV2i330AAAAASUVORK5CYII=")
    _texture(32,32,"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")
end

function _update()
    local q,w = 81,87
    if _btn(left) then p.x = p.x - 1  end
    if _btn(right) then p.x = p.x + 1 end
    if _btn(up) then p.y = p.y - 1 end
    if _btn(down) then p.y = p.y + 1 end

    if _btn(q) then _crtshader(true,105,110) end
    if _btn(w) then _crtshader(false) end
    _camera(p.x,p.y)
end

function _draw()
    _rect(0, 0, 320, 180, 5, 10)
    _rectfill(100, 50, 50, 50, 10, 10)
    _circ(80, 150, 30, 2, 10)
    _circfill(150, 80, 30, 2, 10)
    _cspre(0, p.x + 120, p.y+ 32, _gelapsedtime(),"441100", 10, 3, 4, 4, false, false)
    _csprc(0, p.x + 32, p.y+ 32, 5, 5, 3, 3, false, false)
    _print(tostring(_gtime()),2,2,1)
    _print(tostring(_isfocused()),2,30,11)
    _print(tostring(_gtimer(2,2)),2,40,11)
    _print(tostring(_gtimer(2)),2,50,11)
    _print(tostring(_gtimer(3,10)),2,60,11)
end