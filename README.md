# Game-Engine

## Requirements 

- 256x144 (16:9)
- 8x8 sprite
- Letter box integer scaling
- C# and monogame
- Game code in Lua

## Palette

![palette](imgs/lospec500-8x.png)

## Controller

- ⬆️(0), ➡️(1), ⬇️(2), ⬅️(3), 🟢(4), 🔵(5), 🔴(6), and Start(7)

![controller](imgs/controller.png)

## Files

<pre>
📁 Content
├── 🖥️ Runner
├── 📄 Code 
├── 🎵 Sound Effects
├── 🎼 Songs
├── 🐓 Sprites
└── ⛱️ Map
</pre>

Note: The runner can be used to run your game. But, it can be also used to create tools to create your game assets (sprites, maps, sound effects and songs)

## Functions

### Game loop

- _init()
- _update30()   -> 30fps
- _update60()     -> 60fps
- _draw()

### Input

- btn([i])    -> Button is pressed
- btnp([i])   -> Button just pressed
- btnr([i])   -> Button just released

### Draw

- rect(x0, y0, x1, y1, [col])
- rectfill(x0, y0, x1, y1, [col])

### System

- _system(0) -> Frames per second should be locked to either 30 or 60.