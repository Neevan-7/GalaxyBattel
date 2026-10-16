
using UnityEngine;
using UnityEngine.Audio;

public static class SettingsStorage
{
    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SfxKey = "SFXVolume";

    public const float DefaultMaster = 100f;
    public const float DefaultMusic = 100f;
    public const float DefaultSFX = 100f;

    public static float Master =>
        PlayerPrefs.GetFloat(MasterKey, DefaultMaster);

    public static float Music =>
        PlayerPrefs.GetFloat(MusicKey, DefaultMusic);

    public static float SFX =>
        PlayerPrefs.GetFloat(SfxKey, DefaultSFX);

    public static void Save(float master, float music, float sfx)
    {
        PlayerPrefs.SetFloat(MasterKey, master);
        PlayerPrefs.SetFloat(MusicKey, music);
        PlayerPrefs.SetFloat(SfxKey, sfx);
        PlayerPrefs.Save();
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static float Load(string key, float defaultValue = 100f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void Load()
    {
        // Stored values are read through Master, Music, and SFX.
    }

    public static void Reset()
    {
        Save(DefaultMaster, DefaultMusic, DefaultSFX);
    }

    public static void Apply(
        AudioMixer mixer,
        string masterParameter = "MasterVolume",
        string musicParameter = "MusicVolume",
        string sfxParameter = "SFXVolume")
    {
        float master = Master / 100f;
        AudioListener.volume = master;

        if (mixer == null)
            return;

        SetMixerVolume(mixer, masterParameter, master);
        SetMixerVolume(mixer, musicParameter, Music / 100f);
        SetMixerVolume(mixer, sfxParameter, SFX / 100f);
    }

    private static void SetMixerVolume(
        AudioMixer mixer, string parameter, float value)
    {
        float decibels = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        if (!mixer.SetFloat(parameter, decibels))
        {
            Debug.LogWarning(
                "Check that Audio Mixer parameter '" + parameter +
                "' exists and is exposed.");
        }
    }
}
