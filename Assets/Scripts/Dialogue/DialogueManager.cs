using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText; // Reference to a TextMeshPro UI component
    public AudioSource audioSource; // AudioSource component to play the MP3 file
    public EmotionChecker emotionChecker; // Reference to the EmotionChecker script
    public bool isGameStopped = false; // Boolean to stop the game

    private List<Dialogue> dialogueList = new List<Dialogue>(); // List to store dialogue sequence
    private bool canProceed = true; // This ensures you only proceed one dialogue at a time
    private Dialogue currentDialogue; // Store the current dialogue

    private Coroutine emotionCheckCoroutine; // Coroutine reference for emotion checking
    private float postDialogueBufferTime = 3f; // Buffer time after dialogue ends

    void Start()
    {
        dialogueList = new List<Dialogue>();
    }

    void Update()
    {
        // If the game is stopped, do nothing
        if (isGameStopped) return;

        // Check for user input, allow proceeding only when the audio isn't playing
        if (Input.GetKeyDown(KeyCode.Space) && canProceed)
        {
            PlayNextDialogue();
        }
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        // If the game is stopped, don't start dialogue
        if (isGameStopped) return;

        dialogueList.Clear();
        dialogueList.AddRange(dialogues); // Store all dialogues in the list
        PlayNextDialogue(); // Start playing the first dialogue
    }

    public void PlayNextDialogue()
    {
        // If the game is stopped, don't proceed with the next dialogue
        if (isGameStopped) return;

        // Prevent skipping dialogue when audio is still playing
        canProceed = false;

        // Select a random dialogue from the list
        int randomIndex = Random.Range(0, dialogueList.Count);
        currentDialogue = dialogueList[randomIndex];

        dialogueText.text = currentDialogue.text; // Set the dialogue text in the TextMeshPro component

        // Pass the current dialogue to EmotionChecker before playing the audio
        if (emotionChecker != null)
        {
            emotionChecker.currentDialogue = currentDialogue; // Pass the dialogue to EmotionChecker
        }

        if (currentDialogue.audioClip != null)
        {
            audioSource.clip = currentDialogue.audioClip;
            audioSource.Play();
            StartCoroutine(WaitForAudioToFinish(currentDialogue.audioClip.length));
        }
        else
        {
            // If no audio, allow immediate transition
            canProceed = true;
            StartCoroutine(PostDialogueBuffer()); // Start post-dialogue buffer
        }
    }

    // Coroutine to wait for the audio to finish playing before allowing the next dialogue
    private IEnumerator WaitForAudioToFinish(float duration)
    {
        float halfDuration = duration / 2f;

        // Wait until the halfway point of the dialogue
        yield return new WaitForSeconds(halfDuration);

        // Start checking emotions every 0.5 seconds after halfway point
        emotionCheckCoroutine = StartCoroutine(CheckEmotionsPeriodically());

        // Wait for the rest of the audio to finish
        yield return new WaitForSeconds(halfDuration);

        // Stop the emotion checking when the audio finishes
        if (emotionCheckCoroutine != null)
        {
            StopCoroutine(emotionCheckCoroutine);
        }

        StartCoroutine(PostDialogueBuffer()); // Start post-dialogue buffer after audio ends
    }

    // Coroutine to check emotions every 0.5 seconds
    private IEnumerator CheckEmotionsPeriodically()
    {
        while (true)
        {
            CheckEmotions(); // Call the emotion checker
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }

    // Coroutine for the post-dialogue buffer period (where emotion checking continues for 3 seconds)
    private IEnumerator PostDialogueBuffer()
    {
        emotionCheckCoroutine = StartCoroutine(CheckEmotionsPeriodically()); // Keep checking emotions during buffer
        yield return new WaitForSeconds(postDialogueBufferTime); // Wait for 3 seconds

        if (emotionCheckCoroutine != null)
        {
            StopCoroutine(emotionCheckCoroutine); // Stop checking emotions after buffer
        }

        canProceed = true; // Now you can proceed to the next dialogue
        PlayNextDialogue(); // Loop to the next random dialogue
    }

    void EndDialogue()
    {
        dialogueText.text = ""; // Clear text when dialogue ends
        audioSource.Stop(); // Stop the audio when dialogue ends
        canProceed = true; // Reset in case user starts new dialogue session
    }

    // Method to call the EmotionChecker to check emotions
    private void CheckEmotions()
    {
        if (emotionChecker != null)
        {
            emotionChecker.CheckCurrentEmotion(); // Call the method to check the emotion
        }
        else
        {
            Debug.LogWarning("EmotionChecker is not assigned.");
        }
    }

    public void ResetDialogueList()
    {
        dialogueList.Clear(); // Clear the dialogue list
        dialogueText.text = ""; // Clear the dialogue text
        audioSource.Stop(); // Stop any playing audio
        Debug.Log("Dialogue list has been reset.");
    }

    // Method to stop the game (set isGameStopped to true)
    public void StopGame()
    {
        isGameStopped = true;
        audioSource.Stop(); // Optionally stop any playing audio when the game is stopped
        Debug.Log("Game has been stopped.");
    }

    // Method to resume the game (set isGameStopped to false)
    public void ResumeGame()
    {
        isGameStopped = false;
        Debug.Log("Game has resumed.");
    }
}
