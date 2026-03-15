using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData Data;

    public void PickUp()
    {
        gameObject.SetActive(false); // vanishes
    }

    // drop at pos with starting velocity, or destroy the gameobject outright
    public void Drop(Vector3 atPosition, bool destroy, Vector3? velocity = null) {
        if(Data.CanDrop) {
            if(destroy) {
                Destroy(this.gameObject);
            } else {
                transform.position = atPosition;
                gameObject.SetActive(true); // reappears                
            }
        }
    }
}
