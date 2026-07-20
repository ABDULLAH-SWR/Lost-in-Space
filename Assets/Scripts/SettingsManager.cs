using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public GameObject instructionsPanel; // --- NEW: Reference to Instructions ---
    public GameObject mainMenuPanel;

    [Header("Confirmation Prompt")]
    public GameObject confirmationPanel;

    void Update()
    {
        // Handle ESC key logic (Checks in order of priority)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 1. If the confirmation prompt is open, close just the prompt
            if (confirmationPanel != null && confirmationPanel.activeInHierarchy)
            {
                CloseConfirmation();
            }
            // 2. If settings is open, close settings
            else if (settingsPanel != null && settingsPanel.activeInHierarchy)
            {
                CloseSettings();
            }
            // 3. --- NEW: If instructions is open, close instructions ---
            else if (instructionsPanel != null && instructionsPanel.activeInHierarchy)
            {
                CloseInstructions();
            }
        }
    }

    // --- SETTINGS METHODS ---
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (instructionsPanel != null) instructionsPanel.SetActive(false); // Prevents overlap
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // --- NEW: INSTRUCTIONS METHODS ---
    public void OpenInstructions()
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false); // Prevents overlap
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    public void CloseInstructions()
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // --- METHODS FOR THE RESET PROMPT ---
    public void OpenConfirmation()
    {
        if (confirmationPanel != null) confirmationPanel.SetActive(true);
    }

    public void CloseConfirmation()
    {
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    public void ConfirmResetData()
    {
        PlayerPrefs.DeleteKey("ReachedLevel");
        PlayerPrefs.Save();
        CloseConfirmation();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}