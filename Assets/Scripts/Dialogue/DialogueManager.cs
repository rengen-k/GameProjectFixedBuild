using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;
    public bool dialogueIsPlaying;

    private bool interactPressed;
    private PlayerActionsScript playerActionsScript;


    private void OnEnable()
    {
        InitPlayerInput();
        ConfigPlayerInput();
    }

    private void InitPlayerInput() 
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
    }

    private void ConfigPlayerInput() 
    {
        playerActionsScript.Player.Interact.performed += Interact;
    }

    private void Awake()
    {
        interactPressed = false;
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
        Debug.Log("callEnter");
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
    }

    public void ExitDialogueMode() {
        Debug.Log("callexit");
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

        if (interactPressed) {
            interactPressed = false;
            ContinueStory();
        }
    }

    private void ContinueStory() 
    {
        Debug.Log(currentStory.canContinue);
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("pressed");
        interactPressed = true;
    }
}
