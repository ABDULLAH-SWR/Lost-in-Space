using UnityEngine;
using UnityEngine.UI;

public class AutoButtonSounds : MonoBehaviour
{
    void Start()
    {
        // Find all Button components anywhere inside this Canvas
        Button[] allButtons = GetComponentsInChildren<Button>(true);

        foreach (Button btn in allButtons)
        {
            // Remove any existing listeners first to prevent duplicates, 
            // then add the click sound function dynamically
            btn.onClick.RemoveListener(PlayClickSound);
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayUIClick();
        }
    }
}