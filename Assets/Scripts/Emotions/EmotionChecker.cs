using System.Collections;
using UnityEngine;
using TMPro;

public class EmotionChecker : MonoBehaviour
{
    public EmotionManager emotionManager;  // Reference to the EmotionManager
    public Dialogue currentDialogue;       // Reference to the currently playing dialogue
    public DialogueManager dialogueManager;
    public GameObject gameOverPanel;

    // Stress score variables
    public float stressScore = 0f;          // The current stress score
    public float maxStressThreshold = 50f; // The threshold above which the game stops
    public float stressIncreaseAmount = 15f;// How much stress increases for incorrect emotion
    public float stressDecreaseRate = 2f;   // How much stress decreases per second
    public float stressCooldown = 5f;       // Time to wait after the last increase before decreasing

    // Reference to TextMeshProUGUI to display the stress score
    public TextMeshProUGUI stressText;

    public DialController dialController;
    public CanvasGroup stressPanelCanvasGroup; // CanvasGroup for controlling the panel opacity

    private float lastStressIncreaseTime;    // The time of the last stress increase

    private bool hasFailed = false;

    public AudioSource audioSource;

    void Start()
    {
        // Start decreasing the stress score over time
        StartCoroutine(DecreaseStressOverTime());
        UpdateStressText();  // Initialize the text with the current stress score
        lastStressIncreaseTime = -stressCooldown; // Ensure it starts decreasing immediately if stress hasn't been increased yet
    }

    // Method to check if the current emotion matches the dialogue's associated emotions
    public void CheckCurrentEmotion()
    {
        if (emotionManager != null && currentDialogue != null)
        {
            EmotionManager.Emotion currentEmotion = emotionManager.GetCurrentEmotion();

            // Check if the current emotion is part of the dialogue's associated emotions
            if (!IsEmotionInDialogue(currentEmotion))
            {
                Debug.Log("Current emotion is different from the emotions associated with the dialogue.");
                Debug.Log("Associated Emotions: " + string.Join(", ", currentDialogue.associatedEmotions));

                IncreaseStressScore(); // Increase stress when emotion is incorrect

                if (stressScore >= maxStressThreshold && !hasFailed)
                {
                    dialogueManager.Fail();
                    hasFailed = true;
                }
            }
            else
            {
                Debug.Log("Current emotion matches one of the emotions associated with the dialogue.");
            }
        }
    }

    // Method to check if the current emotion is part of the dialogue's associated emotions
    bool IsEmotionInDialogue(EmotionManager.Emotion currentEmotion)
    {
        return currentDialogue.associatedEmotions.Contains(currentEmotion);
    }

    // Method to increase the stress score
    private void IncreaseStressScore()
    {
        stressScore += stressIncreaseAmount;
        lastStressIncreaseTime = Time.time;  // Update the time when stress was last increased
        UpdateStressText();  // Update the stress text whenever the score changes
        UpdateStressPanelOpacity();  // Update the panel's opacity
        Debug.Log("Stress increased. Current stress: " + stressScore);
    }

    // Coroutine to gradually decrease the stress score over time
    IEnumerator DecreaseStressOverTime()
    {
        while (true)
        {
            // Only decrease stress if enough time has passed since the last stress increase
            if (stressScore > 0 && stressScore < 100 && Time.time - lastStressIncreaseTime >= stressCooldown)
            {
                stressScore -= stressDecreaseRate * Time.deltaTime; // Decrease stress over time
                stressScore = Mathf.Max(stressScore, 0);            // Ensure stress doesn't go below 0
                UpdateStressText();                                 // Update the stress text
                UpdateStressPanelOpacity();                         // Update the panel's opacity
                Debug.Log("Stress decreased. Current stress: " + stressScore);
            }

            yield return null; // Wait for next frame
        }
    }

    // Method to update the TextMeshProUGUI text with the current stress score and max score
    private void UpdateStressText()
    {
        dialController.stress = Mathf.Clamp(stressScore, 0f, 100f);
        if (stressText != null)
        {
            stressText.text = $"Stress: {Mathf.Round(stressScore)} / {maxStressThreshold}";
        }
        else
        {
            Debug.LogWarning("Stress text reference is not set.");
        }
    }

    // Method to update the opacity of the stress panel based on the stress score
    private void UpdateStressPanelOpacity()
    {

        if (stressPanelCanvasGroup != null)
        {
            // Normalize the stress score to be between 0 and 1 (for the alpha value)
            float normalizedStress = Mathf.Clamp01(stressScore / maxStressThreshold);
            stressPanelCanvasGroup.alpha = normalizedStress * 0.005f;
            audioSource.volume = 2.5f * normalizedStress;
        }
        else
        {
            Debug.LogWarning("StressPanel CanvasGroup reference is not set.");
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        dialogueManager.StopGame();
        gameOverPanel.SetActive(true);
    }

    
}
