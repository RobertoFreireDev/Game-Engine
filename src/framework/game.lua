p = {}
p.x = 8
p.y = 8

local left, up, right, down	= 37, 38, 39, 40

function _init()
    _stimer(2)
    _bckgdclr(2)
end

function _update()
    local q,w = 81,87
    if _btn(left) then p.x = p.x - 1 _mouseshow(false) end
    if _btn(right) then p.x = p.x + 1 _mouseshow(true) end
    if _btn(up) then p.y = p.y - 1 end
    if _btn(down) then p.y = p.y + 1 end
    _camera(p.x, p.y)
end

function _draw()
    _rect(0, 0, 320, 180, 5, 10)
end