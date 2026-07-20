using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject draftPanel; // Your Card Menu
    public GameObject hintPanel;  // The new Hint Popup

    [Header("HUD")]
    public TextMeshProUGUI activeCardText;

    [Header("Hint Settings")]
    public TextMeshProUGUI hintTextDisplay;
    [TextArea]
    public string levelHint = "Type your level-specific hint here in the Inspector!";


    public static int savedCardIndex = 3;

    void Start()
    {
        // 1. Start the game in NORMAL mode
        Time.timeScale = 1f;

        // 2. Hide all popups on load
        if (draftPanel != null) draftPanel.SetActive(false);
        if (hintPanel != null) hintPanel.SetActive(false);

        ApplySavedCard();
    }

    private void ApplySavedCard()
    {
        if (CardManager.instance != null)
        {
            CardManager.instance.ApplyCard(savedCardIndex);
        }
        UpdateHUDText(savedCardIndex);
    }

    private void UpdateHUDText(int index)
    {
        if (activeCardText == null) return;

        // Update these names to match your exact card order in the Inspector!
        switch (index)
        {
            case 0: activeCardText.text = "Card: Anvil"; break;
            case 1: activeCardText.text = "Card: Flubber"; break;
            case 2: activeCardText.text = "Card: Astronaut"; break;
            case 3: activeCardText.text = "Card: Normal"; break;
            default: activeCardText.text = "Card: Normal"; break;
        }
    }

    // --- CARD MENU LOGIC ---
    public void OpenCardMenu()
    {
        SlingshotLauncher player = FindAnyObjectByType<SlingshotLauncher>();
        if (player != null && player.hasFired == true)
        {
            Debug.Log("Cannot change cards in mid-air!");
            return;
        }
        Time.timeScale = 0f; // Pause the game
        if (draftPanel != null) draftPanel.SetActive(true);
    }

    // --- CARD SELECTION LOGIC ---
    public void SelectCard(int cardIndex)
    {
        savedCardIndex = cardIndex;
        // Apply the chosen card using your CardManager
        if (CardManager.instance != null)
        {
            CardManager.instance.ApplyCard(cardIndex);
        }
        UpdateHUDText(cardIndex);
        // Close the menu and unpause the game
        CloseCardMenu();
    }

    public void CloseCardMenu()
    {
        Time.timeScale = 1f; // Unpause the game
        if (draftPanel != null) draftPanel.SetActive(false);
    }

    // --- HINT SYSTEM LOGIC ---
    public void OpenHint()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
            if (hintTextDisplay != null)
            {
                hintTextDisplay.text = levelHint;
            }
        }
    }

    public void CloseHint()
    {
        if (hintPanel != null) hintPanel.SetActive(false);
    }

    // --- RESTART LOGIC ---
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Fixes a bug where reloading a paused game keeps it paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}