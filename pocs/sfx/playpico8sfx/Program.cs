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

            // Example: play sound 3 on any available channel
            Console.WriteLine("Playing sfx(3) - sound 3 on any available channel...");
            sfx.Sfx(3);
            
            System.Threading.Thread.Sleep(2000);

            // Example: play sound 3 starting from note position 21 on any available channel
            Console.WriteLine("Playing sfx(3, -1, 21) - sound 3 from note 21 on any available channel...");
            sfx.Sfx(3, -1, 21);
            
            System.Threading.Thread.Sleep(2000);

            // Example: stop playing sound on channel 0
            Console.WriteLine("Calling sfx(-1, 0) - stop playing on channel 0...");
            sfx.Sfx(-1, 0);
            
            System.Threading.Thread.Sleep(1000);

            // Example: play sound on specific channel
            Console.WriteLine("Playing sfx(5, 0) - sound 5 on channel 0...");
            sfx.Sfx(5, 0);
            
            System.Threading.Thread.Sleep(1000);

            // Example: release looping sound on channel 0
            Console.WriteLine("Calling sfx(-2, 0) - release looping on channel 0...");
            sfx.Sfx(-2, 0);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}

