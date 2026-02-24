using System.Collections.Generic;
using UnityEngine;

public class PhysicsChestScript_Michael : MonoBehaviour, I_Interactble_Michael
{
    [SerializeField] private Transform spawnLocationTransform;
    private List<string> listOfWeapons = new List<string>
    {
        "Auto Rifle",
        "Hand Cannon",
        "Rocket Launcher",
        "Sword"
    };

    public string GetInteractText()
    {
        return "Open PHYSICS Chest";
    }

    public void OnInteract()
    {
        Debug.Log("Yippie you opened the PHYSICS chest!");

        int randomIndex = Random.Range(0, listOfWeapons.Count);
        string selectedItem = listOfWeapons[randomIndex];

        Engram_Collection_Manager_Michael.Instance.SpawnFromPool("Exotic", spawnLocationTransform.position, selectedItem, true, 80, 10);
    }
}
