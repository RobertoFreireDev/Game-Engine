using Microsoft.Xna.Framework.Audio;
using System;

namespace framework.Sfx;

public class SfxPlayer
{
    private const int SampleRate = 44100;
    private SfxChannel[] channels = new SfxChannel[4];
    private DynamicSoundEffectInstance sound;

    public SfxPlayer()
    {
        for (int i = 0; i < 4; i++)
            channels[i] = new SfxChannel();

        sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        sound.BufferNeeded += OnBufferNeeded;
        sound.Play();
    }

    public void Sfx(SfxData sfx, int channel = -1, int offset = 0)
    {
        if (channel == -1)
        {
            channel = FindFreeChannel();
            if (channel == -1) channel = 0; // overwrite channel 0 if all busy
        }

        var ch = channels[channel];
        ch.CurrentSfx = sfx;
        ch.Position = offset;
        ch.Time = 0;
        ch.Playing = true;
    }

    public void Stop(int channel)
    {
        channels[channel].Playing = false;
    }

    private int FindFreeChannel()
    {
        for (int i = 0; i < channels.Length; i++)
            if (!channels[i].Playing) return i;
        return -1;
    }

    private void OnBufferNeeded(object sender, EventArgs e)
    {
        int samples = SampleRate / 30; // about 33ms of audio
        float[] buffer = new float[samples];
        GenerateAudio(buffer, samples);
        byte[] byteBuffer = new byte[samples * sizeof(short)];
        int outIndex = 0;
        foreach (var f in buffer)
        {
            short sample = (short)(Math.Clamp(f, -1f, 1f) * short.MaxValue);
            byteBuffer[outIndex++] = (byte)(sample & 0xFF);
            byteBuffer[outIndex++] = (byte)((sample >> 8) & 0xFF);
        }
        sound.SubmitBuffer(byteBuffer);
    }

    private void GenerateAudio(float[] buffer, int samples)
    {
        Array.Clear(buffer, 0, samples);

        for (int chIndex = 0; chIndex < channels.Length; chIndex++)
        {
            var ch = channels[chIndex];
            if (!ch.Playing || ch.CurrentSfx == null) continue;

            var sfx = ch.CurrentSfx;

            for (int i = 0; i < samples; i++)
            {
                ch.Time += 1.0 / SampleRate;
                if (ch.Time >= sfx.Speed)
                {
                    ch.Time -= sfx.Speed;
                    ch.Position++;
                    if (ch.Position >= sfx.Notes.Length)
                    {
                        ch.Playing = false;
                        break;
                    }
                }

                var note = sfx.Notes[ch.Position];
                float freq = PitchToFrequency(note.Pitch);
                float t = (float)(i / (float)SampleRate);

                ch.Phase += freq / SampleRate;
                ch.Phase -= Math.Floor(ch.Phase); // keep in 0..1
                buffer[i] += GenerateWave(note.Wave, ch.Phase, note.Volume);
            }
        }
    }

    private float PitchToFrequency(int pitch)
    {
        return 440f * (float)Math.Pow(2, (pitch - 69) / 12.0);
    }

    private float GenerateWave(Waveform wave, double phase, float volume)
    {
        switch (wave)
        {
            case Waveform.Square:
                return (phase < 0.5 ? 1f : -1f) * volume;
            case Waveform.Triangle:
                return (float)((Math.Abs(phase * 2 - 1) * 2 - 1) * volume);
            case Waveform.Saw:
                return (float)((phase * 2 - 1) * volume);
            case Waveform.Noise:
                return ((float)(new Random().NextDouble() * 2 - 1) * volume);
            default:
                return 0;
        }
    }
}