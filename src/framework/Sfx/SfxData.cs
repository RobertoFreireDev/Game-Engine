using framework.Utils;

namespace framework.Sfx;

public class SfxData
{
    public Note[] Notes = new Note[32];
    public float Speed = 0.02f;

    public void SetSpeed(int speed)
    {
        speed = CalcUtils.Clamp(speed, 1, 32);
        Speed = speed * 0.02f;
    }
}