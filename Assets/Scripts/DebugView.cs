using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    [SerializeField] private TMP_Text InventoryPrinter;
    [SerializeField] private InventoryManager PlayerInventoryManager;

    // Update is called once per frame
    void Update() {
        InventoryPrinter.text = PlayerInventoryManager.Print();
    }
}
