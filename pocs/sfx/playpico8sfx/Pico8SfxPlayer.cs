using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace playpico8sfx
{
    public sealed class Pico8SfxPlayer : IDisposable
    {
        private readonly DynamicSoundEffectInstance _instance;
        private readonly Pico8Synthesizer[] _synths;
        private readonly byte[] _pcmBuffer;
        private readonly object _sync = new();
        private IReadOnlyList<Pico8Sound> _sounds = Array.Empty<Pico8Sound>();

        public Pico8SfxPlayer(int sampleRate = 44100, int channels = 4, int bufferMilliseconds = 100)
        {
            _instance = new DynamicSoundEffectInstance(sampleRate, AudioChannels.Mono);
            _instance.BufferNeeded += OnBufferNeeded;
            _synths = new Pico8Synthesizer[channels];
            for (var i = 0; i < channels; i++) _synths[i] = new Pico8Synthesizer(sampleRate);
            _pcmBuffer = new byte[sampleRate * Math.Max(1, bufferMilliseconds) / 1000 * sizeof(short)];
        }

        public void LoadSounds(IReadOnlyList<Pico8Sound> sounds)
        {
            _sounds = sounds ?? Array.Empty<Pico8Sound>();
        }

        public void Sfx(int id, int channel = 0, int offset = 0, bool loop = false)
        {
            lock (_sync)
            {
                if (channel < 0 || channel >= _synths.Length) return;
                if (id < 0 || id >= _sounds.Count)
                {
                    // stop when invalid id
                    _synths[channel].LoadSound(new Pico8Sound(Array.Empty<Pico8Note>()));
                    return;
                }

                var sound = _sounds[id];
                Pico8Sound sub;
                if (offset <= 0) sub = sound;
                else
                {
                    var notes = sound.Notes.Skip(offset).ToArray();
                    sub = new Pico8Sound(notes, sound.StepDurationSeconds);
                }

                _synths[channel].LoadSound(sub, 0);
                _synths[channel].Loop = loop;

                // ensure playback started
                if (_instance.State != SoundState.Playing)
                {
                    SubmitBuffer();
                    SubmitBuffer();
                    SubmitBuffer();
                    _instance.Play();
                }
            }
        }

        public void Stop(int channel)
        {
            lock (_sync)
            {
                if (channel < 0 || channel >= _synths.Length) return;
                _synths[channel].LoadSound(new Pico8Sound(Array.Empty<Pico8Note>()));
            }
        }

        public void Dispose()
        {
            _instance.BufferNeeded -= OnBufferNeeded;
            _instance.Dispose();
        }

        private void OnBufferNeeded(object? sender, EventArgs e)
        {
            lock (_sync) SubmitBuffer();
        }

        private void SubmitBuffer()
        {
            for (var i = 0; i < _pcmBuffer.Length / sizeof(short); i++)
            {
                float mixed = 0f;
                for (var s = 0; s < _synths.Length; s++) mixed += _synths[s].NextSample();
                mixed = Math.Clamp(mixed, -1f, 1f);
                var sample = FloatToPcm(mixed);
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
}
