using TMPro;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    [SerializeField] private TMP_Text InventoryPrinter;
    [SerializeField] private InventoryManager PlayerInventoryManager;
    public static DebugView Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this); return;
        }
        Instance = this;
    }
    
    public void DebugUpdate() {
        InventoryPrinter.text = PlayerInventoryManager.Print();
    }
}
