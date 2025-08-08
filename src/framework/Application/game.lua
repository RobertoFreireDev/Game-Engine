function _init()
    pal(""#000000,#ffffff,#ffffb0,#7e70ca,#a8734a,#e9b287,#772d26,#b66862,#85d4dc,#c5ffff,#a85fb4,#e99df5,#559e4a,#92df87,#42348b,#bdcc71"")
end

function _update()
end

function _draw()
    rectfill(0, 0, 320, 180, 2)
    rect(0, 0, 320, 180, 1)
    print(""HELLO WORLD"", 0, 0)
    print(""HELLO WORLD"", 0, 7)
end