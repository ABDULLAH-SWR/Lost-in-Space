using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject stageSelectPanel;
    public GameObject levelSelectPanel;

    private void Start()
    {
        // Check if the player just exited a level
        if (PlayerPrefs.GetInt("ReturnToLevelSelect", 0) == 1)
        {
            // Reset the flag so it doesn't get stuck
            PlayerPrefs.SetInt("ReturnToLevelSelect", 0);
            PlayerPrefs.Save();

            // Skip the main menu and go straight to the level grid
            ShowLevelSelect();
        }
        else
        {
            // Normal boot up, start on the Main Menu
            ShowMainMenu();
        }
    }

    private void Update()
    {
        // Constantly listen for the ESC key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    private void HandleEscapeKey()
    {
        // If we are on the Level Select screen, go back to Stage Select
        if (levelSelectPanel.activeSelf)
        {
            ShowStageSelect();
        }
        // If we are on the Stage Select screen, go back to Main Menu
        else if (stageSelectPanel.activeSelf)
        {
            ShowMainMenu();
        }
        // If we are already on the Main Menu, we can quit the game
        else if (mainMenuPanel.activeSelf)
        {
            QuitGame();
        }
    }

    // Call this from the "Play" button or "Back" buttons on Stage Select
    public void ShowStageSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);

        stageSelectPanel.SetActive(true);
    }

    // Call this from the "Stage 1" button or "Back" buttons on Level Select
    public void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        stageSelectPanel.SetActive(false);

        levelSelectPanel.SetActive(true);
    }

    // Call this from the "Back" button on the Stage Select screen
    public void ShowMainMenu()
    {
        stageSelectPanel.SetActive(false);
        levelSelectPanel.SetActive(false);

        mainMenuPanel.SetActive(true);
    }

    // Call this from the "Quit" button
    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }
}