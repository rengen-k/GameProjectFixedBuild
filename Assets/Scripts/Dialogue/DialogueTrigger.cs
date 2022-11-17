using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private TextAsset inkJSON;
    private PlayerActionsScript playerActionsScript;
    private bool isTriggerText;

    private bool playerInRange;
    // Start is called before the first frame update

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

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange) 
        {
            visualCue.SetActive(true);
            isTriggerText = true;
        }
        else
        {
            visualCue.SetActive(false);
            isTriggerText = false;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!isTriggerText)
        {
            return;
        }
        Debug.Log(inkJSON.text);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
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

    
}
