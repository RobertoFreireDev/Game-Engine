# Black-Box

## Requirements 

- 320x180 (16:9)
- Letter box integer scaling
- C# and monogame
- Game code in Lua

## Diagram

<pre>
📁 Framework (C#, monogame and NLua)
├── 📁 Application (Lua)
│   ├── 📄 Code
│   ├── 📄 Metadata (Program title, fps, ...)
│   ├── 🎵 Sound Effects
│   ├── 🎼 Songs
│   ├── 🐓 Sprites
│   └── ⛱️ Map
├── 📁 Binding
│   └── 📄 Lua Functions
├── 📁 Data
│   └── 📄 Images
├── 📁 Utils
│   └── 📄 Screen
├── 📁 System
│   └── 📄 System
├── 📁 IO
│   ├── 📄 Read/Update/Delete/Write file txt
│   ├── 📄 Read/Update/Delete/Write image png
│   ├── 📄 Keyboard input
│   └── 📄 Mouse input
└── 📁 Graphics
    ├── 📄 Font
    ├── 📄 Configure palette
    ├── 📄 Draw sprites
    └── 📄 Draw shapes
</pre>

Note: The runner can be used to run your game. But, it can be also used to create tools to create your game assets (sprites, maps, sound effects and songs)

## Functions

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