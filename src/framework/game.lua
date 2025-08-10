function _init()
    _loadsfx()
    _configsfx(0,32,"64110360006411065110671106711036000651106411062110360006011060110");
end

function _update()
    local q,w = 81,87
    if _btnp(q) then
        _savesfx()
        --_playsfx(0,8)    
    end
    if _btnp(w) then
        _playsfx(0)    
    end
end

function _draw()
    _rect(0, 0, 320, 180, 5)
end