using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to reload the scene

public class GameManager : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to the DialogueManager

    // Start is called before the first frame update
    void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Press 'R' to reload the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadGame();
        }
    }

    // Function to reload the game and reset the dialogue list
    public void ReloadGame()
    {
        // Reset the dialogue list
        if (dialogueManager != null)
        {
            //dialogueManager.ResetDialogueList();
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
