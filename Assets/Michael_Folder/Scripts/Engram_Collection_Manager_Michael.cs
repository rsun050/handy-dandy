using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Engram_Collection_Manager_Michael;

public class Engram_Collection_Manager_Michael : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static Engram_Collection_Manager_Michael Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool item in pools)
        {
            GameObject container = new GameObject(item.tag + " Pool");
            container.transform.SetParent(this.transform);
            container.transform.localPosition = Vector3.zero;

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(container.transform);
                obj.transform.localPosition = Vector3.zero;
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.tag, objectPool);
        }
    }

    public void SpawnFromPool(string tag, Vector3 position, string weaponName, bool EnablePhysics, int angle = 0, int launchForce = 0)
    {
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.GetComponent<Engram_Prefab_Data_Michael>().Set_WeaponName(weaponName);
        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);

        if(EnablePhysics)
        {
            Launcher_Manager_Michael.Instance.LaunchCube(angle, objectToSpawn.GetComponent<Rigidbody>(), launchForce);
        }
    }

    public void ReturnToPool(GameObject incomingEngram)
    {
        string currentWeaponName = incomingEngram.GetComponent<Engram_Prefab_Data_Michael>().Get_WeaponName();
        Debug.Log("You picked up " + currentWeaponName);
        incomingEngram.SetActive(false);
        incomingEngram.transform.localPosition = Vector3.zero;
        incomingEngram.GetComponent<Engram_Prefab_Data_Michael>().Set_WeaponName(null);
    }
}
