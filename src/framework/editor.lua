require("library")
require("component")
local spriteeditor = require("spriteeditor")
local mapeditor = require("mapeditor")

buttonSelected = nil
buttons = {}
state = {}

function change_state(st)
    state=st
    state:init()
    _fps30()
end

function _init()
    buttons = {}
    _texture(0,10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAAl0lEQVRoge3V0Q6AIAiFYV+C93/VNiUEvStwq/3funBecBJZtXaGdLouzdAUS0sP0ac2xJ+kJOJkSCsPsbvwc5aecpcuDPmV2aTQLYmz966XYnMlfsDm9lw+ThE7Qc8Qt+1C3n3dxluKlQrVkkK05eMQW7GskHiQpVZmiPV/LZV18f5nsr1u3ggDAAAAAAAAAAAAAIDvuQBw8w7vir6ICAAAAABJRU5ErkJggg==")
    _texture(1,10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAAUUlEQVRoge3RMREAAAjEMFDw/t2iAo4hQ+cOqaR7uzqZ7C+SmwkTJkyYMGHChAkTJkyYMGHChAkTJkyYMGHChAkTJkyYMGHChAkTJkyYMHlpMh2+OJAFn+v0AAAAAElFTkSuQmCC")
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
end

function _draw()
    state:draw()
    _rectfill(0,0,10,180,12)
    foreach(buttons, function(o)
        o.b:draw()
    end)
end