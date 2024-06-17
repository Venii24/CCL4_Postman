using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] public GameObject FKeyAlert;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage characterImage;
    
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private bool isTyping = false;
    private string currentLine = "";

    private static DialogueManager instance;
    private Timer timer;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager found!");
            return;
        }
        instance = this;
        FKeyAlert.SetActive(false);
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        timer = FindObjectOfType<Timer>();
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLine;
                isTyping = false;
            }
            else
            {
                ContinueStory();
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, Texture characterImageTexture)
    {
        nameText.text = inkJSON.name;
        characterImage.texture = characterImageTexture;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        timer.stopTimer = true;
        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        timer.stopTimer = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue) 
        {
            currentLine = currentStory.Continue();
            StartCoroutine(TypeSentence(currentLine));
        }
        else 
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); //speed of typing
        }
        isTyping = false;
    }
}