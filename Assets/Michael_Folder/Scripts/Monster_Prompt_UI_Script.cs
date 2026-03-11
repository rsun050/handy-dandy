using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Prompt_UI_Script : MonoBehaviour
{
    public static Monster_Prompt_UI_Script Instance { get; private set; }  

    [SerializeField] private CanvasGroup myCanvasGroup;
    [SerializeField] private TextMeshProUGUI textRef;

    private bool isWithinDistance = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && isWithinDistance)
        {
            string nameOfItem = NPC_Dialouge_Screen_Script.Instance.Get_Name_of_Item();
            switch (nameOfItem)
            {
                case "Sword":
                    Debug.Log("Teleporting you to fight the monster...");
                    Load_Level(1);
                    break;

                case "Food":
                    Debug.Log("You have put the monster to sleep...");
                    break;
            }
        }
    }

    public void Show_Screen()
    {
        myCanvasGroup.alpha = 1;
        myCanvasGroup.blocksRaycasts = true;
        myCanvasGroup.interactable = true;

        isWithinDistance = true;
    }

    public void Hide_Screen()
    {
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;

        isWithinDistance = false;
    }

    public void Set_Text(string paramText)
    {
        textRef.text = paramText;
    }

    public void Load_Level(int sceneIndex)
    {
        StartCoroutine(My_Timer(sceneIndex));
    }

    IEnumerator My_Timer(int sceneIndex)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync(sceneIndex);
    }

}
