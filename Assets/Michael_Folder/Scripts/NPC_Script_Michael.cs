using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script_Michael : MonoBehaviour, I_Interactble_Michael
{
    public string GetInteractText()
    {
        return "Talk to NPC";
    }

    public void OnInteract()
    {
        // show dialouge text here
    }

    public string GetSuccessInteractionText()
    {
        return "Yippie you are talking to the NPC";
    }
}
