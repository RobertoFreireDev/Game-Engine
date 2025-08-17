require("library")
require("component")
local spriteeditor = require("spriteeditor")
local mapeditor = require("mapeditor")

objects = {}
state = {}
function change_state(st)
    state=st
    state:init()
end

function _init()
    _texture(10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAATElEQVRoge3SwQkAIQxFwTRh/60uSFYswC8eZvDgyUeIVXeMqe/RRldW7XikTzayTxJJ3IxUPLJ2sf+z45X/6WAEAAAAAAAAAAB42wdh2QY9AvdvoAAAAABJRU5ErkJggg==")
    local spritebutton = new_button(0,11,10,0,0,0,0,10,10)
    spritebutton.clicked = function() change_state(spriteeditor) end
    local mapbutton = new_button(1,11,10,0,10,0,0,10,10)
    mapbutton.clicked = function() change_state(mapeditor) end
    add(objects,spritebutton)
    add(objects,mapbutton)
    change_state(spriteeditor)
end

function _update()
    state:update()
    if _mouseclick(0) then
        foreach(objects, function(o)
            o:update()
        end)
    end
end

function _draw()
    state:draw()
    _rectfill(0,0,10,180,12)
    foreach(objects, function(o)
        o.b:draw()
    end)
end