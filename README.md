# Black-Box

Note: The runner can be used to run your game. But, it can be also used to create tools to create your game assets (sprites, maps, sound effects and songs)

# Functions

## Draw

- void pal(p)                 -> Set palette

```lua
pal("#000000,#ffffff,#ffffb0,#7e70ca,#a8734a,#e9b287,#772d26,#b66862,#85d4dc,#c5ffff,#a85fb4,#e99df5,#559e4a,#92df87,#42348b,#bdcc71")
```

- void rect(x,y,w,h,[c])      -> Draw rectangle border

```lua
rect(0, 0, 320, 180, 3)  
```

- void rectfill(x,y,w,h,[c])  -> Draw rectangle
- void print(t,x,y,[c],[w],[l])

### Parameters

- string p -> palette list in hex color
- int c    -> 0-15 color index
- bool w   -> wraptext
- int l    -> wrap limit in x

## System

- int sysfps()        -> get FPS

## Configuration

- void inittitle()    -> Update title

```lua
inittitle("MY GAME")
```

- void initbckgdclr(c) -> Update background color
- void initfps30()    -> Set 30 Fps 
- void initfps60()    -> Set 60 Fps

### Parameters

- int c    -> 0-15 color index

## Mouse

- {x,y} mousepos()      -> Get mouse position
- bool mouseclick(s)   -> Mouse button pressed
- bool mouseclickp(s)  -> Mouse button just pressed
- bool mouseclickr(s)  -> Mouse button just released
- bool mousescroll(d)  -> scroll
- void mousecursor(c)  -> update cursor

### Parameters

- int s: 1 Right, others Left
- int d: 1 Up, others Down
- int c: 1 Pointer, others Context menu

## Keyboard

- bool btn(k)    -> keyboard pressed
- bool btnp(k)   -> keyboard just pressed
- bool btnr(k)   -> keyboard released

```lua
btnr(72) -- button h
```

### Keys int k

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
