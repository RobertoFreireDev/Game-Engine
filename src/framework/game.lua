-- screen
screen={x0=80,y0=10,x1=178,y1=150}

-- paddle
pw=24
ph=3
px=(screen.x0+screen.x1)/2-pw/2
py=screen.y1-6

-- ball
bx=(screen.x0+screen.x1)/2
by=(screen.y0+screen.y1)/2
br=2
bvx=1
bvy=-1

-- bricks
bricks={}

local game={
}

function game:init()
    -- create bricks
    for i=0,7 do
        for j=0,3 do
            local bw=20
            local bh=8
            local bxpos=screen.x0+8+i*bw
            local bypos=screen.y0+8+j*bh
            add(bricks,{x=bxpos,y=bypos,w=bw-2,h=bh-2,alive=true})
        end
    end
end

function game:update()   
    if _btn(_keys.A) then px=px-2 end
    if _btn(_keys.D) then px=px+2 end
    px=mid(screen.x0,px,screen.x0+screen.x1-pw - 1)

    -- move ball
    bx=bx+bvx
    by=by+bvy

    -- wall bounce
    if bx<br+screen.x0 then bvx=abs(bvx)
    elseif bx>screen.x0+screen.x1-br then bvx=-abs(bvx) end
    if by<br+screen.y0 then bvy=abs(bvy) end

    -- paddle bounce
    if by+br>=py and bx>px and bx<px+pw then
        bvy=-abs(bvy)
        bx=bx+(bx-(px+pw/2))/12 -- add angle
    end

    -- brick collision
    for b in all(bricks) do
        if b.alive and
           bx> b.x and bx< b.x+b.w and
           by> b.y and by< b.y+b.h then
            b.alive=false
            bvy=-bvy
        end
    end

    -- lose
    if by>screen.y1 then
        bx=(screen.x0+screen.x1)/2
        by=(screen.y0+screen.y1)/2
        bvx=1 bvy=-1
    end
end

function game:draw()
    -- draw playfield
    _rect(screen.x0,screen.y0,0,0,screen.x1,screen.y1,1,1)

    -- paddle
    _rectfill2(px,py,0,0,pw,ph,1,13)

    -- ball
    _circfill2(bx,by,br,1,8)

    -- bricks
    for b in all(bricks) do
        if b.alive then
            _rectfill2(b.x,b.y,0,0,b.w,b.h,1,6)
        end
    end

    -- win
    local win=true
    for b in all(bricks) do
        if b.alive then win=false end
    end
    if win then
        _print("you win!",(screen.x0+screen.x1)/2-20,(screen.y0+screen.y1)/2,11)
    end
end

return game