using System.Globalization;

namespace sfxcc;

internal static class Program
{
    static int Main(string[] args)
    {
        string[] candidatePaths = args.Length > 0
            ? [args[0]]
            : [
                Path.Combine(Directory.GetCurrentDirectory(), "sfx.p8"),
                Path.Combine(AppContext.BaseDirectory, "sfx.p8"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "sfx.p8"),
            ];

        string? cartPath = candidatePaths.FirstOrDefault(File.Exists);
        int slotIndex = args.Length > 1 ? int.Parse(args[1], CultureInfo.InvariantCulture) : 0;
        bool playAudio = args.Length > 2 && string.Equals(args[2], "play", StringComparison.OrdinalIgnoreCase);

        if (cartPath is null)
        {
            Console.Error.WriteLine("Cart file not found. Pass a .p8 file path as the first argument.");
            return 1;
        }

        Pico8Sfx[] sfxTable = Pico8SfxParser.LoadFromP8(cartPath);
        if (slotIndex < 0 || slotIndex >= sfxTable.Length)
        {
            Console.Error.WriteLine($"SFX slot must be between 0 and {sfxTable.Length - 1}.");
            return 2;
        }

        Pico8Sfx slot = sfxTable[slotIndex];
        Console.WriteLine($"Loaded {sfxTable.Length} SFX slots from {Path.GetFileName(cartPath)}.");
        Console.WriteLine($"Slot {slotIndex}: duration={slot.NoteDuration}, loop={slot.LoopStart}->{slot.LoopEnd}, notes={slot.Notes.Count(note => note.Volume > 0)} active");

        for (int i = 0; i < 8; i++)
        {
            Pico8Note note = slot.Notes[i];
            Console.WriteLine($"  note[{i}] pitch={note.Pitch}, waveform={note.Waveform}, volume={note.Volume}, effect={note.Effect}, custom={note.Custom}");
        }

        float[] rendered = Pico8Synth.RenderSfx(slot, 0);
        Console.WriteLine($"Rendered {rendered.Length} samples (~{rendered.Length / (double)Pico8Synth.SampleRate:F2}s) from slot {slotIndex}.");
        Console.WriteLine($"Peak amplitude: {rendered.Max():F4}, RMS: {Math.Sqrt(rendered.Average(sample => sample * sample)):F4}");

        if (playAudio)
        {
            Console.WriteLine("Playing rendered SFX on the default output device...");
            using var audio = new Pico8AudioPlayer();
            audio.QueueSamples(rendered);

            Thread.Sleep((int)(rendered.Length / (double)Pico8Synth.SampleRate * 1000) + 200);
        }

        return 0;
    }
}
