using UnityEngine;

public class PlayerTriggerDetection_Michael : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Engram_Prefab_Data_Michael myEngramData = other.GetComponent<Engram_Prefab_Data_Michael>();

        if (myEngramData)
        {
            Debug.Log("Item detected");
            Engram_Collection_Manager_Michael.Instance.ReturnToPool(other.gameObject);
        }

        //switch(other.gameObject.tag)
        //{
        //    case "NPC_Male":
        //        Debug.Log("Interacted with male");
        //        break;

        //    case "NPC_Female":
        //        Debug.Log("Interacted with female");
        //        break;

        //    case "NPC_Blacksmith":
        //        Debug.Log("Interacted with blacksmith");
        //        other.GetComponent<Blacksmith_Script>().
        //        break;
        //}

    }
}
