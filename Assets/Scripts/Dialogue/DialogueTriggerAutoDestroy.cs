using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTriggerAutoDestroy : MonoBehaviour
{
    private GameObject visualCue;
    private PlayerActionsScript playerActionsScript;
    private bool talkPressed;
    [SerializeField] private TextAsset inkJSON;
    private bool triggerCalled = false;

    private bool playerInRange;

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
        // visualCue = GameObject.Find("NPC/Canvas/DialogueVisual");
        playerInRange = false;
    }

    private void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying) {
            playerActionsScript.Player.Disable();
        }

        if (playerInRange)
        {
            if (DialogueManager.GetInstance().IsTriggerCalled(gameObject)) {
                if (!DialogueManager.GetInstance().dialogueIsPlaying) {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, gameObject);
                }
            }
        }
        else
        {
            DialogueManager.GetInstance().ExitDialogueMode();
        }

        if (!DialogueManager.GetInstance().dialogueIsPlaying) {

            DialogueManager.GetInstance().ExitDialogueMode();
            StartCoroutine(DialogueCooldown());
        }
    }

    public void Talk(InputAction.CallbackContext context)
    {
        if (playerInRange) {
            talkPressed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DialogueManager.GetInstance().IsTriggerCalled(gameObject)) {
            return;
        }
        DialogueManager.GetInstance().SetTriggerCalled(gameObject);
        if (other.gameObject.name == "Player" && DialogueManager.GetInstance().IsTriggerCalled(gameObject))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {   
        if (!DialogueManager.GetInstance().IsTriggerCalled(gameObject)) {
            return;
        }
        DialogueManager.GetInstance().RemoveTriggerCalled(gameObject);
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
    
    private IEnumerator DialogueCooldown()
    {
        playerActionsScript.Player.Disable();
        yield return new WaitForSeconds(0.1f);
        playerActionsScript.Player.Enable();
    }
    
}
