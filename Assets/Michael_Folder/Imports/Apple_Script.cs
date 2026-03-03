using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple_Script : MonoBehaviour, I_Interactble_Michael
{
    [SerializeField] private Transform myTransform;

    public string GetInteractText()
    {
        return "Pick Up Apple";
    }

    public void OnInteract()
    {
        Debug.Log("Yippie you picked up the Apple");
        Player_Held_Item_Manager.Instance.Grab_Held_Item(myTransform);
    }
}
