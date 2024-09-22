using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum Emotion
{
    Happy,
    Neutral,
    Sad
}


public class BackgroundAnimator : MonoBehaviour
{
    // frames:
    public string neutralToHappyPath;
    public Texture2D[] neutralToHappyFrames;

    public string neutralToSadPath;
    public Texture2D[] neutralToSadFrames;

    public Texture2D[] activeSequence;

    float elapsedTime;
    public float timePerFrame;
    public int currentFrameIndex;

    public Material targetMaterial;

    public bool emotionActive;

    public Emotion currentEmotion;

    bool animationBuffered;
    bool coroutineIsRunning;




    void loadFrames()
    {
        neutralToHappyFrames = Resources.LoadAll<Texture2D>(neutralToHappyPath);
        neutralToSadFrames = Resources.LoadAll<Texture2D>(neutralToSadPath);
    }



    void updateActiveSequence()
    {
        switch (currentEmotion) 
        {
            case Emotion.Happy:
                activeSequence = neutralToHappyFrames;
                break;
            case Emotion.Sad:
                activeSequence = neutralToSadFrames;
                break;
        }
    }

    private IEnumerator runEmotionAnimation(bool reverse) // use reverse to go from emotion to neutral state
    {
        coroutineIsRunning = true;

        int numberOfFrames = activeSequence.Length;

        currentFrameIndex = reverse ? numberOfFrames - 1 : 0;

        while ((currentFrameIndex < numberOfFrames || reverse) && currentFrameIndex >= 0)
        {
            Texture2D currentFrame = activeSequence[currentFrameIndex];

            targetMaterial.SetTexture("_Frame", currentFrame);

            if (!reverse)
                currentFrameIndex += 1; // note that the frame index is updated after the texture is updated
            else
                currentFrameIndex -= 1;

            yield return new WaitForSeconds(timePerFrame);   
        }

        if (reverse)
        {
            emotionActive = false;
        }
        else
        {
            emotionActive = true;
        }
            

        coroutineIsRunning = false;
    }



    // Start is called before the first frame update
    void Start()
    {
        loadFrames();

        targetMaterial.SetTexture("_Frame", neutralToHappyFrames[0]); // initialize to neutral

    }

    public bool debug_runAnimation;
    public bool debug_playReverse;

    public void setEmotion(Emotion emotion) 
    {
        currentEmotion = emotion;
        animationBuffered = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (debug_runAnimation)
        {
            animationBuffered = true;

            
            debug_runAnimation = false;
        }

        if (!coroutineIsRunning && animationBuffered && !emotionActive)
        {
            updateActiveSequence();
            loadFrames();

            if (currentEmotion != Emotion.Neutral)
                StartCoroutine(runEmotionAnimation(emotionActive)); // returns emotion state to neutral

            animationBuffered = false;
        }
        else if (animationBuffered && emotionActive && !coroutineIsRunning)
        {
            StartCoroutine(runEmotionAnimation(emotionActive)); // returns emotion state to neutral
        }
        /*
        if (!coroutineIsRunning && !emotionActive && isNeutral)
        {
            
        }
        */
        

        
       


    }
}
