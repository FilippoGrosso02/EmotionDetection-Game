using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required to work with TextMeshPro

public class PointManager : MonoBehaviour
{
    public TMP_Text pointsText; // Reference to TextMeshPro for current points
    public TMP_Text highScoreText; // Reference to TextMeshPro for high score
    private int currentPoints = 0; // Variable to store current points
    private int highScore = 0; // Variable to store high score

    void Start()
    {
        // Load the high score from PlayerPrefs (default to 0 if not set)
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdatePointsText(); // Initialize the points and high score text
        UpdateHighScoreText(); // Initialize the high score text
    }

    // Function to add points
    public void AddPoint(int points)
    {
        currentPoints += points;
        UpdatePointsText(); // Update points text whenever points are added

        // If the current score exceeds the high score, update the high score
        if (currentPoints > highScore)
        {
            highScore = currentPoints;
            PlayerPrefs.SetInt("HighScore", highScore); // Save the new high score
            UpdateHighScoreText(); // Update the high score text
        }
    }

    // Function to update the TMP text component with the current points
    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "Points: " + currentPoints.ToString();
        }
        else
        {
            Debug.LogWarning("PointsText reference is not set in the inspector.");
        }
    }

    // Function to update the TMP text component with the high score
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
        else
        {
            Debug.LogWarning("HighScoreText reference is not set in the inspector.");
        }
    }

    // Optional: Function to reset high score for testing
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScore = 0;
        UpdateHighScoreText();
    }
}
