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
end

function _init()
    buttons = {}
    _texture(10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAATElEQVRoge3SwQkAIQxFwTRh/60uSFYswC8eZvDgyUeIVXeMqe/RRldW7XikTzayTxJJ3IxUPLJ2sf+z45X/6WAEAAAAAAAAAAB42wdh2QY9AvdvoAAAAABJRU5ErkJggg==")
    spritebutton = new_button(0,11,10,0,0,0,0,10,10)
    spritebutton.clicked = function(o) change_state(spriteeditor) buttonSelected = o end
    mapbutton = new_button(1,11,10,0,10,0,0,10,10)
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