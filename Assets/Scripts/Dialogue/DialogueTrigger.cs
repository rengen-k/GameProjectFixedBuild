using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    private PlayerActionsScript playerActionsScript;
    private bool talkPressed;
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
        playerActionsScript.Player.Talk.performed += Talk;
    }

    private void Awake()
    {
        playerInRange = false;
        talkPressed = false;
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
            if (talkPressed)
            {
                talkPressed = false;
                if (!DialogueManager.GetInstance().dialogueIsPlaying)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
            DialogueManager.GetInstance().ExitDialogueMode();

        }

        if (!DialogueManager.GetInstance().dialogueIsPlaying) {
            talkPressed = false;
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
        yield return new WaitForSeconds(0.2f);
        playerActionsScript.Player.Enable();
    }
    
}
