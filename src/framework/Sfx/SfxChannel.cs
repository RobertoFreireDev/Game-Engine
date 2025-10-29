using blackbox.Utils;
using Microsoft.Xna.Framework.Audio;
using System;

namespace blackbox.Sfx;

public class SfxChannel
{
    public SfxData CurrentSfx;
    public DynamicSoundEffectInstance Sound;
    public int Position;
    public double Phase; // continuous phase for waveform generation
    public double Time;
    public bool Playing;
    public int CurrentSample;    // position in samples from start of entire sfx
    public int TotalSamples;     // total samples in entire sfx (calculated once)
    private const int SampleRate = 44100;
    private const float FadeInSeconds = 0.01f;
    private const float FadeOutSeconds = 0.01f;

    public void CreateSound()
    {
        Sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        Sound.BufferNeeded += OnBufferNeeded;
        Sound.Play();
    }

    public void OnBufferNeeded(object sender, EventArgs e)
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

    public void Play(SfxData sfx)
    {
        CurrentSfx = sfx;
        Position = 0;
        Time = 0;
        Playing = true;
        Phase = 0.0;
        CurrentSample = 0;
        TotalSamples = (int)(sfx.Notes.Length * sfx.Speed * Constants.SpeedSfx * SampleRate);
    }

    private void GenerateAudio(float[] buffer, int samples)
    {
        Array.Clear(buffer, 0, samples);

        if (Playing && CurrentSfx != null)
        {
            var sfx = CurrentSfx;
            var speed = sfx.Speed * Constants.SpeedSfx;

            for (int i = 0; i < samples; i++)
            {
                Time += 1.0 / SampleRate;
                CurrentSample++;

                if (Time >= speed)
                {
                    Time -= speed;
                    Position++;
                    if (Position >= sfx.Notes.Length)
                    {
                        Playing = false;
                        break;
                    }
                }

                var note = sfx.Notes[Position];
                float freq = PitchToFrequency(note.Pitch);

                Phase += freq / SampleRate;
                Phase -= Math.Floor(Phase);

                float sample = GenerateWave(note.Wave, Phase, note.Volume);

                float gain = 1f;
                int fadeInSamples = (int)(FadeInSeconds * SampleRate);
                int fadeOutSamples = (int)(FadeOutSeconds * SampleRate);

                if (CurrentSample < fadeInSamples)
                    gain = CurrentSample / (float)fadeInSamples;
                else if (CurrentSample > TotalSamples - fadeOutSamples)
                    gain = (TotalSamples - CurrentSample) / (float)fadeOutSamples;

                buffer[i] += sample * gain;
            }
        }

        float max = 0f;
        for (int i = 0; i < samples; i++)
            max = Math.Max(max, Math.Abs(buffer[i]));

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
            default:
                return 0;
        }
    }
}