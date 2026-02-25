using System.Collections.Generic;
using UnityEngine;

public class Chest_Script_Michael : MonoBehaviour, I_Interactble_Michael
{
    [SerializeField] private Transform spawnLocationTransform;

    private List<string> listOfWeapons = new List<string>
    {
        "Auto Rifle",
        "Hand Cannon",
    };

    public string GetInteractText()
    {
        return "Open Chest";
    }

    public void OnInteract()
    {
        Debug.Log("Yippie you opened the chest!");

        int randomIndex = Random.Range(0, listOfWeapons.Count);
        string selectedItem = listOfWeapons[randomIndex];

        Engram_Collection_Manager_Michael.Instance.SpawnFromPool("Legendary", spawnLocationTransform.position, selectedItem, false);
    }
}
