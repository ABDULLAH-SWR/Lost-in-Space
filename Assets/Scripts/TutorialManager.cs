using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("UI References")]
    public TextMeshProUGUI tutorialText;
    public GameObject hintPanel;

    [Header("Story Settings")]
    [Tooltip("Type your story line by line here!")]
    public string[] storyLines;
    private int currentStoryIndex = 0;

    [Tooltip("Time in seconds between each letter appearing")]
    public float typingSpeed = 0.04f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    [Header("Tutorial Settings")]
    public Button[] lockedCardButtons;

    public enum TutorialState
    {
        StorySequence,
        SelectCard,
        AimSlingshot,
        Shoot,
        Completed
    }

    public TutorialState currentState;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Lock buttons immediately so players can't click them early
        if (lockedCardButtons != null)
        {
            foreach (Button cardBtn in lockedCardButtons)
            {
                if (cardBtn != null) cardBtn.interactable = false;
            }
        }

        // --- THE FIX: Start the delayed initialization ---
        StartCoroutine(InitializeTutorialWithDelay());
    }

    // --- NEW: The micro-delay to fix the WebGL race condition ---
    private IEnumerator InitializeTutorialWithDelay()
    {
        // Wait a tiny fraction of a second for all other level scripts to finish loading
        yield return new WaitForSeconds(0.1f);

        currentState = TutorialState.StorySequence;
        UpdateTutorialUI();
    }

    public void UpdateTutorialUI()
    {
        switch (currentState)
        {
            case TutorialState.StorySequence:
                if (hintPanel != null) hintPanel.SetActive(true);

                // Stop any existing typing before starting a new line
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeStoryText(storyLines[currentStoryIndex]));
                break;

            case TutorialState.SelectCard:
                if (hintPanel != null) hintPanel.SetActive(true);
                tutorialText.text = "STEP 1: Open the Cards Panel and select the 'NORMAL' card.";
                break;

            case TutorialState.AimSlingshot:
                if (hintPanel != null) hintPanel.SetActive(true);
                tutorialText.text = "STEP 2: Click and drag the Astronaut backwards from anywhere to aim.";
                break;

            case TutorialState.Shoot:
                if (hintPanel != null) hintPanel.SetActive(true);
                tutorialText.text = "STEP 3: Release to launch! Try to Hit the target.";
                break;

            case TutorialState.Completed:
                if (hintPanel != null) hintPanel.SetActive(false);
                break;
        }
    }

    private IEnumerator TypeStoryText(string lineToType)
    {
        isTyping = true;
        tutorialText.text = "";

        int letterCount = 0;

        // Loop through each letter in the sentence one by one
        foreach (char letter in lineToType.ToCharArray())
        {
            tutorialText.text += letter;

            // Skip spaces, and only play the sound every 2nd letter 
            if (letter != ' ' && letterCount % 2 == 0)
            {
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.PlayTypewriter();
                }
            }

            letterCount++;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Once the sentence is fully typed, add the "Tap to continue" prompt
        tutorialText.text += "\n\n<size=60%><color=#AAAAAA><i>(Tap here to continue)</i></color></size>";
        isTyping = false;
    }

    public void AdvanceStory()
    {
        if (currentState == TutorialState.StorySequence)
        {
            // If the text is currently typing out, clicking will instantly fill it
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);

                tutorialText.text = storyLines[currentStoryIndex] + "\n\n<size=60%><color=#AAAAAA><i>(Tap here to continue)</i></color></size>";
                isTyping = false;
            }
            // If the text is already finished, move to the next line
            else
            {
                currentStoryIndex++;

                if (currentStoryIndex < storyLines.Length)
                {
                    UpdateTutorialUI();
                }
                else
                {
                    currentState = TutorialState.SelectCard;
                    UpdateTutorialUI();
                }
            }
        }
    }

    // --- TRIGGER METHODS ---
    public void OnCardSelected()
    {
        if (currentState == TutorialState.SelectCard)
        {
            currentState = TutorialState.AimSlingshot;
            UpdateTutorialUI();
        }
    }

    public void OnSlingshotGrabbed()
    {
        if (currentState == TutorialState.AimSlingshot)
        {
            currentState = TutorialState.Shoot;
            UpdateTutorialUI();
        }
    }

    public void OnSlingshotReleased()
    {
        if (currentState == TutorialState.Shoot)
        {
            currentState = TutorialState.Completed;
            UpdateTutorialUI();
        }
    }

    // --- TEMPORARY HIDE LOGIC ---
    public void HideHintTemporary()
    {
        if (hintPanel != null) hintPanel.SetActive(false);
    }

    public void RestoreHintTemporary()
    {
        if (hintPanel != null && currentState == TutorialState.SelectCard)
        {
            hintPanel.SetActive(true);
        }
    }

    public void HideTutorial()
    {
        if (hintPanel != null) hintPanel.SetActive(false);
        if (tutorialText != null) tutorialText.gameObject.SetActive(false);
    }
}