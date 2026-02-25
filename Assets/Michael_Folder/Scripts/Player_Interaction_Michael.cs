using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Player_Interaction_Michael : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    public Image progressCircle;

    private float holdTimer = 0f;
    private float holdDuration = 3f;

    private I_Interactble_Michael currentTarget;

    [SerializeField] private TextMeshProUGUI successInteractionText;
    private Coroutine timerCoroutine;

    void Update()
    {
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            I_Interactble_Michael interactable = hit.collider.GetComponent<I_Interactble_Michael>();

            if (interactable != null)
            {
                if (interactable != currentTarget)
                {
                    Debug.Log("looking at interactble right now");
                    OnNewObjectFound(interactable);
                }

                HandleInteractionInput(interactable);
                return;
            }
        }

        if (currentTarget != null)
        {
            ResetInteraction();
        }
    }

    private void HandleInteractionInput(I_Interactble_Michael interactable)
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;
            progressCircle.fillAmount = holdTimer / holdDuration;

            if (holdTimer >= holdDuration)
            {
                Update_SuccessInteractionText(interactable.GetSuccessInteractionText());
                interactable.OnInteract();
                ResetInteraction();
            }
        }
        else
        {
            holdTimer = 0f;
            progressCircle.fillAmount = 0f;
        }
    }

    private void ResetInteraction()
    {
        currentTarget = null;
        holdTimer = 0f;

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        if (progressCircle != null)
        {
            progressCircle.fillAmount = 0f;
        }
    }

    private void OnNewObjectFound(I_Interactble_Michael newTarget)
    {
        currentTarget = newTarget;
        interactionUI.SetActive(true);
        interactionText.text = "Hold E to " + newTarget.GetInteractText();

        holdTimer = 0f;
        progressCircle.fillAmount = 0f;
    }

    private void Update_SuccessInteractionText(string textParam)
    {
        successInteractionText.text = textParam;
        successInteractionText.gameObject.SetActive(true);
        StartMyTimer();
    }

    public void StartMyTimer()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(TimerRoutine(2));
    }

    IEnumerator TimerRoutine(int seconds)
    {
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1f);
            seconds--;
        }
        Debug.Log("Timer Finished!");
        successInteractionText.text = null;
        successInteractionText.gameObject.SetActive(false);
    }
}
