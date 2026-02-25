using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPickedUpItem_Michael : MonoBehaviour
{
    public static ShowPickedUpItem_Michael Instance { get; private set; }

    [SerializeField] private Transform defaultItemSpawn;
    [SerializeField] private Transform spawnLocation;
    private bool hasItemInHand = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            if(hasItemInHand)
            {
                DropItem();
            }
        }
    }

    public void ShowItem(Transform transformParam)
    {
        transformParam.GetComponent<Rigidbody>().useGravity = false;
        transformParam.parent = spawnLocation;
        transformParam.localPosition = Vector3.zero;
        hasItemInHand = true;
    }

    public void DropItem()
    {
        Transform item = spawnLocation.GetChild(0);
        item.parent = defaultItemSpawn;
        item.GetComponent<Rigidbody>().useGravity = true;
        hasItemInHand = false;
    }
}
