using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private List<GameObject> triggeredNPCs = new List<GameObject>();
    private DialogueTrigger[] allNPCObjectsInteract;
    private DialogueTriggerAuto[] allNPCObjects;
    private DialogueTriggerAutoDestroy[] allNPCObjectsDestroy;
    private GameObject[] allNPCs;
    private Story currentStory;
    public bool dialogueIsPlaying;

    private bool talkPressed;
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
        playerActionsScript.Player.Talk.performed += Talk;
    }

    private void Awake()
    {
        allNPCObjectsInteract = FindObjectsOfType<DialogueTrigger>();
        allNPCObjects = FindObjectsOfType<DialogueTriggerAuto>();
        allNPCObjectsDestroy = FindObjectsOfType<DialogueTriggerAutoDestroy>();
        allNPCs = allNPCObjects.Select(s => s.gameObject).ToArray().Concat(allNPCObjectsDestroy.Select(s => s.gameObject)).ToArray().Concat(allNPCObjectsInteract.Select(s => s.gameObject)).ToArray();
        dialoguePanel = GameObject.Find("DialoguePanel");
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();

        
        if (instance != null)
        {
            Debug.LogWarning("More than one DialogueManager found");
        }
        instance = this;
        talkPressed = false;
        
    }

    public static DialogueManager GetInstance() {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    public void EnterDialogueMode(TextAsset inkJSON, GameObject npc) {
        Debug.Log("enter Dialogue");
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    public void ExitDialogueMode() {
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

        if (talkPressed) {
            talkPressed = false;
            ContinueStory();
        }
    }

    public void ContinueStory() 
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    public void Talk(InputAction.CallbackContext context)
    {
        talkPressed = true;
    }

    public bool IsTriggerCalled(GameObject npc)
    {
        
        return triggeredNPCs.Contains(npc);
    }

    public void SetTriggerCalled(GameObject npc)
    {
        triggeredNPCs.Add(npc);
        foreach (var n in allNPCs)
        {
            if (n != npc && n != null && n.GetComponent<DialogueTrigger>() == null) {
                n.SetActive(false);
            }
        }
        Debug.Log(npc.name + "add");
    }

    public void RemoveTriggerCalled(GameObject npc)
    {
        foreach (var n in allNPCs)
        {
            if (n != null) {
                n.SetActive(true);
            }
        }
        if (npc.GetComponent<DialogueTriggerAutoDestroy>() != null) {
            Destroy(npc);
        }
        Debug.Log(npc.name + "remove");
        triggeredNPCs.Remove(npc);
    }

    public bool isDialoguePlaying()
    {
        return dialogueIsPlaying;
    }
}
