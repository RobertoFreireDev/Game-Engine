using blackbox.Utils;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Linq;
using System.Text;

namespace blackbox.Sfx;

public class SfxPlayer
{
    private const int SampleRate = 44100;
    private SfxChannel[] channels = new SfxChannel[Constants.ChannelQty];
    private DynamicSoundEffectInstance sound;
    private const float FadeInSeconds = 0.01f;
    private const float FadeOutSeconds = 0.01f;
    private SfxData[] Data = new SfxData[Constants.SfxQty];

    public SfxPlayer()
    {
        for (int i = 0; i < channels.Length; i++)
        {
            channels[i] = new SfxChannel();
        }

        for (int i = 0; i < Data.Length; i++)
        {
            Data[i] = new SfxData();
        }

        sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        sound.BufferNeeded += OnBufferNeeded;
        sound.Play();
    }

    public void SetNote(int index, int noteIndex, string note)
    {
        if (!IsValidIndex(index) || !IsValidNoteIndex(noteIndex) || !IsValidNoteString(note) || Data[index] is null)
        {
            return;
        }

        int pitchDigit = (note[0] - '0') * 10 + (note[1] - '0');
        int waveDigit = note[2] - '0';
        int volumeDigit = (note[3] - '0') * 10 + (note[4] - '0');

        pitchDigit = Math.Clamp(pitchDigit, Constants.MinPitch, Constants.MaxPitch);
        waveDigit = Math.Clamp(waveDigit, Constants.MinWave, Constants.MaxWave);
        volumeDigit = Math.Clamp(volumeDigit, Constants.MinVolume, Constants.MaxVolume);

        Data[index].Notes[noteIndex] = new Note
        {
            Pitch = pitchDigit,
            Wave = volumeDigit == 0 ? Waveform.None : (Waveform)waveDigit,
            Volume = volumeDigit / 10f
        };
    }

    public void SetSfx(int index, string sound)
    {
        sound = CleanNotes(sound);
        if (!IsValidIndex(index) || !IsValidSoundString(sound))
        {
            return;
        }

        int noteCount = (sound.Length - Constants.SpeedDigits) / Constants.CharsPerNote;
        for (int i = 0; i < Math.Min(noteCount, Constants.MaxNotes); i++)
        {
            SetNote(index, i, sound.Substring(i * Constants.CharsPerNote, Constants.CharsPerNote));
        }

        Data[index].Speed = int.Parse(sound.Substring(noteCount * Constants.CharsPerNote, 2));
    }

    private string CleanNotes(string sound)
    {
        if (sound.Length < 2)
            return sound;

        string speed = sound.Substring(sound.Length - 2, 2);
        string notesPart = sound.Substring(0, sound.Length - 2);
        var groups = Enumerable.Range(0, notesPart.Length / 5)
            .Select(i => notesPart.Substring(i * 5, 5))
            .Where(g => g != "00000");
        return string.Concat(groups) + speed;
    }

    public bool IsValidNoteString(string sound)
    {
        if (string.IsNullOrEmpty(sound) || sound.Length != Constants.CharsPerNote)
            return false;

        int pitch;
        int wave;
        int volume;

        if (!int.TryParse(sound.Substring(0, 2), out pitch))
            return false;
        if (!int.TryParse(sound.Substring(2, 1), out wave))
            return false;
        if (!int.TryParse(sound.Substring(3, 2), out volume))
            return false;

        if (pitch < Constants.MinPitch || pitch > Constants.MaxPitch)
            return false;
        if (wave < Constants.MinWave || wave > Constants.MaxWave)
            return false;
        if (volume < Constants.MinVolume || volume > Constants.MaxVolume)
            return false;

        return true;
    }

    public bool IsValidSoundString(string sound)
    {
        if (string.IsNullOrEmpty(sound))
            return false;

        int notesLength = (sound.Length - Constants.SpeedDigits);

        if (notesLength % Constants.CharsPerNote != 0)
            return false;

        int noteCount = notesLength / Constants.CharsPerNote;

        if (noteCount > Constants.MaxNotes)
        {
            return false;
        }

        int speed;
        for (int i = 0; i < noteCount; i++)
        {
            if (!IsValidNoteString(sound.Substring(i * Constants.CharsPerNote, Constants.CharsPerNote)))
            {
                return false;
            }
        }

        if (!int.TryParse(sound.Substring(noteCount * Constants.CharsPerNote, 2), out speed))
            return false;

        return true;
    }

    public void ConvertStringToData(string sounds)
    {
        var data = sounds.Split('\n');
        for (int i=0; i < data.Length; i++)
        {
            SetSfx(i, data[i].TrimEnd());
        }
    }

    public StringBuilder GetSfx(int i, StringBuilder sb)
    {
        for (int j = 0; j < Constants.MaxNotes; j++)
        {
            if (Data[i]?.Notes?[j] is not null)
            {
                var note = Data[i].Notes[j];

                int pitch = note.Pitch;
                int wave = (int)note.Wave;
                int volume = (int)Math.Round(note.Volume * 10f);

                sb.Append(pitch.ToString("D2"));
                sb.Append(wave);
                sb.Append(volume.ToString("D2"));
            }
        }

        sb.Append(Data[i].Speed.ToString("D2"));

        return sb;
    }

    public string ConvertDataToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Data.Length; i++)
        {
            sb = GetSfx(i, sb);
            if (i < Data.Length - 1)
            {
                sb.Append("\n");
            }
        }
        return sb.ToString();
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Constants.SfxQty;
    }

    private bool IsValidNoteIndex(int index)
    {
        return index >= 0 && index < Constants.SfxQty;
    }

    public void PlaySfx(int index, int speed = 1,int channel = -1, int offset = 0)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        var sfx = Data[index];
        sfx.Speed = Math.Clamp(speed, Constants.MinSpeed, Constants.MaxSpeed);
        channel = Math.Clamp(channel, 0, Constants.ChannelQty);

        var ch = channels[channel];
        ch.CurrentSfx = sfx;
        ch.Position = offset;
        ch.Time = 0;
        ch.Playing = true;
        ch.Phase = 0.0;
        ch.CurrentSample = 0;
        ch.TotalSamples = (int)(sfx.Notes.Length * sfx.Speed * Constants.SpeedSfx * SampleRate);
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
                return (phase < 0.5 ? 1f : -1f) * volume;
            case Waveform.Triangle:
                return (float)((2 * Math.Abs(2 * phase - 1) - 1) * volume);
            case Waveform.Saw:
                return (float)((2 * phase - 1) * volume);
            case Waveform.Noise:
                return (float)((new Random().NextDouble() * 2 - 1) * volume);
            default:
                return 0;
        }
    }
}