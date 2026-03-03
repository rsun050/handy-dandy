using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData Data;

    public void PickUp()
    {
        gameObject.SetActive(false); // vanishes
    }

    public void Drop()
    {
        if(Data.CanDrop)
        {
            gameObject.SetActive(true); // vanishes
        }
    }
}
