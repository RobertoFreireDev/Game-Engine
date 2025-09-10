# Black-Box

This engine starts as a blank canvas, giving Lua control over display and logic

Developers can write Lua scripts to build custom tools, editors, or games.

![blackbox](src/assets/blackbox.png)

## Legal

- How to mention Nlua and Monogame?
- Make sure there is no copyright issue with Name and/or Product/Game Engine.
- What else?

**Functions:**

Create Area functions and use it instead of new_body lua function

- void Delete(index)
- bool Collided(x,y,w=1,h=1)
- bool Hasvalue(index) return bool
- bool Update(index,x,y,w,h)
- int Add (x, y,w,h, usenextnullobject false)
- void Clearlist()
- int Count(includingnull = false)

Validate first if it is possible to Store lua object/table reference in c# and then in Lua use that reference value to manipulate as a object

- - Stack
- Queue
- Tree

**Tutorial:**

- implement to dos written in tutorialeditor.lua

**Game:**

- implement to dos written in game.lua

**SFX:**

- music([n,] [fade_len,] [channel_mask])

**MAP:**

- Add flag functions