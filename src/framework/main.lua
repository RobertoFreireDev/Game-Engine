require("library")
require("constants")
require("component")
require("helperfunctions")
local spriteeditor = require("spriteeditor")
local mapeditor = require("mapeditor")
local sfxeditor = require("sfxeditor")
local musiceditor = require("musiceditor")
local tutorialeditor = require("tutorialeditor")
local game = require("game")

buttonSelected = nil
buttons = {}
state = {}
textureloaded = false
texture0loaded = false
texture1loaded = false

local spriteFileName, mapFileName = "spritedata", "mapData"

function change_state(st)
    state=st
    state:init()
    _title("Black Box Editor")
    _fps30()
end

function _init()   
    _ngrid(0,30,(const.maxPage+1)*4,10,true)
    _cmap(0,320,180,10)
    if _iohasfile(spriteFileName) then
        _sgrid(0,_ioread(spriteFileName))
    end
    if _iohasfile(mapFileName) then
        _smap(0,_ioread(mapFileName))
    end    
    
    buttons = {}
    _loadtexture(0,10,10,"iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAABQElEQVRoge2WWQ7DIAxEucTc/6qVmoDHS0JC1495lQoi2GMb07S174DxNYapCYBgi/3DM9ZAG4vdeJtXHp3r7UkQyRk8B6BlEdAIZwRbnGeCoOXLVYrA4jkUqXLx5ZqIjFDpQa9ZzgQblzLZawgvNM8EDVWH0CKNezB0XNF/nQnIzdY+OBbxxfWPzzO5fCd+Awa37caMl81j954aYHQF9Witwd2VLp6bwovUGVFjemfmwt1V+llIhqA8/UWwvf62WfNXF2cq4jPhxu0r6N5TTMsisA1FIt75arlSJnwiUWPx4Isz4cIFjbUWrrrrJquX8beurVxBpGqouxHY2+ZIJLx50mGe+74gwkHjqkjdaCci/FfmkyJHZX1fuYr4FpgevO1kg0XJqUjc9xITESGEEEIIIYQQQgghhBBCCCHEX/AA1p48yoKq63kAAAAASUVORK5CYII=")
    _loadtexture(1,10,10,"iVBORw0KGgoAAAANSUhEUgAAAUAAAAC0CAMAAADSOgUjAAAAAXNSR0IArs4c6QAAAGBQTFRFAAAAIiA0RSg8Zjkxj1Y733Em2aBm7sOa+/I2meVQar4wN5RuS2kvUkskMjw5Pz90MGCCW27hY5v/X83ky9v8////m623hH6HaWpqWVZSdkKKrDIy2Vdj13u6j5dKim8w+2O8zwAAACB0Uk5TAP////////////////////////////////////////+Smq12AAABAUlEQVR4nO3SMQ0AMAzAsJIof6oD0SPHLBPIkdnhYvIC/uZAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlQFoOpOVAWg6k5UBaDqTlwKMHvBM7COSTH1YAAAAASUVORK5CYII=")
    spritebutton = new_button(0,0,11,10,0,0,0,0,10,10)
    spritebutton.clicked = function(o) change_state(spriteeditor) buttonSelected = o end
    mapbutton = new_button(0,1,11,10,0,10,0,0,10,10)
    mapbutton.clicked = function(o) change_state(mapeditor) buttonSelected = o end
    sfxbutton = new_button(0,2,11,10,0,20,0,0,10,10)
    sfxbutton.clicked = function(o) change_state(sfxeditor) buttonSelected = o end
    musicbutton = new_button(0,3,11,10,0,30,0,0,10,10)
    musicbutton.clicked = function(o) change_state(musiceditor) buttonSelected = o end
    tutorialbutton = new_button(0,27,11,10,0,40,0,0,10,10)
    tutorialbutton.clicked = function(o) change_state(tutorialeditor) buttonSelected = o end
    gamebutton = new_button(0,18,11,10,0,50,0,0,10,10)
    gamebutton.clicked = function(o) change_state(game) buttonSelected = o end

    add(buttons,spritebutton)
    add(buttons,mapbutton)
    add(buttons,sfxbutton)
    add(buttons,musicbutton)
    add(buttons,tutorialbutton)
    add(buttons,gamebutton)

    buttonSelected = spritebutton
    change_state(spriteeditor)
end

function loadsplitedtextures()
   if not texture0loaded and not _loadnextsplitedtexture(0) then
       texture0loaded = true
   end
   if not texture1loaded and not _loadnextsplitedtexture(1) then
       texture1loaded = true
   end

   textureloaded = texture0loaded and texture1loaded
end

function _update()
    if not _isfocused() then
        return
    end

    if not textureloaded then
        loadsplitedtextures()
    end

    state:update()
    foreach(buttons, function(o)
        o:update()
        o.b.c = buttonSelected == o and 1 or 11
    end)

    if _btnp(_keys.Escape) then
        _reboot()
    end

    if (_btn(_keys.LeftControl) or _btn(_keys.RightControl)) and _btnp(_keys.R) then
        _iocreateorupdate(spriteFileName,_ggrid(0))
        _iocreateorupdate(mapFileName,_gmap(0))
    end
end

function _draw()
    state:draw()
    _rectfill(0,0,10,180,12)
    foreach(buttons, function(o)
        o.b:draw()
    end)
end