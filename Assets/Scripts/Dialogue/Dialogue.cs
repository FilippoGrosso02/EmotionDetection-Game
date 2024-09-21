using System.Collections.Generic; // Required for using lists
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int id;
    public string text;
    public AudioClip audioClip; // Use Unity's built-in AudioClip for the mp3 files
    public List<EmotionManager.Emotion> associatedEmotions; // List of associated emotions

    // Constructor
    public Dialogue(int id, string text, AudioClip audioClip, List<EmotionManager.Emotion> associatedEmotions)
    {
        this.id = id;
        this.text = text;
        this.audioClip = audioClip;
        this.associatedEmotions = associatedEmotions;
    }
}
