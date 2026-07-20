using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Visual Effects")]
    public ParticleSystem winParticles;

    [Header("Game Rules")]
    public int totalChances = 3;
    private int currentChances;

    [Tooltip("Type the number of THIS level (e.g., 1 for Level 1)")]
    public int currentLevelNumber = 1;
    public string mainMenuSceneName = "MainMenu";

    [Header("UI Elements")]
    public TextMeshProUGUI chancesTextDisplay;
    [Tooltip("Drag your hidden Win Panel here")]
    public GameObject winPanel;
    [Tooltip("Drag your hidden Lose Panel here")]
    public GameObject losePanel; // --- NEW: Added the Lose Panel reference back! ---

    // The Multi-Trigger Lock
    private bool isHandlingMiss = false;

    void Start()
    {
        currentChances = totalChances;
        UpdateChancesUI();

        // Ensure both panels are hidden when the level starts
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false); // --- NEW ---

        if (SoundManager.instance != null) SoundManager.instance.PlayGameplayMusic();
    }

    public void TriggerWin()
    {
        // Lock the game so they can't lose (or win twice) after winning
        if (isHandlingMiss) return;
        isHandlingMiss = true;

        Debug.Log("Level Win! Success! Playing particles...");
        if (winParticles != null) winParticles.Play(); // Will now play normally!

        if (SoundManager.instance != null) SoundManager.instance.PlayWinJingle();
        // 1. Calculate the next level to unlock
        int nextLevel = currentLevelNumber + 1;

        // 2. Save the progress if it's a new high-water mark
        if (nextLevel > PlayerPrefs.GetInt("ReachedLevel", 1))
        {
            PlayerPrefs.SetInt("ReachedLevel", nextLevel);
            PlayerPrefs.Save();
            Debug.Log("Unlocked Level " + nextLevel);
        }

        // 3. Show the Win Panel after a short delay
        StartCoroutine(ShowWinPanelAfterDelay(1.0f));
    }

    private IEnumerator ShowWinPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        PlayerPrefs.SetInt("ReturnToLevelSelect", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RegisterMiss()
    {
        // If we are already processing a miss or a win, completely ignore this request
        if (isHandlingMiss == true) return;

        currentChances--;
        UpdateChancesUI();

        if (currentChances > 0)
        {
            isHandlingMiss = true;
            Debug.Log("Missed! Chances left: " + currentChances + ". Respawning...");
            StartCoroutine(RespawnPlayerAfterDelay(0.5f));
        }
        else
        {
            isHandlingMiss = true;
            TriggerLoss();
        }
    }

    public void TriggerLoss()
    {
        if (SoundManager.instance != null) SoundManager.instance.PlayLoseJingle();
        Debug.Log("Out of chances! Showing Lose Panel...");
        StartCoroutine(ShowLosePanelAfterDelay(1.0f)); // --- NEW: Delays the fail screen ---
    }

    // --- NEW: Coroutine to show the Fail UI ---
    private IEnumerator ShowLosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }

    // --- NEW: Triggered by the "Restart" button on the Lose Panel ---
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void UpdateChancesUI()
    {
        if (chancesTextDisplay != null)
        {
            chancesTextDisplay.text = "x" + currentChances;
        }
    }

    private IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (SoundManager.instance != null) SoundManager.instance.PlayRespawn();

        SlingshotLauncher player = FindObjectOfType<SlingshotLauncher>();
        if (player != null)
        {
            player.ResetShot();
        }

        isHandlingMiss = false;
    }
}