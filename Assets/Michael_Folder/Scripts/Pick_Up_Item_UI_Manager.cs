using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pick_Up_Item_UI_Manager : MonoBehaviour
{
    public static Pick_Up_Item_UI_Manager Instance {  get; private set; }

    [SerializeField] private CanvasGroup myCanvasGroup;
    [SerializeField] private TextMeshProUGUI myText;

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

    public void SetText(string paramText)
    {
        myText.text = paramText;
    }

    public void Start_Timer()
    {
        StartCoroutine(MyRoutine());
    }

    IEnumerator MyRoutine()
    {
        yield return new WaitForSeconds(4f);
        Hide_Screen();
        SetText(null);
    }
}
