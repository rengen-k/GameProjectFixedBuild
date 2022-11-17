using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMpro;
using ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    private DialogueManager instance;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;
    private bool dialogueIsPlaying;

    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one DialogueManager found");
        }
        instance = this;
        
    }

    public static DialogueManager GetInstance() {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueIsPlaying) {
            return;
        }

        if (InputManager.GetInstance().GetInteractPressed()) {

        }
    }
}
