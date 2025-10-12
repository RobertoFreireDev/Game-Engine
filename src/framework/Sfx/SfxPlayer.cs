using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;

namespace blackbox.Sfx;

public static class SfxPlayer
{
    private static Dictionary<string,SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

    public static void Load(ContentManager content)
    {
        _soundEffects.Clear();
        string sfxFolder = Path.Combine(content.RootDirectory, "sfx");

        if (!Directory.Exists(sfxFolder))
        {
            return;
        }

        var files = Directory.GetFiles(sfxFolder, "*.xnb", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string assetName = Path.GetRelativePath(content.RootDirectory, file)
                .Replace(".xnb", "")
                .Replace("\\", "/");

            string key = Path.GetFileNameWithoutExtension(file).ToLower();

            SoundEffect sfx = content.Load<SoundEffect>(assetName);

            if (!_soundEffects.ContainsKey(key))
                _soundEffects.Add(key, sfx);
        }
    }

    public static void PlaySfx(string index)
    {
        _soundEffects.TryGetValue(index, out SoundEffect soundEffect);

        if (soundEffect is null)
        {
            return;
        }
        soundEffect.Play();
    }
}