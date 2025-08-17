require("library")
require("component")
local sprite = require("sprite")

objects = {}
function change_state(st)
    state=st
    state:init()
    add(objects,new_body(0,11,10,0,0,0,0,10,10))
end

function _init()
    _texture(10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAAATElEQVRoge3SwQkAIQxFwTRh/60uSFYswC8eZvDgyUeIVXeMqe/RRldW7XikTzayTxJJ3IxUPLJ2sf+z45X/6WAEAAAAAAAAAAB42wdh2QY9AvdvoAAAAABJRU5ErkJggg==")
    change_state(sprite)
end

function _update()
    sprite:update()
end

function _draw()
    sprite:draw()
    _rectfill(0,0,10,180,12)
    foreach(objects, function(o)
        if o.draw ~= nil then
            o:draw()
        end
    end)
end