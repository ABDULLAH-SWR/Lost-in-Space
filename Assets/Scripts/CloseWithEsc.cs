using UnityEngine;

public class CloseWithESC : MonoBehaviour
{
    void Update()
    {
        // Checks if the panel is currently active AND the player hits ESC
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false); // Hides the panel

            // Optional: Play your UI click sound so it feels responsive!
            if (SoundManager.instance != null) SoundManager.instance.PlayUIClick();
        }
    }
}