# Game-Engine

## Requirements 

- 320x180 (16:9)
- 10x10 sprite
- Letter box integer scaling
- C# and monogame
- Game code in Lua

## Diagram

<pre>
📁 Layers
├── 📁 Application (Lua)
│   ├── 🖥️ Runner
│   ├── 📄 Code 
│   ├── 🎵 Sound Effects
│   ├── 🎼 Songs
│   ├── 🐓 Sprites
│   └── ⛱️ Map
└── 📁 Framework (C#, monogame and NLua)
    ├── 📁 Binding
    │   └── 📄 Lua Functions
    ├── 📁 Data
    │   └── 📄 Images
    ├── 📁 Utils
    │   └── 📄 Screen
    ├── 📁 Game loop
    │   └── 📄 Game loop
    ├── 📁 System
    │   └── 📄 System
    ├── 📁 IO
    │   ├── 📄 Read/Update/Delete/Write file txt
    │   ├── 📄 Read/Update/Delete/Write image png
    │   ├── 📄 Keyboard input
    │   └── 📄 Mouse input
    └── 📁 Graphics
        ├── 📄 Configure palette
        ├── 📄 Draw sprites
        └── 📄 Draw shapes
</pre>

Note: The runner can be used to run your game. But, it can be also used to create tools to create your game assets (sprites, maps, sound effects and songs)

## Functions

### Game loop

- _init()
- _update30()   -> 30fps
- _update60()   -> 60fps
- _draw()

### Input

⌨️ i -> almost any key

- btn(i)    -> Button is pressed
- btnp(i)   -> Button just pressed
- btnr(i)   -> Button just released

🖱️ i -> 0 left 1 right

- click(i)    -> Mouse button is pressed
- clickp(i)   -> Mouse button just pressed
- clickr(i)   -> Mouse button just released
- mouse()     -> Mouse (X,Y) position

### Graphics

- pal(t) -> pass a list of Hex color values to use as a palette. MAX: 32 collors. Default: black color.
- rect(x0, y0, x1, y1, [col])
- rectfill(x0, y0, x1, y1, [col])

### System

- _system(0) -> Frames per second should be locked to either 30 or 60.
