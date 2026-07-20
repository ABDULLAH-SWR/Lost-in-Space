using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // When the panel opens, set the sliders to match the current saved volume
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        }
    }

    // These methods will be called automatically when you drag the slider handles
    public void OnMusicSliderMoved(float value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetMusicVolume(value);
        }
    }

    public void OnSFXSliderMoved(float value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetSFXVolume(value);
        }
    }
}