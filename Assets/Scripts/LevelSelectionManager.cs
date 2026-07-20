using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Level Buttons")]
    // Drag your 10 level buttons into this array in the Inspector
    public Button[] levelButtons;

    private void Start()
    {
        // Check the saved data for "ReachedLevel". 
        // If it doesn't exist (new player), default it to 1.
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        // Loop through all our buttons to lock or unlock them
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > reachedLevel)
            {
                // 1. Lock the button
                levelButtons[i].interactable = false;

                // 2. --- NEW: Fade the actual Button Image to 50% opacity ---
                Image buttonImage = levelButtons[i].GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);
                }

                // 3. Fade the Text out to 50% opacity
                TMP_Text buttonText = levelButtons[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 0.5f);
                }
            }
            else
            {
                // 1. Unlock the button
                levelButtons[i].interactable = true;

                // 2. --- NEW: Ensure the unlocked Button Image is 100% visible ---
                Image buttonImage = levelButtons[i].GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
                }
            }
        }
    }

    // Connect this to the OnClick event of your UI Buttons
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}