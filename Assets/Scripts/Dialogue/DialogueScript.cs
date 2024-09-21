using UnityEngine;
using System.Collections.Generic;

public class DialogueScript : MonoBehaviour
{
    [SerializeField]
    public List<Dialogue> dialogues; // Make this list editable in the Inspector

    public DialogueManager dialogueManager;
    // some comment rffrfr
    void Start()
    { 
        BeginDialogue();
    }

    public void BeginDialogue(){
        if (dialogues != null && dialogues.Count > 0)
        {
            // Start the dialogue sequence with the dialogues configured in the Inspector
            dialogueManager.StartDialogue(dialogues);
        }
    }
}
