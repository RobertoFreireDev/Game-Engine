using System;

namespace playpico8sfx
{
    internal static class Program
    {
        private static void Main()
        {
            var sfxList = Pico8SfxParser.Parse(Files.Sfx);
            if (sfxList.Count == 0)
            {
                Console.WriteLine("No SFX data could be parsed.");
                return;
            }

            using var sfx = new Pico8SfxPlayer(sampleRate: 44100, channels: 4);
            sfx.LoadSounds(sfxList);

            Console.WriteLine("Playing PICO-8 sfx(12) on channel 0. Press Enter to stop.");
            sfx.Sfx(32, channel: 0, offset: 0, loop: true);

            Console.ReadLine();

            sfx.Stop(0);
        }
    }
}

