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

            using var player = new Pico8SoundPlayer(sampleRate: 44100, channels: 1);
            player.Play(sfxList[12], loop: true);

            Console.WriteLine("Playing first PICO-8 SFX. Press Enter to stop.");
            Console.ReadLine();

            player.Stop();
        }
    }
}

