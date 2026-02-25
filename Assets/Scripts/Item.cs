using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData Data;

    public void PickUp()
    {
        gameObject.SetActive(false); // vanishes
    }
}
