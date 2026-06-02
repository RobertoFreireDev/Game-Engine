using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace playpico8sfx
{
    public sealed class Pico8SoundPlayer : IDisposable
    {
        private readonly DynamicSoundEffectInstance _instance;
        private readonly Pico8Synthesizer _synth;
        private readonly byte[] _pcmBuffer;
        private readonly object _sync = new();

        public Pico8SoundPlayer(int sampleRate = 44100, int channels = 1, int bufferMilliseconds = 100)
        {
            _instance = new DynamicSoundEffectInstance(sampleRate, AudioChannels.Mono);
            _instance.BufferNeeded += OnBufferNeeded;
            _synth = new Pico8Synthesizer(sampleRate);
            _pcmBuffer = new byte[sampleRate * Math.Max(1, bufferMilliseconds) / 1000 * sizeof(short)];
        }

        public void Play(Pico8Sound sound, bool loop = true)
        {
            lock (_sync)
            {
                _synth.LoadSound(sound);
                _synth.Loop = loop;
                SubmitBuffer();
                SubmitBuffer();
                SubmitBuffer();
                _instance.Play();
            }
        }

        public void Stop()
        {
            lock (_sync)
            {
                _instance.Stop();
            }
        }

        public void Dispose()
        {
            _instance.BufferNeeded -= OnBufferNeeded;
            _instance.Dispose();
        }

        private void OnBufferNeeded(object? sender, EventArgs e)
        {
            lock (_sync)
            {
                SubmitBuffer();
            }
        }

        private void SubmitBuffer()
        {
            for (var i = 0; i < _pcmBuffer.Length / sizeof(short); i++)
            {
                var sample = FloatToPcm(_synth.NextSample());
                _pcmBuffer[i * 2] = (byte)(sample & 0xFF);
                _pcmBuffer[i * 2 + 1] = (byte)(sample >> 8);
            }

            _instance.SubmitBuffer(_pcmBuffer);
        }

        private static short FloatToPcm(float value)
        {
            value = Math.Clamp(value, -1f, 1f);
            return (short)MathF.Round(value * short.MaxValue);
        }
    }

    public sealed class Pico8Synthesizer
    {
        private readonly int _sampleRate;
        private readonly Random _random = new(0xC0DE);
        private Pico8Sound? _sound;
        private int _currentNoteIndex;
        private int _samplesLeft;
        private int _waveType;
        private float _volume;
        private double _phase;
        private double _phaseIncrement;
        private bool _isSilent;

        public bool Loop { get; set; } = true;

        public Pico8Synthesizer(int sampleRate)
        {
            _sampleRate = sampleRate;
        }

        public void LoadSound(Pico8Sound sound)
        {
            _sound = sound;
            _currentNoteIndex = 0;
            _samplesLeft = 0;
            _phase = 0;
            _isSilent = false;
            AdvanceNote();
        }

        public float NextSample()
        {
            if (_sound == null)
            {
                return 0f;
            }

            if (_samplesLeft <= 0)
            {
                if (!AdvanceNote())
                {
                    return 0f;
                }
            }

            var sample = _isSilent ? 0f : GenerateWaveSample();
            _samplesLeft--;
            _phase += _phaseIncrement;

            return sample * _volume * 0.25f;
        }

        private bool AdvanceNote()
        {
            if (_sound == null || _sound.Notes.Count == 0)
            {
                return false;
            }

            if (_currentNoteIndex >= _sound.Notes.Count)
            {
                if (!Loop)
                {
                    _sound = null;
                    return false;
                }

                _currentNoteIndex = 0;
            }

            var note = _sound.Notes[_currentNoteIndex++];
            _isSilent = note.NoteIndex == 0;
            _waveType = note.Instrument & 3;
            _volume = Math.Clamp(note.Volume, 0f, 1f);
            _phase = 0.0;
            _phaseIncrement = _isSilent ? 0.0 : GetFrequency(note.NoteIndex) / _sampleRate;
            _samplesLeft = Math.Max(1, (int)(_sound.StepDurationSeconds * _sampleRate));

            return true;
        }

        private float GenerateWaveSample()
        {
            var phase = (float)(_phase - Math.Floor(_phase));

            return _waveType switch
            {
                0 => 1f - 4f * MathF.Abs(phase - 0.5f),
                1 => 2f * phase - 1f,
                2 => phase < 0.5f ? 1f : -1f,
                3 => (float)(_random.NextDouble() * 2.0 - 1.0),
                _ => 0f,
            };
        }

        private static double GetFrequency(int note)
        {
            const double baseNote = 110.0; // around A2
            return baseNote * Math.Pow(2.0, (note - 32) / 12.0);
        }
    }

    public sealed record Pico8Note(int NoteIndex, int Instrument, float Volume, int Effect);

    public sealed record Pico8Sound(IReadOnlyList<Pico8Note> Notes, float StepDurationSeconds = 0.08f);

    public static class Pico8SfxParser
    {
        public static IReadOnlyList<Pico8Sound> Parse(string rawSfx)
        {
            var lines = rawSfx.Replace("\r", string.Empty).Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Pico8Sound>(lines.Length);

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (line.EndsWith("\";"))
                {
                    line = line[..^2];
                }

                if (line.StartsWith("@\""))
                {
                    line = line[2..];
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    var bytes = ParseHex(line);
                    var sound = ParseSound(bytes);
                    result.Add(sound);
                }
                catch
                {
                    // swallow invalid lines; keep parsing the rest.
                }
            }

            return result;
        }

        private static Pico8Sound ParseSound(byte[] bytes)
        {
            if (bytes.Length == 84)
            {
                return ParseRaw84ByteSound(bytes);
            }

            if (bytes.Length == 32)
            {
                return ParseLegacyPico8Sound(bytes);
            }

            if (bytes.Length >= 8 && bytes.Length % 4 == 0)
            {
                return ParseGeneric4ByteSound(bytes);
            }

            return new Pico8Sound(Array.Empty<Pico8Note>());
        }

        private static Pico8Sound ParseRaw84ByteSound(byte[] bytes)
        {
            var notes = new List<Pico8Note>();
            for (var i = 4; i + 3 < bytes.Length; i += 4)
            {
                notes.Add(ParseEvent(bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3]));
            }

            return new Pico8Sound(notes, 0.075f);
        }

        private static Pico8Sound ParseLegacyPico8Sound(byte[] bytes)
        {
            var notes = new List<Pico8Note>();
            for (var i = 0; i + 3 < bytes.Length; i += 4)
            {
                notes.Add(ParseEvent(bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3]));
            }

            return new Pico8Sound(notes, 0.08f);
        }

        private static Pico8Sound ParseGeneric4ByteSound(byte[] bytes)
        {
            var notes = new List<Pico8Note>();
            for (var i = 0; i + 3 < bytes.Length; i += 4)
            {
                notes.Add(ParseEvent(bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3]));
            }

            return new Pico8Sound(notes, 0.08f);
        }

        private static Pico8Note ParseEvent(byte noteByte, byte instrumentByte, byte volumeByte, byte effectByte)
        {
            var note = DecodeNoteValue(noteByte);
            var instrument = instrumentByte & 0x07;
            var volume = Math.Clamp((volumeByte & 0x0F) / 15f, 0f, 1f);
            var effect = effectByte & 0x0F;
            return new Pico8Note(note, instrument, volume, effect);
        }

        private static int DecodeNoteValue(byte input)
        {
            var value = input & 0x3F;
            return value == 0 ? 0 : value;
        }

        private static byte[] ParseHex(string text)
        {
            var cleaned = text.Replace(" ", string.Empty).Replace("\t", string.Empty);
            if (cleaned.Length % 2 != 0)
            {
                throw new FormatException("Hex string length must be even.");
            }

            var output = new byte[cleaned.Length / 2];
            for (var i = 0; i < output.Length; i++)
            {
                output[i] = Convert.ToByte(cleaned.Substring(i * 2, 2), 16);
            }

            return output;
        }
    }
}
