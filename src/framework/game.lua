function _init()
    _stimer(2)
    _bckgdclr(2)
end

function _update()
    local q,w = 81,87
    if _btnp(q) then  
        _stimer(2)
    end
    if _btnp(w) then  
    end
end

function _draw()
    _rect(0, 0, 320, 180, 5, 10)
    _print(tostring(_gtime()),2,2,1)
    _print(tostring(_gtimer(2,2)),2,10,1)
end