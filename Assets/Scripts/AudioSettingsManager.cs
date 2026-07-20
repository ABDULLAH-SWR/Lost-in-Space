using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Sliders

public class AudioSettingsManager : MonoBehaviour
{
    // --- LATER: We will add references to your AudioSource or AudioMixer here ---

    public void SetMusicVolume(float volume)
    {
        // This runs every time the player moves the Music slider
        Debug.Log("Music Volume changed to: " + volume);

        // LATER: audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        // This runs every time the player moves the SFX slider
        Debug.Log("SFX Volume changed to: " + volume);

        // LATER: audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
}