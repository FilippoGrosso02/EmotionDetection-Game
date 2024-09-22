using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // The audio source that will play the music
    public AudioClip menuTrack;     // The audio clip for the menu music
    public AudioClip gameTrack;     // The audio clip for the game music

    // Start is called before the first frame update
    void Start()
    {
        // Initially play the menu track
        PlayMenuTrack();
    }

    // Method to play the menu track
    public void PlayMenuTrack()
    {
        if (audioSource.clip != menuTrack)
        {
            audioSource.clip = menuTrack;
            audioSource.Play();
        }
    }

    // Method to play the game track
    public void PlayGameTrack()
    {
        if (audioSource.clip != gameTrack)
        {
            audioSource.clip = gameTrack;
            audioSource.Play();
        }
    }

    // Example method to swap based on the game state
    public void SwapTrack(bool isInGame)
    {
        if (isInGame)
        {
            PlayGameTrack();
        }
        else
        {
            PlayMenuTrack();
        }
    }
}
