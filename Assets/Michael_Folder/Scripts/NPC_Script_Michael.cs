using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script_Michael : MonoBehaviour, I_Interactble_Michael
{
    [SerializeField] private List<string> dialougeOptions = new List<string>();
    [SerializeField] private Player_Interaction_Michael playerInteractScript;

    public string GetInteractText()
    {
        return "Talk to NPC";
    }

    public void OnInteract()
    {
        StartCoroutine(MyRoutine());
    }

    IEnumerator MyRoutine()
    {
        int index = 0;
        playerInteractScript.enabled = false;

        while (index < dialougeOptions.Count)
        {
            NPC_Dialouge_Screen_Script.Instance.Show_Screen();
            NPC_Dialouge_Screen_Script.Instance.Set_Text(dialougeOptions[index]);
            yield return new WaitForSeconds(3f);
            NPC_Dialouge_Screen_Script.Instance.Hide_Screen();
            index++;
        }

        playerInteractScript.enabled = true;
    }
}
