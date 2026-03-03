using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup panelGroup;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;

    [Header("Settings")]
    public float typeSpeed = 0.02f;

    private Coroutine typingCoroutine;

    void Awake()
    {
        HideInstant();
    }

    public void ShowDialogue(string npcName, string text, List<string> choices)
    {
        nameText.text = npcName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));

        ClearChoices();
        CreateChoices(choices);

        Show();
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void CreateChoices(List<string> choices)
    {
        foreach (string choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer);
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = choice;

            // Hook up button click later when programmers connect logic
        }
    }

    void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
            Destroy(child.gameObject);
    }

    public void Show()
    {
        StartCoroutine(Fade(1));
    }

    public void Hide()
    {
        StartCoroutine(Fade(0));
    }

    public void HideInstant()
    {
        panelGroup.alpha = 0;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;
    }

    IEnumerator Fade(float target)
    {
        float start = panelGroup.alpha;
        float time = 0;

        while (time < 0.2f)
        {
            panelGroup.alpha = Mathf.Lerp(start, target, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        panelGroup.alpha = target;
        panelGroup.interactable = target == 1;
        panelGroup.blocksRaycasts = target == 1;
    }
}