local p={}
p.x=0
p.y=0
txt = ""

function _init()
    inittitle("MY GAME")
    initfps60()
    initbckgdclr(0)
    pal("#000000,#ffffff,#ffffb0,#7e70ca,#a8734a,#e9b287,#772d26,#b66862,#85d4dc,#c5ffff,#a85fb4,#e99df5,#559e4a,#92df87,#42348b,#bdcc71")
    txt = ioread("test");
end

function _update()
    local h = 72
    if btnr(h) then
        iodelete("test");
    end
end

function _draw()
    rect(0, 0, 320, 180, 3)                  
    print(txt,8,8,2)
end