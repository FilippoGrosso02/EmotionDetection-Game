using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public EmotionFetcher emotionFetcher;

    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (emotionManager.GetCurrentEmotion() == EmotionManager.Emotion.Happy) StartGame();
        if (emotionFetcher.happiness == 100)StartGame();
    }

    public void StartGame(){
        dialogueManager.StartDialogue();
        gameObject.SetActive(false);
        audioManager.PlayGameTrack();
    }
}
