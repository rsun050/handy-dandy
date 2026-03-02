using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInventory : MonoBehaviour
{
    public Transform itemSlot;
    public GameObject currentItem;  
    // Start is called before the first frame update
    public void ShowItem(GameObject itemPrefab)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);

            currentItem = Instantiate(itemPrefab, itemSlot);
        }
    }

    // Update is called once per frame
    public void ClearItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }
    }
}
