using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engram_Prefab_Data_Michael : MonoBehaviour
{
    private string weaponName;
    
    public void Set_WeaponName(string weaponName)
    {
        this.weaponName = weaponName;
    }

    public string Get_WeaponName()
    {
        return weaponName;
    }
}
