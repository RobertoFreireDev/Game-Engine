# Game-Engine

## Requirements 

- 256x144 (16:9)
- 8x8 sprite
- Letter box integer scaling
- Code in Lua
- Use SDL2 and C++

## Palette

![palette](imgs/lospec500-8x.png)

## Controller

- ⬆️,➡️,⬇️,⬅️,1,2,3,4 and Start

![controller](imgs/controller.png)

## Files

<pre>
📁 Content
├── 🛠️ Game Engine
├── 🕹️ Game Player
├── 📄 Code 
├── 📄 Sound Effects
├── 📄 Songs
├── 📄 Map
└── 📄 Sprites
</pre>

## Functions

### Game loop

- _init()
- _update()
- _draw()

### Input

- btn([i])    -> Button is pressed
- btnp([i])   -> Button just pressed
- btnr([i])   -> Button just released

### Draw

- rect(x0, y0, x1, y1, [col])
- rectfill(x0, y0, x1, y1, [col])