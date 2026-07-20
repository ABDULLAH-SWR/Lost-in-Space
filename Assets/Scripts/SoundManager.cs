using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // --- SINGLETON SETUP ---
    public static SoundManager instance;

    [Header("Audio Sources")]
    [Tooltip("Attach an Audio Source here, set Volume to ~0.3, check Loop")]
    public AudioSource musicSource;
    [Tooltip("Attach an Audio Source here, leave Loop unchecked")]
    public AudioSource sfxSource;

    [Header("Music Tracks")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    [Header("Gameplay SFX")]
    public AudioClip slingshotRelease;
    public AudioClip wallBounce;
    public AudioClip spikeBurst;
    public AudioClip respawnSound;

    [Header("UI & System SFX")]
    public AudioClip winJingle;
    public AudioClip loseJingle;
    public AudioClip uiClick;
    public AudioClip typewriterBlip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // --- NEW: Load saved volume preferences when the game starts ---
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- NEW: VOLUME CONTROLS ---
    public void LoadVolumeSettings()
    {
        // Defaults to 0.3 for music and 1.0 for SFX if no save data exists
        if (musicSource != null) musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        if (sfxSource != null) sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null) musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Saves the setting
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume); // Saves the setting
    }

    // --- NEW: Listen for scene changes ---
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Check manually on first boot
        CheckMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically check every time a new level or menu loads
        CheckMusic(scene.name);
    }

    private void CheckMusic(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            PlayMainMenuMusic();
        }
        else
        {
            PlayGameplayMusic();
        }
    }

    // --- MUSIC METHODS ---

    public void PlayMainMenuMusic()
    {
        if (musicSource.clip != mainMenuMusic)
        {
            musicSource.clip = mainMenuMusic;
            musicSource.Play();
        }
    }

    public void PlayGameplayMusic()
    {
        if (musicSource.clip != gameplayMusic)
        {
            musicSource.clip = gameplayMusic;
            musicSource.Play();
        }
    }

    // --- SFX METHODS ---

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Gameplay Triggers
    public void PlaySlingshotRelease() => PlaySFX(slingshotRelease);
    public void PlayWallBounce() => PlaySFX(wallBounce);
    public void PlaySpikeBurst() => PlaySFX(spikeBurst);
    public void PlayRespawn() => PlaySFX(respawnSound);

    // UI & State Triggers
    public void PlayWinJingle() => PlaySFX(winJingle);
    public void PlayLoseJingle() => PlaySFX(loseJingle);
    public void PlayUIClick() => PlaySFX(uiClick);
    public void PlayTypewriter() => PlaySFX(typewriterBlip);
}