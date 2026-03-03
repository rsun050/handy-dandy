using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public DialogueUI dialogueUI;

    void Start()
    {
        dialogueUI.ShowDialogue(
            "NPC1",
            "Hello! Can you bring me 3 apples?",
            new List<string> { "Sure!", "No thanks." }
        );
    }
}
