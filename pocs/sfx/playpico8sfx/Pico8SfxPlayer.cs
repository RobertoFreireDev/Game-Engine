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
        private readonly bool[] _isPlaying;

        public Pico8SfxPlayer(int sampleRate = 44100, int channels = 4, int bufferMilliseconds = 100)
        {
            _instance = new DynamicSoundEffectInstance(sampleRate, AudioChannels.Mono);
            _instance.BufferNeeded += OnBufferNeeded;
            _synths = new Pico8Synthesizer[channels];
            _isPlaying = new bool[channels];
            for (var i = 0; i < channels; i++) _synths[i] = new Pico8Synthesizer(sampleRate);
            _pcmBuffer = new byte[sampleRate * Math.Max(1, bufferMilliseconds) / 1000 * sizeof(short)];
        }

        public void LoadSounds(IReadOnlyList<Pico8Sound> sounds)
        {
            _sounds = sounds ?? Array.Empty<Pico8Sound>();
        }

        public void Sfx(int id, int channel = -1, int offset = 0)
        {
            lock (_sync)
            {
                // Stop playing sound on specified channel
                if (id == -1)
                {
                    if (channel >= 0 && channel < _synths.Length)
                    {
                        _synths[channel].LoadSound(new Pico8Sound(Array.Empty<Pico8Note>()));
                        _isPlaying[channel] = false;
                    }
                    return;
                }

                // Release looping sound on specified channel
                if (id == -2)
                {
                    if (channel >= 0 && channel < _synths.Length)
                    {
                        _synths[channel].Loop = false;
                    }
                    return;
                }

                // Play sound on specified or available channel
                if (id < 0 || id >= _sounds.Count)
                {
                    return;
                }

                // Find available channel if -1 was specified
                int targetChannel = channel;
                if (channel == -1)
                {
                    targetChannel = -1;
                    for (var i = 0; i < _synths.Length; i++)
                    {
                        if (!_isPlaying[i])
                        {
                            targetChannel = i;
                            break;
                        }
                    }
                    if (targetChannel == -1)
                    {
                        return; // No available channel
                    }
                }

                if (targetChannel < 0 || targetChannel >= _synths.Length)
                {
                    return;
                }

                var sound = _sounds[id];
                Pico8Sound sub;
                if (offset <= 0)
                {
                    sub = sound;
                }
                else
                {
                    var notes = sound.Notes.Skip(offset).ToArray();
                    sub = new Pico8Sound(notes, sound.StepDurationSeconds);
                }

                _synths[targetChannel].LoadSound(sub, 0);
                _synths[targetChannel].Loop = false;
                _isPlaying[targetChannel] = true;

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
                _isPlaying[channel] = false;
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
