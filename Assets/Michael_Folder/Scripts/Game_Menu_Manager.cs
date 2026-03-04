using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game_Menu_Manager : MonoBehaviour
{
    public static Game_Menu_Manager Instance { get; private set; }

    [SerializeField] private CanvasGroup myCanvasGroup;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private Mouse_Look_Michael mouseLook_Script;
    [SerializeField] private Button applyChangesButton;
    [SerializeField] private Slider mySlider;
    [SerializeField] private CanvasGroup openScreenText;
    [SerializeField] private CanvasGroup confirmationTextCanvasGroup;

    private bool myToggle = false;
    private bool isConfirmationTextShowing = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle_Game_Menu();
        }
    }

    public void Toggle_Game_Menu()
    {
        myToggle = !myToggle;

        if(myToggle)
        {
            Show_Game_Menu();
        }
        else
        {
            Hide_Game_Menu();
        }
    }

    private void Show_Game_Menu()
    {
        myCanvasGroup.alpha = 1;
        myCanvasGroup.blocksRaycasts = true;
        myCanvasGroup.interactable = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseLook_Script.Enable_Mouse_Look(false);

        openScreenText.alpha = 0;
        openScreenText.blocksRaycasts = false;
        openScreenText.interactable = false;
    }

    private void Hide_Game_Menu()
    {
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;

        Cursor.lockState = CursorLockMode.Locked;
        mouseLook_Script.Enable_Mouse_Look(true);

        openScreenText.alpha = 1;
        openScreenText.blocksRaycasts = true;
        openScreenText.interactable = true;
    }

    public void SliderChange(float value)
    {
        sliderText.text = value.ToString();
    }

    public void Apply_Changes_Button()
    {
        mouseLook_Script.Set_Mouse_Sensitivity(mySlider.value * 10);

        if(isConfirmationTextShowing == false)
        {
            StartCoroutine(Confirmation_Text_Timer());
        }
        else
        {
            Debug.Log("Confirmation text already showing");
        }
    }

    IEnumerator Confirmation_Text_Timer()
    {
        isConfirmationTextShowing = true;
        confirmationTextCanvasGroup.alpha = 1;
        confirmationTextCanvasGroup.interactable = true;
        confirmationTextCanvasGroup.blocksRaycasts = true;

        yield return new WaitForSeconds(3f);

        confirmationTextCanvasGroup.alpha = 0;
        confirmationTextCanvasGroup.interactable = false;
        confirmationTextCanvasGroup.blocksRaycasts = false;
        isConfirmationTextShowing = false;
    }
}
