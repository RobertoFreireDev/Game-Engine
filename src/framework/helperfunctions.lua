function updateSpriteNumber(sp,sn,pn,w,h)
    if sp.x and sp.y then
        sn = pn*w*h + flr(sp.x + sp.y*w)
    end
    return sn
end

function movearrows(minX,minY,maxX,maxY,pos)
    if _btn(_keys.A) then
        pos.x = max(minX,pos.x-1)
    end
    if _btn(_keys.D) then
        pos.x = min(pos.x+1,maxX)
    end
    if _btn(_keys.W) then
        pos.y = max(minY,pos.y-1)
    end
    if _btn(_keys.S) then
        pos.y = min(pos.y+1,maxY)
    end

    return pos
end

function movepage(a,pn,b)
    if _btnp(_keys.E) then
        pn = min(pn + 1,b)
    end
    if _btnp(_keys.Q) then
        pn = max(a,pn - 1)
    end

    return pn
end

function mousescroll(a,ms,b)
    if _mousescroll(1) then
        ms = max(a,ms - 1)
    end
    if _mousescroll(0) then
        ms = min(ms + 1,b)
    end

    return ms
end

function drawPageSpriteNumbers(sn,pn,x,y)
    _print("SPR#:",x,y - 8, 12)
    _print(tostring(sn),x + 20,y - 8, 1)
    _print("PAG#:",x + 35,y - 8, 12)
    _print(tostring(pn),x + 55,y - 8, 1)
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