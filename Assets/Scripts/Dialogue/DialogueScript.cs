/*using UnityEngine;
using System.Collections.Generic;

public class DialogueScript : MonoBehaviour
{
    [SerializeField]
    public List<Dialogue> openingDialogues; // List for opening dialogues
    [SerializeField]
    public List<Dialogue> halfwayDialogues; // List for halfway dialogues
    [SerializeField]
    public List<Dialogue> failDialogues; // List for fail dialogues
    [SerializeField]
    public List<Dialogue> mainDialogues; // Main dialogue list

    public DialogueManager dialogueManager;

    void Start()
    {
        //BeginOpeningDialogue();
    }

    // Start the opening dialogue sequence
    public void BeginOpeningDialogue()
    {
        if (openingDialogues != null && openingDialogues.Count > 0)
        {
            dialogueManager.StartDialogue(openingDialogues);
        }
        else
        {
            // If no opening dialogue, start the main dialogue
            BeginMainDialogue();
        }
    }

    // Start the main dialogue sequence
    public void BeginMainDialogue()
    {
        if (mainDialogues != null && mainDialogues.Count > 0)
        {
            dialogueManager.StartDialogue(mainDialogues);
        }
    }

    // Start the halfway dialogue sequence
    public void BeginHalfwayDialogue()
    {
        if (halfwayDialogues != null && halfwayDialogues.Count > 0)
        {
            dialogueManager.StartDialogue(halfwayDialogues);
        }
    }

    // Start the fail dialogue sequence
    public void BeginFailDialogue()
    {
        if (failDialogues != null && failDialogues.Count > 0)
        {
            dialogueManager.StartDialogue(failDialogues);
        }
    }
}
*/