using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInputManager : MonoBehaviour
{
    // The exact name of your main menu scene
    public string mainMenuSceneName = "MainMenu";

    void Update()
    {
        // Listen for the Esc key during gameplay
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        // Set the flag so the menu knows to open the Level Select panel
        PlayerPrefs.SetInt("ReturnToLevelSelect", 1);
        PlayerPrefs.Save();

        // Load the Main Menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}