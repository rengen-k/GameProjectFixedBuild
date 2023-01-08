using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//-----------------------------------------//
// DialogueTriggerAuto
//-----------------------------------------//
// Base code from https://www.youtube.com/watch?v=vY0Sk93YUhA and heavily modified.
// Automatically triggers dialogue when the player makes contact

public class DialogueTriggerAutoDestroy : MonoBehaviour
{
    private GameObject visualCue;
    private PlayerActionsScript playerActionsScript;
    [SerializeField] private TextAsset inkJSON;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
    }

    private void Update()
    {
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !DialogueManager.GetInstance().isDialoguePlaying()) {
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
            playerInRange = false;
            if (DialogueManager.GetInstance().IsTriggerCalled(gameObject)) {
                DialogueManager.GetInstance().RemoveTriggerCalled(gameObject);
            }
            
        }
    }
    
    
}
