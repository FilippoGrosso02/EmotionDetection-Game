using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmotionManager : MonoBehaviour
{
    // Enum for readability
    public enum Emotion
    {
        Happy = 0,
        Neutral = 1,
        Sad = 2
    }

    // Store the current emotion
    private Emotion currentEmotion = Emotion.Neutral;

    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI emotionText;

    // Reference to the EmotionFetcher
    public EmotionFetcher emotionFetcher;

    // Buffers for averaging emotion values over time
    private List<float> happinessBuffer = new List<float>();
    private List<float> neutralBuffer = new List<float>();
    private List<float> sadnessBuffer = new List<float>();

    // Settings for the averaging window
    public int averageWindowSize = 5; // Number of frames to average over

    void Start()
    {
        // Initialize the emotion text at the start
        UpdateEmotionText();
    }

    void Update()
    {
        // Get the prevalent emotion from EmotionFetcher, average it, and update
        UpdateEmotionFromFetcher();
        SetEmotionBasedOnAverage();
    }

    // Method to set the current emotion based on the highest averaged emotion from EmotionFetcher
    private void UpdateEmotionFromFetcher()
    {
        if (emotionFetcher != null)
        {
            // Get emotion values from the EmotionFetcher
            float happiness = emotionFetcher.happiness;
            float neutral = emotionFetcher.neutral;
            float sadness = emotionFetcher.sadness + emotionFetcher.angry;

            // Add values to the buffers
            AddToBuffer(happinessBuffer, happiness);
            AddToBuffer(neutralBuffer, neutral);
            AddToBuffer(sadnessBuffer, sadness);
        }
        else
        {
            Debug.LogWarning("EmotionFetcher is not assigned.");
        }
    }

    // Add a new emotion value to the buffer, maintaining the window size
    private void AddToBuffer(List<float> buffer, float newValue)
    {
        buffer.Add(newValue);
        if (buffer.Count > averageWindowSize)
        {
            buffer.RemoveAt(0);  // Keep the buffer size within the set window
        }
    }

    // Calculate the average of a list of float values
    private float CalculateAverage(List<float> buffer)
    {
        float sum = 0;
        foreach (float value in buffer)
        {
            sum += value;
        }
        return buffer.Count > 0 ? sum / buffer.Count : 0;
    }

    // Set the current emotion based on the highest average value from the buffers
    private void SetEmotionBasedOnAverage()
    {
        float avgHappiness = CalculateAverage(happinessBuffer);
        float avgNeutral = CalculateAverage(neutralBuffer);
        float avgSadness = CalculateAverage(sadnessBuffer);

        // Determine the prevalent averaged emotion
        if (avgHappiness > avgNeutral && avgHappiness > avgSadness)
        {
            SetCurrentEmotion("Happy");
        }
        else if (avgNeutral > avgHappiness && avgNeutral > avgSadness)
        {
            SetCurrentEmotion("Neutral");
        }
        else
        {
            SetCurrentEmotion("Sad");
        }
    }

    // Method to set the current emotion based on a string input
    public void SetCurrentEmotion(string emotionName)
    {
        if (System.Enum.TryParse(emotionName, true, out Emotion parsedEmotion))
        {
            currentEmotion = parsedEmotion;
            UpdateEmotionText();
            Debug.Log($"Emotion switched to: {currentEmotion}");
        }
        else
        {
            Debug.LogWarning($"Invalid emotion: {emotionName}");
        }
    }

    // Update the UI with the current emotion
    private void UpdateEmotionText()
    {
        if (emotionText != null)
        {
            emotionText.text = $"Current: {currentEmotion}";
        }
        else
        {
            Debug.LogWarning("Emotion text reference is not set.");
        }
    }

    public Emotion GetCurrentEmotion()
    {
        return currentEmotion;
    }
}
