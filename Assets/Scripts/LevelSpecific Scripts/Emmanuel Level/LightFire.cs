using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightFire : MonoBehaviour
{
    public float interactDist;
    private PlayerActionsScript playerActionsScript;
    private Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        interactDist = Vector3.Distance(player.position, transform.position);
    }

    private void OnEnable()
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Interact.performed += Interact;
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        //print("Light Fire");
    }
}
