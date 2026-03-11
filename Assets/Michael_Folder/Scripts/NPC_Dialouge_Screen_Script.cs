using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Dialouge_Screen_Script : MonoBehaviour
{
    public static NPC_Dialouge_Screen_Script Instance { get; private set; }

    [SerializeField] private CanvasGroup myCanvasGroup;
    [SerializeField] private TextMeshProUGUI textRef;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    [SerializeField] private Blacksmith_Script blacksmithScript;

    private string nameOfItem = "Empty";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide_Screen();
    }

    public void Show_Screen()
    {
        myCanvasGroup.alpha = 1;
        myCanvasGroup.blocksRaycasts = true;
        myCanvasGroup.interactable = true;
    }

    public void Hide_Screen()
    {
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;

        Hide_Dialouge_Options();
    }

    public void Show_Dialouge_Options()
    {
        buttonsCanvasGroup.alpha = 1;
        buttonsCanvasGroup.blocksRaycasts = true;
        buttonsCanvasGroup.interactable = true;

        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Hide_Dialouge_Options()
    {
        buttonsCanvasGroup.alpha = 0;
        buttonsCanvasGroup.blocksRaycasts = false;
        buttonsCanvasGroup.interactable = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void On_Sword_Button_Clicked()
    {
        nameOfItem = "Sword";
        blacksmithScript.Reset_Blacksmith();
        Monster_Prompt_UI_Script.Instance.Set_Text("You have the Sword");
    }
    public void On_Food_Button_Clicked()
    {
        nameOfItem = "Food";
        blacksmithScript.Reset_Blacksmith();
        Monster_Prompt_UI_Script.Instance.Set_Text("You have the Food");
    }

    public void Set_Text(string paramString)
    {
        textRef.text = paramString;
    }

    public string Get_Name_of_Item()
    {
        return nameOfItem;
    }
}
