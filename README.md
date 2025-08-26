# Black-Box

This engine starts as a blank canvas, giving Lua control over display and logic

Developers can write Lua scripts to build custom tools, editors, or games.

## Legal

- How to mention Nlua and Monogame?
- Make sure there is no copyright issue with Name and/or Product/Game Engine.
- What else?

**Tutorial:**

- Create a tutorial screen with pages
- IsAltF4Pressed -> Exit
- IsF2Released -> ToggleFullScreen
- Explain all functions

**SFX:**

- music([n,] [fade_len,] [channel_mask])

**MAP:**

- Add flag functions

**Shader:**

add if else logic to frac()/loop image or not 

-  float2 distortedUV = frac(uv + float2(waveX, waveY));

add new shader:
after horizontal or vertical line, it draws pixel adding color and transparency including the possibility of 100%. basically it changes the color or doesn't draw image after x or y line.

work on bloom effect.
right now it draws again the image with offset in x and y. add more offsets from 1 until to offset number

# Validations

- Validate every single function/logic to avoid errors