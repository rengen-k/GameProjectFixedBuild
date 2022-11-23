using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    private PlayerActionsScript playerActionsScript;
    private bool interactPressed;
    [SerializeField] private TextAsset inkJSON;

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
        playerActionsScript.Player.Interact.performed += Interact;
        // playerActionsScript.Player.Interact.canceled += Interact;

    }

    private void Awake()
    {
        playerInRange = false;
        interactPressed = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying) {
            playerActionsScript.Player.Disable();
        }

        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (interactPressed)
            {
                interactPressed = false;
                if (!DialogueManager.GetInstance().dialogueIsPlaying)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }

        if (!DialogueManager.GetInstance().dialogueIsPlaying) {
            interactPressed = false;
            StartCoroutine(DialogueCooldown());
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (playerInRange) {
            // Debug.Log("Boom");
            interactPressed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
