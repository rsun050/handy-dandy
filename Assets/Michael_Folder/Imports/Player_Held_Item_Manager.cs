using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Held_Item_Manager : MonoBehaviour
{
    [SerializeField] private Transform heldItemTransform;
    [SerializeField] private Transform appleCollectionTransform;

    private bool isHoldingAnyItems = false;
    private int amountOfItems = 0;

    public static Player_Held_Item_Manager Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Drop_Held_Item();
        }
    }

    public void Grab_Held_Item(Transform paramTransform)
    {
        paramTransform.GetComponent<Rigidbody>().useGravity = false;
        paramTransform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        paramTransform.parent = heldItemTransform;
        paramTransform.localPosition = Vector3.zero;
        Update_Your_Item_Count();
    }

    public void Drop_Held_Item()
    {
        if(isHoldingAnyItems)
        {
            Transform childTransform = heldItemTransform.GetChild(0);
            childTransform.parent = appleCollectionTransform;
            childTransform.GetComponent<Rigidbody>().useGravity = true;
            childTransform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
            Update_Your_Item_Count();
        }
    }

    public void Update_Your_Item_Count()
    {
        amountOfItems = heldItemTransform.childCount;
        Debug.Log(amountOfItems);

        if(amountOfItems == 0)
        {
            isHoldingAnyItems = false;
        }
        else
        {
            isHoldingAnyItems = true;
        }
    }

    public int Get_Item_Count()
    {
        return amountOfItems;
    }

    public void Turn_In_Held_Items()
    {
        for(int i = amountOfItems - 1; i >= 0; i--)
        {
            Destroy(heldItemTransform.GetChild(i).gameObject);
        }

        Debug.Log("Destoryed items");
        Update_Your_Item_Count();
    }
}
