using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTriggerAuto : MonoBehaviour
{
    private GameObject visualCue;
    private PlayerActionsScript playerActionsScript;
    private bool talkPressed;
    [SerializeField] private TextAsset inkJSON;
    private bool triggerCalled = false;

    // int instanceID = gameObject.GetInstanceID();

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
            // Debug.Log("Boom");
            talkPressed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !DialogueManager.GetInstance().isDialoguePlaying()) {
            Debug.Log("Enter trigger");
            if (!DialogueManager.GetInstance().IsTriggerCalled(gameObject)) {
                 DialogueManager.GetInstance().SetTriggerCalled(gameObject);
            }
            if (DialogueManager.GetInstance().IsTriggerCalled(gameObject))
            {
                playerInRange = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {   
        if (other.gameObject.tag == "Player" && DialogueManager.GetInstance().isDialoguePlaying()) {
            Debug.Log("Exit trigger");

            if (DialogueManager.GetInstance().IsTriggerCalled(gameObject)) {
                DialogueManager.GetInstance().RemoveTriggerCalled(gameObject);
            }
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
