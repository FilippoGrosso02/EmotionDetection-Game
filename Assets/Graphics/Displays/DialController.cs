using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class DialController : MonoBehaviour
{

    // Stress meter:
    public Transform needleTransform;

    public float stress;
    public Vector2 stressRange; // (min, max)
    public Vector2 angleRange; // (min, max) in degrees

    void updateStressNeedle()
    {
        float targetAngle = angleRange.x + angleRange.y; // given in degrees

        float stressNormalized = (stress + stressRange.x) / (stressRange.x + stressRange.y);
        targetAngle = targetAngle * stressNormalized + angleRange.x;


        float angleDelta = targetAngle - needleTransform.eulerAngles.z;

        needleTransform.Rotate(Vector3.forward, angleDelta * Time.deltaTime, Space.Self); // rotation is clockwise
    }

    // Emotion guess meter:
    public float guessConfidence;
    public Vector2 guessConfidenceRange; // (min, max)
    public Vector2 guessConfidenceDisplayRange; // (min, max), the range to which guess confidence is remapped
    public Emotion displayedEmotion;

    public TextMeshPro guessedEmotionText;
    public TextMeshPro confidenceText;

    // Emotion display:
    public BackgroundAnimator backGroundAnimator;

    public void setEmotion(string emotionString)
    {
        Emotion parsedEmotion;

        // Try to parse the string into the Emotion enum
        if (Enum.TryParse(emotionString, true, out parsedEmotion))
        {
            backGroundAnimator.setEmotion(parsedEmotion);
            Debug.Log("Emotion set to: " + parsedEmotion);
        }
        else
        {
            Debug.LogWarning("Invalid emotion string: " + emotionString);
        }
    }
    void updateGuessDisplay()
    {
        float normalizedGuessConfidence = (guessConfidence + guessConfidenceRange.x) / (guessConfidenceRange.x + guessConfidenceRange.y);
        float remappedGuessConfidence = guessConfidence * (guessConfidenceDisplayRange.x + guessConfidenceDisplayRange.y) + guessConfidenceDisplayRange.x;

        confidenceText.text = remappedGuessConfidence.ToString("F1");


        string guessedEmotionString = "NEUTRAL"; // assume "neutral" emotion as the default state
            
        switch(displayedEmotion) 
        {
            case Emotion.Happy:
                guessedEmotionString = "HAPPY";
                break;
            case Emotion.Sad:
                guessedEmotionString = "SAD";
                break;
            default: // neutral emotion
                break;
        }

        guessedEmotionText.text = guessedEmotionString;
    }


    private void Update()
    {
        updateStressNeedle();
        updateGuessDisplay();
    }
}
