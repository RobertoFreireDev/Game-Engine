namespace framework.Sfx;

public class SfxChannel
{
    public SfxData CurrentSfx;
    public int Position;
    public double Phase; // continuous phase for waveform generation
    public double Time;
    public bool Playing;
}