using blackbox.Utils;
using Microsoft.Xna.Framework.Audio;
using System;

namespace blackbox.Sfx;

public static class AudioLib
{
    private static SfxChannel[] Channels = new SfxChannel[Constants.ChannelQty];
    private static DynamicSoundEffectInstance Sound;
    private const int SampleRate = 44100;
    private const float FadeInSeconds = 0.01f;
    private const float FadeOutSeconds = 0.01f;

    public static void CreateSound()
    {
        for (int i = 0; i < Channels.Length; i++)
        {
            Channels[i] = new SfxChannel();
        }
        Sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        Sound.BufferNeeded += OnBufferNeeded;
        Sound.Play();
    }

    public static void Play(SfxData sfx, int channel = -1, int offset = 0)
    {
        channel = Math.Clamp(channel, 0, Constants.ChannelQty - 1);
        var ch = Channels[channel];
        ch.CurrentSfx = sfx;
        ch.Position = offset;
        ch.Time = 0;
        ch.Playing = true;
        ch.Phase = 0.0;
        ch.CurrentSample = 0;
        ch.TotalSamples = (int)(sfx.Notes.Length * sfx.Speed * Constants.SpeedSfx * SampleRate);
    }

    public static void Stop(int channel)
    {
        if (channel < 0 || channel > Channels.Length)
        {
            return;
        }
        Channels[channel].Playing = false;
    }

    public static void OnBufferNeeded(object sender, EventArgs e)
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
        Sound.SubmitBuffer(byteBuffer);
    }

    private static void GenerateAudio(float[] buffer, int samples)
    {
        Array.Clear(buffer, 0, samples);

        for (int chIndex = 0; chIndex < Channels.Length; chIndex++)
        {
            var ch = Channels[chIndex];
            if (!ch.Playing || ch.CurrentSfx == null) continue;

            var sfx = ch.CurrentSfx;
            var speed = sfx.Speed * Constants.SpeedSfx;

            for (int i = 0; i < samples; i++)
            {
                ch.Time += 1.0 / SampleRate;
                ch.CurrentSample++;

                if (ch.Time >= speed)
                {
                    ch.Time -= speed;
                    ch.Position++;
                    if (ch.Position >= sfx.Notes.Length)
                    {
                        ch.Playing = false;
                        break;
                    }
                }

                var note = sfx.Notes[ch.Position];
                float freq = PitchToFrequency(note.Pitch);

                ch.Phase += freq / SampleRate;
                ch.Phase -= Math.Floor(ch.Phase);

                float sample = GenerateWave(note.Wave, ch.Phase, note.Volume);

                // Fade-in / fade-out
                float gain = 1f;
                int fadeInSamples = (int)(FadeInSeconds * SampleRate);
                int fadeOutSamples = (int)(FadeOutSeconds * SampleRate);

                if (ch.CurrentSample < fadeInSamples)
                    gain = ch.CurrentSample / (float)fadeInSamples;
                else if (ch.CurrentSample > ch.TotalSamples - fadeOutSamples)
                    gain = (ch.TotalSamples - ch.CurrentSample) / (float)fadeOutSamples;

                buffer[i] += sample * gain;
            }
        }

        // 2. Find max absolute value
        float max = 0f;
        for (int i = 0; i < samples; i++)
            max = Math.Max(max, Math.Abs(buffer[i]));

        // 3. If needed, scale down to fit [-1, 1]
        if (max > 1f)
        {
            float scale = 1f / max;
            for (int i = 0; i < samples; i++)
                buffer[i] *= scale;
        }
    }

    private static float PitchToFrequency(int pitch)
    {
        return 440f * (float)Math.Pow(2, (pitch - 69) / 12.0);
    }

    private static float GenerateWave(Waveform wave, double phase, float volume)
    {
        switch (wave)
        {
            case Waveform.Square:
                return (phase < 0.5 ? 1f : -1f) * volume * 0.577f;
            case Waveform.Saw:
                return (float)((2 * phase - 1) * volume);
            case Waveform.Noise:
                return (float)((new Random().NextDouble() * 2 - 1) * volume);
            default:
                return 0;
        }
    }
}
