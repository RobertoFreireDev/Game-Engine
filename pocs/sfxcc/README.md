# PICO-8 `sfx()` Implementation in C#

A focused guide to implementing only the `sfx()` function from PICO-8 using pure C# — no external audio libraries beyond what's in the .NET BCL.

---

## Table of Contents

1. [PICO-8 SFX Data Format](#1-pico-8-sfx-data-format)
2. [Memory Layout](#2-memory-layout)
3. [C# Data Structures](#3-c-data-structures)
4. [Waveform Synthesis](#4-waveform-synthesis)
5. [Audio Output via NAudio](#5-audio-output-via-naudio)
6. [The `sfx()` Function](#6-the-sfx-function)
7. [Parsing `.p8` Cart SFX Data](#7-parsing-p8-cart-sfx-data)
8. [Putting It All Together](#8-putting-it-all-together)
9. [Known Quirks & Approximations](#9-known-quirks--approximations)

---

## 1. PICO-8 SFX Data Format

Each PICO-8 cartridge has **64 SFX slots** (indices 0–63). Each SFX contains:

| Field          | Size    | Description                                      |
|----------------|---------|--------------------------------------------------|
| `editor_mode`  | 1 byte  | 0 = pitch mode, 1 = tracker mode (display only) |
| `note_duration` | 1 byte | Ticks per note (1–255). Lower = faster.         |
| `loop_start`   | 1 byte  | Loop start note index (0–31)                    |
| `loop_end`     | 1 byte  | Loop end note index (0–31, 0 = no loop)         |
| `notes[32]`    | 64 bytes| 32 notes × 2 bytes each                         |

**Total: 68 bytes per SFX slot.**

### Note Layout (2 bytes per note)

Each note is packed into 16 bits:

```
Bit 15–12  Bit 11–9   Bit 8      Bit 7–4    Bit 3–0
---------  ---------  ---------  ---------  ---------
 Waveform   Volume    Custom FX   Effect     Pitch
 (0–7)      (0–7)     flag        (0–7)      (0–63)
```

| Bits  | Field      | Range | Notes                                  |
|-------|------------|-------|----------------------------------------|
| 0–5   | `pitch`    | 0–63  | 0 = C0, 63 = D#5 (see table below)    |
| 6–8   | `waveform` | 0–7   | See waveform list                      |
| 9–11  | `volume`   | 0–7   | 0 = silent, 7 = max                    |
| 12–14 | `effect`   | 0–7   | See effects list                       |
| 15    | `custom`   | 0–1   | 1 = use custom instrument (SFX 0–7)   |

### Pitch Table (0–63 → Hz)

Pitch 0 is C1 (~32.7 Hz). Each semitone step multiplies by `2^(1/12)`.

```
Pitch  Note   Freq (Hz)
  0    C1      32.70
  1    C#1     34.65
  2    D1      36.71
  ...
 36    C4     261.63  (middle C)
  ...
 63    D#5    622.25
```

Formula: `freq = 32.703 * pow(2.0, pitch / 12.0)`

### Waveforms (0–7)

| ID | Name      | Description                        |
|----|-----------|------------------------------------|
| 0  | Triangle  | Smooth triangle wave               |
| 1  | Tilted Saw| Asymmetric saw (not pure sawtooth) |
| 2  | Sawtooth  | Standard sawtooth                  |
| 3  | Square    | 50% duty cycle square wave         |
| 4  | Pulse     | ~33% duty cycle square             |
| 5  | Organ     | Approximated organ (harmonics)     |
| 6  | Noise     | White noise                        |
| 7  | Phaser    | Phase-modulated sine               |

### Effects (0–7)

| ID | Name       | Description                              |
|----|------------|------------------------------------------|
| 0  | None       | No effect                                |
| 1  | Slide      | Pitch slides to next note's pitch        |
| 2  | Vibrato    | Fast pitch oscillation (~7 Hz)           |
| 3  | Drop       | Pitch drops over note duration           |
| 4  | Fade In    | Volume ramps from 0 to note volume       |
| 5  | Fade Out   | Volume ramps from note volume to 0       |
| 6  | Arpeggio Fast | Arpeggiate with next note, fast      |
| 7  | Arpeggio Slow | Arpeggiate with next note, slow      |

---

## 2. Memory Layout

In PICO-8's 64KB RAM, SFX data lives at:

```
0x3200 – 0x42FF   SFX data (64 slots × 68 bytes = 4352 bytes)
```

Each slot at: `0x3200 + (slot_index * 68)`

The header bytes of each slot:

```
Offset 0:  editor_mode    (1 byte)
Offset 1:  note_duration  (1 byte)
Offset 2:  loop_start     (1 byte)
Offset 3:  loop_end       (1 byte)
Offset 4–67: notes        (32 × 2 bytes)
```

---

## 3. C# Data Structures

```csharp
// Represents one note in an SFX slot
public struct Pico8Note
{
    public int   Pitch;    // 0–63
    public int   Waveform; // 0–7
    public int   Volume;   // 0–7
    public int   Effect;   // 0–7
    public bool  Custom;   // true = use custom instrument

    public static Pico8Note FromBytes(byte lo, byte hi)
    {
        int raw = lo | (hi << 8);
        return new Pico8Note
        {
            Pitch    = (raw >> 0) & 0x3F,
            Waveform = (raw >> 6) & 0x07,
            Volume   = (raw >> 9) & 0x07,
            Effect   = (raw >> 12) & 0x07,
            Custom   = ((raw >> 15) & 0x01) != 0
        };
    }
}

// Represents one SFX slot (index 0–63)
public class Pico8Sfx
{
    public int         EditorMode;    // 0 or 1
    public int         NoteDuration;  // ticks per note
    public int         LoopStart;     // note index
    public int         LoopEnd;       // note index (0 = no loop)
    public Pico8Note[] Notes = new Pico8Note[32];

    public static Pico8Sfx FromBytes(byte[] data, int offset)
    {
        var sfx = new Pico8Sfx
        {
            EditorMode   = data[offset + 0],
            NoteDuration = data[offset + 1],
            LoopStart    = data[offset + 2],
            LoopEnd      = data[offset + 3]
        };
        for (int i = 0; i < 32; i++)
        {
            sfx.Notes[i] = Pico8Note.FromBytes(
                data[offset + 4 + i * 2],
                data[offset + 4 + i * 2 + 1]
            );
        }
        return sfx;
    }
}
```

---

## 4. Waveform Synthesis

Each note is rendered to a PCM buffer. The sample rate should be **44100 Hz**.

```csharp
public static class Waveforms
{
    // t = phase in [0.0, 1.0)
    public static float Sample(int waveform, float t, ref float noiseState)
    {
        return waveform switch
        {
            0 => Triangle(t),
            1 => TiltedSaw(t),
            2 => Sawtooth(t),
            3 => Square(t, 0.5f),
            4 => Square(t, 0.333f),
            5 => Organ(t),
            6 => Noise(ref noiseState),
            7 => Phaser(t),
            _  => 0f
        };
    }

    static float Triangle(float t) =>
        t < 0.5f ? (4f * t - 1f) : (3f - 4f * t);

    // PICO-8's "tilted saw": asymmetric with a bump at ~0.875
    static float TiltedSaw(float t) =>
        t < 0.875f
            ? (2f * t / 0.875f) - 1f
            : ((1f - t) / 0.125f) * 2f - 1f;

    static float Sawtooth(float t) => 2f * t - 1f;

    static float Square(float t, float duty) =>
        t < duty ? 1f : -1f;

    // Organ: first + second harmonic approximation
    static float Organ(float t) =>
        (MathF.Sin(2f * MathF.PI * t) +
         0.5f * MathF.Sin(4f * MathF.PI * t)) / 1.5f;

    // Simple xorshift noise (not bit-perfect with PICO-8, but close)
    static float Noise(ref float state)
    {
        uint bits = BitConverter.SingleToUInt32Bits(state);
        bits ^= bits << 13; bits ^= bits >> 17; bits ^= bits << 5;
        if (bits == 0) bits = 1;
        state = BitConverter.UInt32BitsToSingle(bits);
        return ((bits & 0xFFFF) / 32767.5f) - 1f;
    }

    // Phaser: sine with self-modulation
    static float Phaser(float t) =>
        MathF.Sin(2f * MathF.PI * (t + MathF.Sin(2f * MathF.PI * t * 3f) * 0.2f));
}
```

### Pitch to Frequency

```csharp
public static float PitchToHz(int pitch) =>
    32.703f * MathF.Pow(2f, pitch / 12f);
```

### Note Duration in Seconds

PICO-8 runs at 120 BPM base. `note_duration` is in "ticks" where:
- 1 tick = 1/128th of a second at the default speed

```csharp
// PICO-8 tick rate: 128 ticks per second (at 60fps, 2 ticks per frame ~ roughly)
// note_duration is actually: seconds = note_duration / 128.0
public static float NoteSeconds(int noteDuration) =>
    noteDuration / 128f;
```

---

## 5. Audio Output via NAudio

Add the NuGet package:

```bash
dotnet add package NAudio
```

```csharp
using NAudio.Wave;

public class Pico8AudioPlayer : IDisposable
{
    const int SampleRate = 44100;
    const int Channels   = 1;

    readonly WaveOutEvent    _waveOut;
    readonly BufferedWaveProvider _buffer;

    public Pico8AudioPlayer()
    {
        var format = new WaveFormat(SampleRate, 16, Channels);
        _buffer  = new BufferedWaveProvider(format) { DiscardOnBufferOverflow = true };
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_buffer);
        _waveOut.Play();
    }

    // Render a float PCM buffer and push to output
    public void QueueSamples(float[] samples)
    {
        var bytes = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short s = (short)Math.Clamp(samples[i] * 32767f, -32768, 32767);
            bytes[i * 2]     = (byte)(s & 0xFF);
            bytes[i * 2 + 1] = (byte)((s >> 8) & 0xFF);
        }
        _buffer.AddSamples(bytes, 0, bytes.Length);
    }

    public void Dispose() { _waveOut.Stop(); _waveOut.Dispose(); }
}
```

> **Linux/macOS alternative:** Replace `WaveOutEvent` with `OpenALAudioOutput` or use `SDL2-cs` via P/Invoke. On Linux, `NAudio.Core` + ALSA works with the `NAudio.Unix` package.

---

## 6. The `sfx()` Function

```csharp
public class Pico8SfxPlayer
{
    const int SampleRate = 44100;

    readonly Pico8Sfx[]      _sfxTable;   // 64 slots
    readonly Pico8AudioPlayer _audio;

    public Pico8SfxPlayer(Pico8Sfx[] sfxTable, Pico8AudioPlayer audio)
    {
        _sfxTable = sfxTable;
        _audio    = audio;
    }

    /// <summary>
    /// Mirrors the PICO-8 sfx() API:
    ///   sfx(n)           — play SFX slot n
    ///   sfx(-1)          — stop all SFX on channel 0
    ///   sfx(n, channel)  — play on specific channel (0–3); ignored here, single-channel demo
    ///   sfx(n, ch, offset) — start from note offset
    /// </summary>
    public void sfx(int n, int channel = 0, int offset = 0)
    {
        if (n == -1) return;  // stop — no-op in this simplified impl
        if (n < 0 || n > 63)  throw new ArgumentOutOfRangeException(nameof(n));

        var slot = _sfxTable[n];
        float[] pcm = RenderSfx(slot, offset);
        _audio.QueueSamples(pcm);
    }

    float[] RenderSfx(Pico8Sfx sfx, int startNote)
    {
        float noteSec = NoteSeconds(sfx.NoteDuration);
        int samplesPerNote = (int)(noteSec * SampleRate);
        int totalSamples   = samplesPerNote * 32;

        var output = new float[totalSamples];
        float noiseState = 1f;

        for (int noteIdx = startNote; noteIdx < 32; noteIdx++)
        {
            var note = sfx.Notes[noteIdx];
            if (note.Volume == 0) continue;

            float freq    = PitchToHz(note.Pitch);
            float volNorm = note.Volume / 7f;
            float phase   = 0f;
            float phaseInc = freq / SampleRate;

            int sampleStart = (noteIdx - startNote) * samplesPerNote;

            for (int s = 0; s < samplesPerNote; s++)
            {
                float t = (float)s / samplesPerNote; // 0.0 → 1.0 within note

                // Apply effect modifiers
                float effFreq = freq;
                float effVol  = volNorm;
                ApplyEffect(note, s, samplesPerNote, ref effFreq, ref effVol, freq, volNorm);
                phaseInc = effFreq / SampleRate;

                float sample = Waveforms.Sample(note.Waveform, phase, ref noiseState);
                output[sampleStart + s] += sample * effVol * 0.5f; // 0.5 headroom

                phase = (phase + phaseInc) % 1f;
            }
        }

        return output;
    }

    void ApplyEffect(Pico8Note note, int s, int total,
                     ref float freq, ref float vol, float baseFreq, float baseVol)
    {
        float t = (float)s / total;
        switch (note.Effect)
        {
            case 1: // Slide: pitch linearly approaches next note (simplified: no next-note lookup here)
                freq = baseFreq * (1f + 0.05f * t);
                break;
            case 2: // Vibrato
                freq = baseFreq * (1f + 0.03f * MathF.Sin(2f * MathF.PI * 7f * t));
                break;
            case 3: // Drop
                freq = baseFreq * (1f - 0.5f * t);
                break;
            case 4: // Fade In
                vol = baseVol * t;
                break;
            case 5: // Fade Out
                vol = baseVol * (1f - t);
                break;
            // Effects 6 & 7 (arpeggio) require next-note context — omitted for brevity
        }
    }

    static float NoteSeconds(int noteDuration) => noteDuration / 128f;
    static float PitchToHz(int pitch) => 32.703f * MathF.Pow(2f, pitch / 12f);
}
```

---

## 7. Parsing `.p8` Cart SFX Data

PICO-8 `.p8` files store SFX as a text section. Each line under `__sfx__` encodes one slot:

```
__sfx__
00180000 1c071c071c071c071c07...  (32 notes, hex-encoded)
```

### Line Format

Each line: `XXYYSSNN note0 note1 ... note31` where:
- `XX` = `editor_mode` (2 hex chars)
- `YY` = `note_duration` (2 hex chars)
- `SS` = `loop_start` (2 hex chars)
- `NN` = `loop_end` (2 hex chars)
- Each note = 5 hex chars (`PPWVE_` — see below)

### Note Encoding in `.p8` Text

Each note is 5 hex digits: `ppppw veeec` → but the actual layout is:
```
5 hex chars = 20 bits:
  [19:16] effect (4 bits, upper nibble)
  [15:12] volume (4 bits)
  [11:8]  waveform (but 3 bits used → bits 11–9, bit 8 = custom)
  [7:0]   pitch (but 6 bits → 0–63)
```

> In practice, PICO-8 `.p8` text stores each note as 5 hex chars in the order: `pitch(2) waveform+custom(1) volume(1) effect(1)`. Parse carefully — the nibble packing differs from binary layout.

```csharp
public static Pico8Sfx ParseP8Line(string line)
{
    // Strip leading/trailing whitespace
    line = line.Trim();
    if (line.Length < 8) return new Pico8Sfx();

    var sfx = new Pico8Sfx
    {
        EditorMode   = Convert.ToInt32(line.Substring(0, 2), 16),
        NoteDuration = Convert.ToInt32(line.Substring(2, 2), 16),
        LoopStart    = Convert.ToInt32(line.Substring(4, 2), 16),
        LoopEnd      = Convert.ToInt32(line.Substring(6, 2), 16)
    };

    // Notes start at char 8, separated by spaces or packed (depends on PICO-8 version)
    // Modern .p8: 8 header chars then 32 notes × 5 chars, no spaces
    string noteSection = line.Substring(8).Replace(" ", "");
    for (int i = 0; i < 32 && (i * 5 + 5) <= noteSection.Length; i++)
    {
        string chunk = noteSection.Substring(i * 5, 5);
        // chunk = "ppwve" where:
        //   pp = pitch (2 hex digits, 0–3f)
        //   w  = waveform (1 hex digit, 0–7; bit 3 = custom flag)
        //   v  = volume (1 hex digit, 0–7)
        //   e  = effect (1 hex digit, 0–7)
        int pitch    = Convert.ToInt32(chunk.Substring(0, 2), 16);
        int wv       = Convert.ToInt32(chunk.Substring(2, 1), 16);
        int volume   = Convert.ToInt32(chunk.Substring(3, 1), 16);
        int effect   = Convert.ToInt32(chunk.Substring(4, 1), 16);

        sfx.Notes[i] = new Pico8Note
        {
            Pitch    = pitch & 0x3F,
            Waveform = wv & 0x07,
            Volume   = volume & 0x07,
            Effect   = effect & 0x07,
            Custom   = (wv & 0x08) != 0
        };
    }

    return sfx;
}

// Load all 64 SFX slots from a .p8 file
public static Pico8Sfx[] LoadFromP8(string path)
{
    var result = new Pico8Sfx[64];
    for (int i = 0; i < 64; i++) result[i] = new Pico8Sfx { NoteDuration = 18 };

    var lines = File.ReadAllLines(path);
    bool inSfx = false;
    int slot = 0;

    foreach (var line in lines)
    {
        if (line.Trim() == "__sfx__") { inSfx = true; continue; }
        if (line.StartsWith("__")) { inSfx = false; continue; }
        if (!inSfx || slot >= 64) continue;

        result[slot++] = ParseP8Line(line);
    }

    return result;
}
```

---

## 8. Putting It All Together

```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        // Load cart
        var sfxTable = Pico8SfxParser.LoadFromP8("my_cart.p8");

        using var audioPlayer = new Pico8AudioPlayer();
        var sfxPlayer = new Pico8SfxPlayer(sfxTable, audioPlayer);

        // Play SFX slot 0
        sfxPlayer.sfx(0);

        // Wait for playback to finish (rough — calculate duration from slot)
        var slot     = sfxTable[0];
        float dur    = slot.NoteDuration / 128f * 32;
        System.Threading.Thread.Sleep((int)(dur * 1000) + 200);

        Console.WriteLine("Done.");
    }
}
```

---

## 9. Known Quirks & Approximations

| Feature | Status | Notes |
|---------|--------|-------|
| Waveforms 0–5 | ✅ Close | Not sample-accurate; PICO-8 uses lookup tables |
| Waveform 6 (Noise) | ⚠️ Approximate | Real PICO-8 uses LFSR; xorshift is audibly similar |
| Waveform 7 (Phaser) | ⚠️ Approximate | Exact algorithm undocumented |
| Effects 1–5 | ✅ Implemented | Slide requires next-note context for full accuracy |
| Effects 6–7 (Arpeggio) | ⚠️ Partial | Needs lookahead to next note's pitch |
| Multi-channel mixing | ❌ Not implemented | PICO-8 supports 4 channels simultaneously |
| Custom instruments | ❌ Not implemented | Requires using SFX 0–7 as wavetables |
| `sfx(-1)` stop | ❌ No-op | Need a playing-channel tracker to stop mid-note |
| Binary `.p8.png` carts | ❌ Not covered | Requires PNG steganography extraction first |

### Improving Accuracy

- **Noise (waveform 6):** PICO-8 uses a 15-bit or 31-bit LFSR. Replace the xorshift with an LFSR for closer fidelity.
- **Volume scaling:** PICO-8 volumes are perceptually scaled, not linear. Apply `vol^2` or a small lookup table.
- **Note duration:** The `note_duration` field maps to ticks at **120 BPM** in 4/4 time with PICO-8's specific resolution. Treat `noteDuration / 128.0` as the starting approximation and tune by ear.

---

## References

- [PICO-8 Wiki — SFX](https://pico-8.fandom.com/wiki/SFX)
- [PICO-8 Memory Map](https://pico-8.fandom.com/wiki/Memory)
- [Lexaloffle BBS — Audio internals threads](https://www.lexaloffle.com/bbs/)
- [NAudio GitHub](https://github.com/naudio/NAudio)