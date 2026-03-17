using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_UI_Script : MonoBehaviour
{
    public static Interaction_UI_Script Instance { get; private set; }

    [SerializeField] private GameObject myGameObject;
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private Image progressBar_Image;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Get_UI()
    {
        return myGameObject;
    }

    public TextMeshProUGUI Get_Text()
    {
        return myText;
    }

    public Image Get_Image()
    {
        return progressBar_Image;
    }
}
