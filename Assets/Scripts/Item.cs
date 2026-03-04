using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData Data;

    public void PickUp()
    {
        gameObject.SetActive(false); // vanishes
    }

    public void Drop(Vector3 atPosition) {
        transform.position = atPosition;

        if(Data.CanDrop) {
            gameObject.SetActive(true); // reappears
        }
    }
}
