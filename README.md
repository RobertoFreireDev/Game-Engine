# Black-Box

This engine starts as a blank canvas, giving Lua full control over display and logic

Developers can write Lua scripts to build custom tools, editors, or complete games

## Requirements 

- 320x180 (16:9)
- Letter box integer scaling
- Game code in Lua

# Functions

## Draw

- void pal(string palette)

```lua
pal("#000000,#ffffff,#ffffb0,#7e70ca,#a8734a,#e9b287,#772d26,#b66862,#85d4dc,#c5ffff,#a85fb4,#e99df5,#559e4a,#92df87,#42348b,#bdcc71")
```

- void rect(int x, int y, int width, int height, int color = 0)

```lua
rect(0, 0, 320, 180, 3)  
```

- void rectfill(int x, int y, int width, int height, int color = 0)
- void print(string text, int x, int y, int color = 0, bool wraptext = false, int wrapLimit = 0)
- void spr(int i, int x, int y, int w = 1, int h = 1, bool flipX = false, bool flipY = false)

## IOFile

- bool iohasfile(string fileName)
- string ioread(string fileName)
- void iocreate(string fileName, string content)
- void ioupdate(string fileName, string content)
- void iodelete(string fileName)

## System

- int sysfps()

## Init

- void inittitle()

```lua
inittitle("MY GAME")
```

- void initbckgdclr(int colorIndex)
- void initfps30() 
- void initfps60()
- void inittexture(string spriteBase64, int tileWidth, int tileHeight)

```lua
inittexture("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",32,32)
```

## Mouse

- {x,y} mousepos()
- bool mouseclick(int i)
- bool mouseclickp(int i)
- bool mouseclickr(int i)
- bool mousescroll(int i)
- void mousecursor(int i)

## Keyboard

- bool btn(int keyNumber)
- bool btnp(int keyNumber)
- bool btnr(int keyNumber)

```lua
btnr(72) -- button h
```

### key Number

| Key | Number | Description |
|-----|--------|-------------|
| `None` | `0` | Reserved. |
| `Back` | `8` | BACKSPACE key. |
| `Tab` | `9` | TAB key. |
| `Enter` | `13` | ENTER key. |
| `CapsLock` | `20` | CAPS LOCK key. |
| `Escape` | `27` | ESC key. |
| `Space` | `32` | SPACEBAR key. |
| `PageUp` | `33` | PAGE UP key. |
| `PageDown` | `34` | PAGE DOWN key. |
| `End` | `35` | END key. |
| `Home` | `36` | HOME key. |
| `Left` | `37` | LEFT ARROW key. |
| `Up` | `38` | UP ARROW key. |
| `Right` | `39` | RIGHT ARROW key. |
| `Down` | `40` | DOWN ARROW key. |
| `Select` | `41` | SELECT key. |
| `Print` | `42` | PRINT key. |
| `Execute` | `43` | EXECUTE key. |
| `PrintScreen` | `44` | PRINT SCREEN key. |
| `Insert` | `45` | INS key. |
| `Delete` | `46` | DEL key. |
| `Help` | `47` | HELP key. |
| `D0` | `48` | Used for miscellaneous characters; it can vary by keyboard. |
| `D1` | `49` | Used for miscellaneous characters; it can vary by keyboard. |
| `D2` | `50` | Used for miscellaneous characters; it can vary by keyboard. |
| `D3` | `51` | Used for miscellaneous characters; it can vary by keyboard. |
| `D4` | `52` | Used for miscellaneous characters; it can vary by keyboard. |
| `D5` | `53` | Used for miscellaneous characters; it can vary by keyboard. |
| `D6` | `54` | Used for miscellaneous characters; it can vary by keyboard. |
| `D7` | `55` | Used for miscellaneous characters; it can vary by keyboard. |
| `D8` | `56` | Used for miscellaneous characters; it can vary by keyboard. |
| `D9` | `57` | Used for miscellaneous characters; it can vary by keyboard. |
| `A` | `65` | A key. |
| `B` | `66` | B key. |
| `C` | `67` | C key. |
| `D` | `68` | D key. |
| `E` | `69` | E key. |
| `F` | `70` | F key. |
| `G` | `71` | G key. |
| `H` | `72` | H key. |
| `I` | `73` | I key. |
| `J` | `74` | J key. |
| `K` | `75` | K key. |
| `L` | `76` | L key. |
| `M` | `77` | M key. |
| `N` | `78` | N key. |
| `O` | `79` | O key. |
| `P` | `80` | P key. |
| `Q` | `81` | Q key. |
| `R` | `82` | R key. |
| `S` | `83` | S key. |
| `T` | `84` | T key. |
| `U` | `85` | U key. |
| `V` | `86` | V key. |
| `W` | `87` | W key. |
| `X` | `88` | X key. |
| `Y` | `89` | Y key. |
| `Z` | `90` | Z key. |
| `LeftWindows` | `91` | Left Windows key. |
| `RightWindows` | `92` | Right Windows key. |
| `Apps` | `93` | Applications key. |
| `Sleep` | `95` | Computer Sleep key. |
| `NumPad0` | `96` | Numeric keypad 0 key. |
| `NumPad1` | `97` | Numeric keypad 1 key. |
| `NumPad2` | `98` | Numeric keypad 2 key. |
| `NumPad3` | `99` | Numeric keypad 3 key. |
| `NumPad4` | `100` | Numeric keypad 4 key. |
| `NumPad5` | `101` | Numeric keypad 5 key. |
| `NumPad6` | `102` | Numeric keypad 6 key. |
| `NumPad7` | `103` | Numeric keypad 7 key. |
| `NumPad8` | `104` | Numeric keypad 8 key. |
| `NumPad9` | `105` | Numeric keypad 9 key. |
| `Multiply` | `106` | Multiply key. |
| `Add` | `107` | Add key. |
| `Separator` | `108` | Separator key. |
| `Subtract` | `109` | Subtract key. |
| `Decimal` | `110` | Decimal key. |
| `Divide` | `111` | Divide key. |
| `F1` | `112` | F1 key. |
| `F2` | `113` | F2 key. |
| `F3` | `114` | F3 key. |
| `F4` | `115` | F4 key. |
| `F5` | `116` | F5 key. |
| `F6` | `117` | F6 key. |
| `F7` | `118` | F7 key. |
| `F8` | `119` | F8 key. |
| `F9` | `120` | F9 key. |
| `F10` | `121` | F10 key. |
| `F11` | `122` | F11 key. |
| `F12` | `123` | F12 key. |
| `F13` | `124` | F13 key. |
| `F14` | `125` | F14 key. |
| `F15` | `126` | F15 key. |
| `F16` | `127` | F16 key. |
| `F17` | `128` | F17 key. |
| `F18` | `129` | F18 key. |
| `F19` | `130` | F19 key. |
| `F20` | `131` | F20 key. |
| `F21` | `132` | F21 key. |
| `F22` | `133` | F22 key. |
| `F23` | `134` | F23 key. |
| `F24` | `135` | F24 key. |
| `NumLock` | `144` | NUM LOCK key. |
| `Scroll` | `145` | SCROLL LOCK key. |
| `LeftShift` | `160` | Left SHIFT key. |
| `RightShift` | `161` | Right SHIFT key. |
| `LeftControl` | `162` | Left CONTROL key. |
| `RightControl` | `163` | Right CONTROL key. |
| `LeftAlt` | `164` | Left ALT key. |
| `RightAlt` | `165` | Right ALT key. |
| `BrowserBack` | `166` | Browser Back key. |
| `BrowserForward` | `167` | Browser Forward key. |
| `BrowserRefresh` | `168` | Browser Refresh key. |
| `BrowserStop` | `169` | Browser Stop key. |
| `BrowserSearch` | `170` | Browser Search key. |
| `BrowserFavorites` | `171` | Browser Favorites key. |
| `BrowserHome` | `172` | Browser Start and Home key. |
| `VolumeMute` | `173` | Volume Mute key. |
| `VolumeDown` | `174` | Volume Down key. |
| `VolumeUp` | `175` | Volume Up key. |
| `MediaNextTrack` | `176` | Next Track key. |
| `MediaPreviousTrack` | `177` | Previous Track key. |
| `MediaStop` | `178` | Stop Media key. |
| `MediaPlayPause` | `179` | Play/Pause Media key. |
| `LaunchMail` | `180` | Start Mail key. |
| `SelectMedia` | `181` | Select Media key. |
| `LaunchApplication1` | `182` | Start Application 1 key. |
| `LaunchApplication2` | `183` | Start Application 2 key. |
| `OemSemicolon` | `186` | The OEM Semicolon key on a US standard keyboard. |
| `OemPlus` | `187` | For any country/region, the '+' key. |
| `OemComma` | `188` | For any country/region, the ',' key. |
| `OemMinus` | `189` | For any country/region, the '-' key. |
| `OemPeriod` | `190` | For any country/region, the '.' key. |
| `OemQuestion` | `191` | The OEM question mark key on a US standard keyboard. |
| `OemTilde` | `192` | The OEM tilde key on a US standard keyboard. |
| `OemOpenBrackets` | `219` | The OEM open bracket key on a US standard keyboard. |
| `OemPipe` | `220` | The OEM pipe key on a US standard keyboard. |
| `OemCloseBrackets` | `221` | The OEM close bracket key on a US standard keyboard. |
| `OemQuotes` | `222` | The OEM singled/double quote key on a US standard keyboard. |
| `Oem8` | `223` | Used for miscellaneous characters; it can vary by keyboard. |
| `OemBackslash` | `226` | The OEM angle bracket or backslash key on the RT 102 key keyboard. |
| `ProcessKey` | `229` | IME PROCESS key. |
| `Attn` | `246` | Attn key. |
| `Crsel` | `247` | CrSel key. |
| `Exsel` | `248` | ExSel key. |
| `EraseEof` | `249` | Erase EOF key. |
| `Play` | `250` | Play key. |
| `Zoom` | `251` | Zoom key. |
| `Pa1` | `253` | PA1 key. |
| `OemClear` | `254` | CLEAR key. |
| `ChatPadGreen` | `202` | Green ChatPad key. |
| `ChatPadOrange` | `203` | Orange ChatPad key. |
| `Pause` | `19` | PAUSE key. |
| `ImeConvert` | `28` | IME Convert key. |
| `ImeNoConvert` | `29` | IME NoConvert key. |
| `Kana` | `21` | Kana key on Japanese keyboards. |
| `Kanji` | `25` | Kanji key on Japanese keyboards. |
| `OemAuto` | `243` | OEM Auto key. |
| `OemCopy` | `242` | OEM Copy key. |
| `OemEnlW` | `244` | OEM Enlarge Window key. |
