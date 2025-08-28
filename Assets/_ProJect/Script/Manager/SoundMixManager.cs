using UnityEngine;
using UnityEngine.Audio;

public class SoundMixManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat(NameMixManager.MasterVolume, Mathf.Log10(level) * 20);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat(NameMixManager.SoundFX, Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat(NameMixManager.Music, Mathf.Log10(level) * 20);
    }
}
