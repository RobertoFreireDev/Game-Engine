using framework.Utils;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Reflection;
using System.Threading.Channels;

namespace framework.Sfx;

public class SfxPlayer
{
    private const int SampleRate = 44100;
    private SfxChannel[] channels = new SfxChannel[4];
    private DynamicSoundEffectInstance sound;
    private const float FadeInSeconds = 0.01f;
    private const float FadeOutSeconds = 0.01f;
    private SfxData[] Data = new SfxData[256];

    public SfxPlayer()
    {
        for (int i = 0; i < 4; i++)
            channels[i] = new SfxChannel();

        sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        sound.BufferNeeded += OnBufferNeeded;
        sound.Play();
    }

    public void SetSfx(int index, string sound)
    {
        if (!ValidIndex(index))
        {
            return;
        }

        var sfx = new SfxData();
        int maxNotes = 32;
        int charsPerNote = 5;
        int noteCount = sound.Length / charsPerNote;
        for (int i = 0; i < Math.Min(noteCount, maxNotes); i++)
        {
            int pitchDigit = (sound[i * charsPerNote] - '0') * 10 + (sound[i * charsPerNote + 1] - '0');
            int waveDigit = sound[i * charsPerNote + 2] - '0';
            int volumeDigit = (sound[i * charsPerNote + 3] - '0') * 10 + (sound[i * charsPerNote + 4] - '0');

            pitchDigit = CalcUtils.Clamp(pitchDigit, 36, 71);
            waveDigit = CalcUtils.Clamp(waveDigit, 0, 4);
            volumeDigit = CalcUtils.Clamp(volumeDigit, 0, 10);            

            sfx.Notes[i] = new Note
            {
                Pitch = pitchDigit,
                Wave = volumeDigit == 0 ? Waveform.None : (Waveform)waveDigit,
                Volume = volumeDigit / 10f
            };
        }

        Data[index] = sfx;
    }

    private bool ValidIndex(int index)
    {
        return index >= 0 && index < channels.Length;
    }

    public void PlaySfx(int index, int speed = 1,int channel = -1, int offset = 0)
    {
        if (!ValidIndex(index))
        {
            return;
        }

        var sfx = Data[index];
        sfx.SetSpeed(speed);
        channel = CalcUtils.Clamp(channel, 0, 4);

        var ch = channels[channel];
        ch.CurrentSfx = sfx;
        ch.Position = offset;
        ch.Time = 0;
        ch.Playing = true;
        ch.Phase = 0.0;
        ch.CurrentSample = 0;
        ch.TotalSamples = (int)(sfx.Notes.Length * sfx.Speed * SampleRate);
    }

    public void Stop(int channel)
    {
        if (channel < 0 || channel > channels.Length)
        {
            return;
        }
        channels[channel].Playing = false;
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
                ch.CurrentSample++;

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

                ch.Phase += freq / SampleRate;
                ch.Phase -= Math.Floor(ch.Phase);

                float sample = GenerateWave(note.Wave, ch.Phase, note.Volume);

                // Calculate fade-in and fade-out gain multiplier
                float gain = 1f;
                int fadeInSamples = (int)(FadeInSeconds * SampleRate);
                int fadeOutSamples = (int)(FadeOutSeconds * SampleRate);

                if (ch.CurrentSample < fadeInSamples)
                {
                    gain = ch.CurrentSample / (float)fadeInSamples; // ramp up 0->1
                }
                else if (ch.CurrentSample > ch.TotalSamples - fadeOutSamples)
                {
                    gain = (ch.TotalSamples - ch.CurrentSample) / (float)fadeOutSamples; // ramp down 1->0
                    gain = Math.Clamp(gain, 0f, 1f);
                }

                buffer[i] += sample * gain;
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
                return (phase < 0.5 ? 1f : -1f) * volume * 0.5f;
            case Waveform.Triangle:
                return (float)(4f * volume * Math.Abs(2 * phase - 1) - 1f);
            case Waveform.Saw:
                return (float)((phase * 2 - 1) * volume) * 0.8f;
            case Waveform.Noise:
                return ((float)(new Random().NextDouble() * 2 - 1) * volume);
            default:
                return 0;
        }
    }
}