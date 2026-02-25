using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple_Script_Michael : MonoBehaviour, I_Interactble_Michael
{
    public string GetInteractText()
    {
        return "Pick Up Apple";
    }

    public void OnInteract()
    {
        ShowPickedUpItem_Michael.Instance.ShowItem(transform);
    }

    public string GetSuccessInteractionText()
    {
        return "Yippie you picked up the apple!";
    }
}
