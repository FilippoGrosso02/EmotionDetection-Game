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

    public List<Dialogue> openingDialogues; // List for opening dialogues
    public List<Dialogue> mainDialogues; // List for main dialogues
    public List<Dialogue> halfwayDialogues; // List for halfway dialogues
    public List<Dialogue> failDialogues; // List for fail dialogues

    private bool canProceed = true; // This ensures you only proceed one dialogue at a time
    private Dialogue currentDialogue; // Store the current dialogue
    private Coroutine emotionCheckCoroutine; // Coroutine reference for emotion checking
    public float postDialogueBufferTime = 3f; // Buffer time after dialogue ends
    private int dialogueStep = 0; // To track the step in the sequence

    public PointManager pointManager;

    public DialController dialController;

    void Start()
    {

    }
    
    public void StartDialogue(){
        PlayNextDialogue();
    }
    void Update()
    {
        // If the game is stopped, do nothing
        if (isGameStopped) return;

        // Check for user input, allow proceeding only when the audio isn't playing
        if (Input.GetKeyDown(KeyCode.Space) && canProceed)
        {
            //PlayNextDialogue();
        }
    }

    // Plays the next dialogue in the sequence (Opening -> Main -> Halfway -> Main)
    public void PlayNextDialogue()
    {
        // If the game is stopped, don't proceed with the next dialogue
        if (isGameStopped) return;

        // Prevent skipping dialogue when audio is still playing
        canProceed = false;

        switch (dialogueStep)
        {
            case 0:
                // Pick a dialogue from the opening dialogues
                currentDialogue = PickRandomDialogue(openingDialogues);
                break;
            case 1:
            case 3:
                // Pick a dialogue from the main dialogues (check emotions here)
                currentDialogue = PickRandomDialogue(mainDialogues);
                
                // Set emotion using the string version of the emotion
                dialController.setEmotion(currentDialogue.associatedEmotions[0].ToString());
                
                break;

            case 2:
                // Pick a dialogue from the halfway dialogues
                currentDialogue = PickRandomDialogue(halfwayDialogues);
                break;
        }

        emotionChecker.currentDialogue = currentDialogue;
        dialogueText.text = currentDialogue.text; // Set the dialogue text in the TextMeshPro component

        if (currentDialogue.audioClip != null)
        {
            audioSource.clip = currentDialogue.audioClip;
            audioSource.Play();
            Debug.Log("Start coroutine");
            StartCoroutine(WaitForAudioToFinish(currentDialogue.audioClip.length));
        }
        else
        {
            // If no audio, allow immediate transition
            canProceed = true;
            StartCoroutine(PostDialogueBuffer()); // Start post-dialogue buffer
        }

        // Update dialogue step for the sequence
// Sequence resets after main dialogues
    }

    // Method to randomly pick a dialogue from the list
    private Dialogue PickRandomDialogue(List<Dialogue> dialogues)
    {
        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogWarning("Dialogue list is empty!");
            return null;
        }

        int randomIndex = Random.Range(0, dialogues.Count);
        return dialogues[randomIndex];
    }

    // Coroutine to wait for the audio to finish playing before allowing the next dialogue
    private IEnumerator WaitForAudioToFinish(float duration)
    {
        Debug.Log("Start coroutine");
        float halfDuration = duration / 2f;

        // Wait until the halfway point of the dialogue
        yield return new WaitForSeconds(2f);

        // Start checking emotions only during main dialogues
        if (dialogueStep == 1 || dialogueStep == 3)
        {
            emotionCheckCoroutine = StartCoroutine(CheckEmotionsPeriodically());
            Debug.Log("Start emotion chjecking");
        }

        // Wait for the rest of the audio to finish
        yield return new WaitForSeconds(halfDuration  -2f);


        StartCoroutine(PostDialogueBuffer()); 
        

    }

    // Coroutine to check emotions every 0.5 seconds
    private IEnumerator CheckEmotionsPeriodically()
    {
        while (true)
        {
            CheckEmotions(); // Call the emotion checker
            yield return new WaitForSeconds(0.1f); // Check every 0.5 seconds
        }
    }

    // Coroutine for the post-dialogue buffer period (where emotion checking continues for 3 seconds)
    private IEnumerator PostDialogueBuffer()
    {
        if (dialogueStep == 0) dialogueStep = 1;
        else if (dialogueStep == 1){ 
            dialogueStep = 2;
            pointManager.AddPoint(1);
        }

        else if (dialogueStep == 2) dialogueStep = 1; // Start post-dialogue buffer after audio ends

        yield return new WaitForSeconds(postDialogueBufferTime); // Wait for 3 seconds

        if (emotionCheckCoroutine != null)
        {
            StopCoroutine(emotionCheckCoroutine); // Stop checking emotions after buffer
        }

        canProceed = true; // Now you can proceed to the next dialogue
        
        PlayNextDialogue(); // Loop to the next dialogue in the sequence
    }

    // Method to call the EmotionChecker to check emotions
    private void CheckEmotions()
    {
        if (emotionChecker != null)
        {
            emotionChecker.CheckCurrentEmotion();
            

        }
        else
        {
            Debug.LogWarning("EmotionChecker is not assigned.");
        }
    }

    // Play fail dialogue and then stop the game
    public void Fail(){
        StartCoroutine(PlayFailDialogueAndStopGame());
    }
    private IEnumerator PlayFailDialogueAndStopGame()
    {
        Debug.Log("Fail dialogue");
        isGameStopped = true;
        // Stop any currently playing audio or dialogue
        audioSource.Stop();
        //StopAllCoroutines(); // Stop all ongoing coroutines (like PostDialogueBuffer)

        // Pick a fail dialogue
        Dialogue failDialogue = PickRandomDialogue(failDialogues);

        yield return new WaitForSeconds(2f);

        emotionChecker.GameOver();

        if (failDialogue != null)
        {
            // Show the fail dialogue text
            dialogueText.text = failDialogue.text;

            if (failDialogue.audioClip != null)
            {
                // Play the fail dialogue audio
                audioSource.clip = failDialogue.audioClip;
                audioSource.Play();
                yield return new WaitForSeconds(failDialogue.audioClip.length + 2f);
            }
            else
            {
                // If no audio, wait for a few seconds
                yield return new WaitForSeconds(3f); 
            }
        }

        // After playing fail dialogue, stop the game
        //emotionChecker.GameOver();
        StopAllCoroutines();
    }

    public void StopGame()
    {
        isGameStopped = true;
        audioSource.Stop(); // Stop any playing audio
        Debug.Log("Game has been stopped.");
    }

    public void ResumeGame()
    {
        isGameStopped = false;
        Debug.Log("Game has resumed.");
    }
}
