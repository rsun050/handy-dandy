using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith_Script : MonoBehaviour, I_Interactble_Michael
{
    [SerializeField] private List<string> dialougeOptions = new List<string>();
    [SerializeField] private string endDialougeText;
    [SerializeField] private NPC_Blacksmith_Anim_Script animScript;
    [SerializeField] private Player_Interaction_Michael playerInteractScript;

    public string GetInteractText()
    {
        return "Talk with Blacksmith";
    }

    public void OnInteract()
    {
        Debug.Log("Yippie you talked to the Blacksmith!");
        animScript.Enable_Idle_Animation();

        if (Player_Held_Item_Manager.Instance.Get_Item_Count() == 3)
        {
            StartCoroutine(MyRoutine_2());
        }
        else
        {
            StartCoroutine(MyRoutine());
        }
    }

    IEnumerator MyRoutine_2()
    {
        NPC_Dialouge_Screen_Script.Instance.Show_Screen();
        NPC_Dialouge_Screen_Script.Instance.Set_Text("You have exactly enough apples for me. THANK YOU");
        Player_Held_Item_Manager.Instance.Turn_In_Held_Items();
        playerInteractScript.enabled = false;
        yield return new WaitForSeconds(3f);
        NPC_Dialouge_Screen_Script.Instance.Hide_Screen();

        animScript.Enable_Smith_Animation();
        playerInteractScript.enabled = true;
    }


    IEnumerator MyRoutine()
    {
        int index = 0;
        playerInteractScript.enabled = false;

        while (index < dialougeOptions.Count)
        {
            NPC_Dialouge_Screen_Script.Instance.Show_Screen();
            NPC_Dialouge_Screen_Script.Instance.Set_Text(dialougeOptions[index]);
            yield return new WaitForSeconds(4f);
            NPC_Dialouge_Screen_Script.Instance.Hide_Screen();
            index++;
        }

        animScript.Enable_Wave_Animation();

        NPC_Dialouge_Screen_Script.Instance.Show_Screen();
        NPC_Dialouge_Screen_Script.Instance.Set_Text(endDialougeText);
        yield return new WaitForSeconds(3f);
        NPC_Dialouge_Screen_Script.Instance.Hide_Screen();

        animScript.Enable_Smith_Animation();
        playerInteractScript.enabled = true;
    }
}
