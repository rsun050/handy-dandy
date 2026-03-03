using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Dialouge_Screen_Script : MonoBehaviour
{
    public static NPC_Dialouge_Screen_Script Instance { get; private set; }

    [SerializeField] private CanvasGroup myCanvasGroup;
    [SerializeField] private TextMeshProUGUI textRef;

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
    }

    public void Set_Text(string paramString)
    {
        textRef.text = paramString;
    }
}
