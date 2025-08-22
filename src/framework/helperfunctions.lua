function updateSpriteNumber(sp,sn,pn,w,h)
    if sp.x and sp.y then
        sn = pn*w*h + flr(sp.x + sp.y*w)
    end
    return sn
end

function movepage(pn)
    if _btnp(_keys.S) then
        pn = clamp(0,pn + 1,6)
    end
    if _btnp(_keys.W) then
        pn = clamp(0,pn - 1,6)
    end

    return pn
end

function drawPageSpriteNumbers(sn,pn,x,y)
    _print("SPR#:",x,y - 8, 12)
    _print(tostring(sn),x + 20,y - 8, 1)
    _print("PAG#:",x + 40,y - 8, 12)
    _print(tostring(pn),x + 60,y - 8, 1)
end

function drawSelectedRec(sn,pn,w,h,sx,sy,sc)
    if sn < pn*w*h or sn >= (pn+1)*w*h  then
        return
    end
    local sn = sn - pn*w*h
    local x = (sn  % w) * sc
    local y = flr(sn / w) * sc
    _rect(sx + x,sy + y,sc,sc,1)
end

function screen_to_grid(p,x,y,w,h,s) 
    local gx = flr((p.x - x) / s)
    local gy = flr((p.y - y) / s)

    if gx < 0 or gx >= w or gy < 0 or gy >= h then
        return { x=nil, y=nil}
    end

    return { x=gx, y=gy}
end