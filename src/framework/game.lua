--[[
    Use this to test each and every function
    Filter and validate each and every parameter of lua functions
    Use this category to show case all game engine features and capabilities
    Test every thing!. Example: Pause game, restart game, initial variables, edge cases for every function
]]

-- screen
screen={sx=80,sy=10,x0=0,y0=0,w=174,h=160,x1=254,y1=170}

-- paddle
pw=24
ph=3
px=(screen.sx+screen.x1)/2-pw/2
py=screen.y1-20

-- ball
bx=(screen.sx+screen.x1)/2
by=(screen.sy+screen.y1)/2
br=3
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
            local bxpos=screen.sx+8+i*bw
            local bypos=screen.sy+8+j*bh
            add(bricks,{x=bxpos,y=bypos,w=bw-2,h=bh-2,alive=true})
        end
    end
end

function game:update()   
    if _btn(_keys.A) then px=px-2 end
    if _btn(_keys.D) then px=px+2 end
    px=mid(screen.sx+1,px,screen.x1-pw - 1)

    -- move ball
    bx=bx+bvx
    by=by+bvy

    -- wall bounce
    if bx<br+screen.sx then bvx=abs(bvx)
    elseif bx>screen.x1-br-1 then bvx=-abs(bvx) end
    if by<br+screen.sy then bvy=abs(bvy) end

    -- paddle bounce
    if by+br>=py and by+br <= py + 2 and bx>px and bx<px+pw then
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
        bx=(screen.sx+screen.x1)/2
        by=(screen.sy+screen.y1)/2
        bvx=1 bvy=-1
    end
end

function game:draw()
    -- draw playfield
    _rect(screen.sx + screen.x0,screen.sy + screen.y0,screen.w,screen.h,1)

    -- paddle
    _rectfill(px,py,pw,ph,13)

    -- ball
    _circ(bx,by,br,8)

    -- bricks
    for b in all(bricks) do
        if b.alive then
            _rectfill(b.x,b.y,b.w,b.h,6)
        end
    end

    -- win
    local win=true
    for b in all(bricks) do
        if b.alive then win=false end
    end
    if win then
        _print("you win!",(screen.sx+screen.x1)/2-20,(screen.sy+screen.y1)/2,11)
    end
end

return game