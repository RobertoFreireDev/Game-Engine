require("library")
require("component")
require("helperfunctions")
local spriteeditor = require("spriteeditor")
local mapeditor = require("mapeditor")

buttonSelected = nil
buttons = {}
state = {}

local spriteFileName = "spritedata"

function change_state(st)
    state=st
    state:init()
    _fps30()
end

function _init()
    _sgrid(_ioread(spriteFileName))
    buttons = {}
    _texture(0,10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAAl0lEQVRoge3V0Q6AIAiFYV+C93/VNiUEvStwq/3funBecBJZtXaGdLouzdAUS0sP0ac2xJ+kJOJkSCsPsbvwc5aecpcuDPmV2aTQLYmz966XYnMlfsDm9lw+ThE7Qc8Qt+1C3n3dxluKlQrVkkK05eMQW7GskHiQpVZmiPV/LZV18f5nsr1u3ggDAAAAAAAAAAAAAIDvuQBw8w7vir6ICAAAAABJRU5ErkJggg==")
    _texture(1,10,10,"iVBORw0KGgoAAAANSUhEUgAAAUAAAAC0CAMAAADSOgUjAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAABAUlEQVR4nO3SMQ0AMAzAsJIof6oD0SPHLBPIkdnhYvIC/uZAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlwKMHvBM7COSTH1YAAAAASUVORK5CYII=")
    --_pal("#272223,#f2d3ac,#e7a76c,#c28462,#905b54,#513a3d,#7a6977,#878c87,#b5c69a,#606b31,#b19e3f,#f8c65c,#d58b39,#996336,#6a422c,#b55b39")
    spritebutton = new_button(0,0,11,10,0,0,0,0,10,10)
    spritebutton.clicked = function(o) change_state(spriteeditor) buttonSelected = o end
    mapbutton = new_button(0,1,11,10,0,10,0,0,10,10)
    mapbutton.clicked = function(o) change_state(mapeditor) buttonSelected = o end
    add(buttons,spritebutton)
    add(buttons,mapbutton)
    buttonSelected = spritebutton
    change_state(spriteeditor)
end

function _update()
    state:update()
    foreach(buttons, function(o)
        o:update()
        o.b.c = buttonSelected == o and 1 or 11
    end)

    if _btnp(_keys.Escape) then
        _reboot()
    end

    if _btnp(_keys.Q) then
        _iocreateorupdate(spriteFileName,_ggrid())
    end
end

function _draw()
    state:draw()
    _rectfill(0,0,10,180,12)
    foreach(buttons, function(o)
        o.b:draw()
    end)
end