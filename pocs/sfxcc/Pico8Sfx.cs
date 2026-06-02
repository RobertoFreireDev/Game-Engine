using System.Globalization;
using NAudio.Wave;

namespace sfxcc;

public struct Pico8Note
{
    public int Pitch;
    public int Waveform;
    public int Volume;
    public int Effect;
    public bool Custom;

    public static Pico8Note FromBytes(byte lo, byte hi)
    {
        int raw = lo | (hi << 8);
        return new Pico8Note
        {
            Pitch = (raw >> 0) & 0x3F,
            Waveform = (raw >> 6) & 0x07,
            Volume = (raw >> 9) & 0x07,
            Effect = (raw >> 12) & 0x07,
            Custom = ((raw >> 15) & 0x01) != 0,
        };
    }
}

public sealed class Pico8Sfx
{
    public int EditorMode { get; set; }
    public int NoteDuration { get; set; }
    public int LoopStart { get; set; }
    public int LoopEnd { get; set; }
    public Pico8Note[] Notes { get; } = new Pico8Note[32];

    public static Pico8Sfx FromBytes(byte[] data, int offset)
    {
        var sfx = new Pico8Sfx
        {
            EditorMode = data[offset + 0],
            NoteDuration = data[offset + 1],
            LoopStart = data[offset + 2],
            LoopEnd = data[offset + 3],
        };

        for (int i = 0; i < 32; i++)
        {
            sfx.Notes[i] = Pico8Note.FromBytes(data[offset + 4 + (i * 2)], data[offset + 5 + (i * 2)]);
        }

        return sfx;
    }
}

public static class Pico8SfxParser
{
    public static Pico8Sfx[] LoadFromP8(string path)
    {
        var result = new Pico8Sfx[64];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Pico8Sfx { NoteDuration = 18 };
        }

        string[] lines = File.ReadAllLines(path);
        bool inSfx = false;
        int slot = 0;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line == "__sfx__")
            {
                inSfx = true;
                continue;
            }

            if (line.StartsWith("__", StringComparison.Ordinal) && line != "__sfx__")
            {
                inSfx = false;
                continue;
            }

            if (!inSfx || slot >= 64 || string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            result[slot++] = ParseP8Line(line);
        }

        return result;
    }

    public static Pico8Sfx ParseP8Line(string line)
    {
        string trimmed = line.Trim();
        if (trimmed.Length < 8)
        {
            return new Pico8Sfx();
        }

        var sfx = new Pico8Sfx
        {
            EditorMode = int.Parse(trimmed[..2], NumberStyles.HexNumber),
            NoteDuration = int.Parse(trimmed[2..4], NumberStyles.HexNumber),
            LoopStart = int.Parse(trimmed[4..6], NumberStyles.HexNumber),
            LoopEnd = int.Parse(trimmed[6..8], NumberStyles.HexNumber),
        };

        string noteSection = trimmed[8..].Replace(" ", string.Empty, StringComparison.Ordinal);
        int noteCount = Math.Min(32, noteSection.Length / 5);

        for (int i = 0; i < noteCount; i++)
        {
            string chunk = noteSection.Substring(i * 5, 5);
            int pitch = int.Parse(chunk[..2], NumberStyles.HexNumber);
            int waveform = int.Parse(chunk[2..3], NumberStyles.HexNumber);
            int volume = int.Parse(chunk[3..4], NumberStyles.HexNumber);
            int effect = int.Parse(chunk[4..5], NumberStyles.HexNumber);

            sfx.Notes[i] = new Pico8Note
            {
                Pitch = pitch & 0x3F,
                Waveform = waveform & 0x07,
                Volume = volume & 0x07,
                Effect = effect & 0x07,
                Custom = (waveform & 0x08) != 0,
            };
        }

        return sfx;
    }
}

public static class Waveforms
{
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
            _ => 0f,
        };
    }

    static float Triangle(float t) => t < 0.5f ? (4f * t - 1f) : (3f - 4f * t);

    static float TiltedSaw(float t) =>
        t < 0.875f
            ? (2f * t / 0.875f) - 1f
            : ((1f - t) / 0.125f) * 2f - 1f;

    static float Sawtooth(float t) => 2f * t - 1f;

    static float Square(float t, float duty) => t < duty ? 1f : -1f;

    static float Organ(float t) =>
        (MathF.Sin(2f * MathF.PI * t) + 0.5f * MathF.Sin(4f * MathF.PI * t)) / 1.5f;

    static float Noise(ref float state)
    {
        uint bits = BitConverter.SingleToUInt32Bits(state);
        bits ^= bits << 13;
        bits ^= bits >> 7;
        bits ^= bits << 17;
        if (bits == 0) bits = 1;
        state = BitConverter.UInt32BitsToSingle(bits);
        return ((bits & 0xFFFF) / 32767.5f) - 1f;
    }

    static float Phaser(float t) =>
        MathF.Sin(2f * MathF.PI * (t + MathF.Sin(2f * MathF.PI * t * 3f) * 0.2f));
}

public static class Pico8Synth
{
    public const int SampleRate = 44100;

    public static float PitchToHz(int pitch) => 32.703f * MathF.Pow(2f, pitch / 12f);

    public static float NoteSeconds(int noteDuration) => noteDuration / 128f;

    public static float[] RenderSfx(Pico8Sfx sfx, int startNote = 0)
    {
        float noteSec = NoteSeconds(sfx.NoteDuration);
        int samplesPerNote = Math.Max(1, (int)(noteSec * SampleRate));
        int totalSamples = samplesPerNote * 32;

        var output = new float[totalSamples];
        float noiseState = 1f;

        for (int noteIdx = startNote; noteIdx < 32; noteIdx++)
        {
            Pico8Note note = sfx.Notes[noteIdx];
            if (note.Volume == 0)
            {
                continue;
            }

            float freq = PitchToHz(note.Pitch);
            float volNorm = note.Volume / 7f;
            int sampleStart = (noteIdx - startNote) * samplesPerNote;

            for (int sample = 0; sample < samplesPerNote; sample++)
            {
                float t = (float)sample / samplesPerNote;
                float effFreq = freq;
                float effVol = volNorm;
                ApplyEffect(note, sample, samplesPerNote, ref effFreq, ref effVol, freq, volNorm);

                float phase = sample / (float)samplesPerNote;
                float waveform = Waveforms.Sample(note.Waveform, phase, ref noiseState);
                output[sampleStart + sample] += waveform * effVol * 0.5f;
            }
        }

        return output;
    }

    static void ApplyEffect(Pico8Note note, int sample, int total, ref float freq, ref float vol, float baseFreq, float baseVol)
    {
        float t = (float)sample / total;

        switch (note.Effect)
        {
            case 1:
                freq = baseFreq * (1f + 0.05f * t);
                break;
            case 2:
                freq = baseFreq * (1f + 0.03f * MathF.Sin(2f * MathF.PI * 7f * t));
                break;
            case 3:
                freq = baseFreq * (1f - 0.5f * t);
                break;
            case 4:
                vol = baseVol * t;
                break;
            case 5:
                vol = baseVol * (1f - t);
                break;
        }
    }
}

public sealed class Pico8AudioPlayer : IDisposable
{
    const int SampleRate = Pico8Synth.SampleRate;
    const int Channels = 1;

    readonly WaveOutEvent _waveOut;
    readonly BufferedWaveProvider _buffer;

    public Pico8AudioPlayer()
    {
        var format = new WaveFormat(SampleRate, 16, Channels);
        _buffer = new BufferedWaveProvider(format) { DiscardOnBufferOverflow = true };
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_buffer);
        _waveOut.Play();
    }

    public void QueueSamples(float[] samples)
    {
        byte[] bytes = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short s = (short)Math.Clamp(samples[i] * 32767f, -32768f, 32767f);
            bytes[i * 2] = (byte)(s & 0xFF);
            bytes[i * 2 + 1] = (byte)((s >> 8) & 0xFF);
        }

        _buffer.AddSamples(bytes, 0, bytes.Length);
    }

    public void Dispose()
    {
        _waveOut.Stop();
        _waveOut.Dispose();
        _buffer.ClearBuffer();
    }
}
